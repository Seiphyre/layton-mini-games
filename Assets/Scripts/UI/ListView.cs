using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ListView : MonoBehaviour, ICollectionView
{
    // -- Source

    private IList _source;

    public IList Source
    {
        get { return _source; }
        set
        {
            var oldSource = _source;
            var newSource = value;

            _source = newSource;

            if (oldSource != value)
            {
                if (oldSource != null && oldSource is INotifyCollectionChanged prevObservableSource)
                    prevObservableSource.CollectionChanged -= Source_CollectionChanged;

                if (newSource != null && newSource is INotifyCollectionChanged nextObservableSource)
                    nextObservableSource.CollectionChanged += Source_CollectionChanged;

                if (oldSource == null)
                    Source_CollectionChanged(this, new(NotifyCollectionChangedAction.Add, newSource));
                else
                    Source_CollectionChanged(this, new(NotifyCollectionChangedAction.Replace, newSource, oldSource));
            }
        }
    }

    public event Action<object, NotifyCollectionChangedEventArgs> SourceChanged;



    // -- Items

    [SerializeField]
    private VisualElement ItemTemplate;

    [SerializeField]
    private VisualElement ItemPreviewTemplate;

    // --

    private ICollection<VisualElement> _items = new ObservableCollection<VisualElement>();
    public ICollection<VisualElement> VisualElements
    {
        get { return _items; }
    }

    public event Action<object, NotifyCollectionChangedEventArgs> ItemsChanged;



    // -- Pagination

    private Paginator _pagination;
    
    public Paginator Pagination
    {
        get
        {
            if (_pagination == null)
                _pagination = GetComponent<Paginator>();

            return _pagination;
        }
    }

    public bool HasPagination
    {
        get
        {
            return Pagination != null;
        }
    }



    // -- Selection

    private Selector _selection;

    public Selector Selection
    {
        get
        {
            if (_selection == null)
                _selection = GetComponent<Selector>();

            return _selection;
        }
    }

    public bool HasSelection
    {
        get
        {
            return Selection != null;
        }
    }



    // -- Drop Zone

    private DropZone _dropZone;

    public DropZone DropZone
    {
        get
        {
            if (_dropZone == null)
                _dropZone = GetComponent<DropZone>();

            return _dropZone;
        }
    }

    public bool HasDropZone
    {
        get
        {
            return _dropZone != null;
        }
    }

    // --

    private int _draggedItemIndex;


    private int _itemPreviewIndex;
    private int ItemPreviewIndex
    {
        get { return _itemPreviewIndex; }
        set
        {
            int oldValue = _itemPreviewIndex;

            _itemPreviewIndex = value;

            if (oldValue != value)
                _shouldRefresh = true;
        }
    }


    private object _itemPreviewValue;
    private object ItemPreviewValue
    {
        get { return _itemPreviewValue; }
        set
        {
            object oldValue = _itemPreviewValue;

            _itemPreviewValue = value;

            if (oldValue != value)
                _shouldRefresh = true;
        }
    }

    // --

    // private List<(Type sourceType, string sourcePath, Type destType, string destPath)> _bindings = new();
    // private IEqualityComparer<TValue> _comparer = EqualityComparer<TValue>.Default;

    private bool _shouldRefresh = false;



    // -------------------------------------------------------------

    private void Awake()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child == transform)
                continue;

            Destroy(child.gameObject);
        }
    }

    // --

    private void OnEnable()
    {
        if (VisualElements != null && VisualElements is INotifyCollectionChanged observableItems)
        {
            observableItems.CollectionChanged += Items_CollectionChanged;
        }

        if (Pagination != null)
        {
            Pagination.ItemPerPageChanged += Pagination_ItemPerPageChanged;
            Pagination.CurrentPageChanged += Pagination_CurrentPageChanged;
        }

        if (Selection != null)
        {
            Selection.SelectionChanged += Selection_SelectionChanged;
        }

        if (DropZone != null)
        {
            DropZone.DraggableEnter += DropZone_DraggableEnter;
            DropZone.DraggableExit += DropZone_DraggableExit;
            DropZone.DraggableMove += DropZone_DraggableMove;
            DropZone.Dropped += DropZone_Dropped;
        }

        _shouldRefresh = true;
    }

    private void OnDisable()
    {
        if (VisualElements != null && VisualElements is INotifyCollectionChanged observableItems)
        {
            observableItems.CollectionChanged -= Items_CollectionChanged;
        }

        if (Pagination != null)
        {
            Pagination.ItemPerPageChanged -= Pagination_ItemPerPageChanged;
            Pagination.CurrentPageChanged -= Pagination_CurrentPageChanged;
        }

        if (Selection != null)
        {
            Selection.SelectionChanged -= Selection_SelectionChanged;
        }

        if (DropZone != null)
        {
            DropZone.DraggableEnter -= DropZone_DraggableEnter;
            DropZone.DraggableExit -= DropZone_DraggableExit;
            DropZone.DraggableMove -= DropZone_DraggableMove;
            DropZone.Dropped -= DropZone_Dropped;
        }
    }

    // --

    private void Update()
    {
        if (_shouldRefresh)
            RefreshView();
    }



    // ------------------------------------------------------------------------

    public void RefreshView()
    {
        _shouldRefresh = false;

        // --

        if (Source == null)
            return;

        // -- Destroy all visual elements

        if (VisualElements != null)
            DestroyVisualElements(VisualElements);

        // -- Create all visual elements

        List<object> objs = HasPagination switch
        {
            true => Pagination.PageItems.ToList(),
            false => Source.OfType<object>().ToList(),
        };

        int itemPerPage = HasPagination switch
        {
            true => Pagination.ItemPerPage,
            false => objs.Count
        };

        CreateVisualElements(objs, itemPerPage);
    }

    private void CreateVisualElements(List<object> elements, int itemPerPage)
    {
        if (_itemPreviewValue != null)
            elements.Insert(_itemPreviewIndex, _itemPreviewValue);

        for (int i = 0; i < itemPerPage; i++)
        {
            object element = elements.ElementAtOrDefault(i);

            CreateVisualElement(element, i);
        }
    }

    private void CreateVisualElement(object element, int index)
    {

        bool isPreviewItem = (_itemPreviewValue != null && element == _itemPreviewValue);

        VisualElement item = Instantiate((!isPreviewItem || ItemPreviewTemplate == null) ? ItemTemplate : ItemPreviewTemplate, transform);

        // --

        if (element != null)
        {
            item.name = FormatItemName(index, FindObjectName(element));
            item.Value = element;

            //PropagateBindings(items.ElementAt(i), instance);
        }
        else
        {
            item.name = FormatItemName(index, null);
            item.Value = null;
        }

        // --

        if (HasSelection)
        {
            item.Selected = Selection.Values.Contains(item.Value);
        }

        if (item.IsDraggable)
        {
            item.Draggable.DragStarted += Draggable_DragStarted;
            item.Draggable.DragEnded += Draggable_DragEnded;
        }

        // --

        VisualElements.Add(item);
    }

    // --

    private void DestroyVisualElements(IEnumerable<VisualElement> visualElements)
    {
        var visualElementsToDestroy = visualElements.ToList();

        foreach (var visualElement in visualElementsToDestroy)
            DestroyVisualElement(visualElement);
    }

    private void DestroyVisualElement(VisualElement visualElement)
    {
        if (visualElement.IsDraggable)
        {
            visualElement.Draggable.DragStarted -= Draggable_DragStarted;
            visualElement.Draggable.DragEnded -= Draggable_DragEnded;
        }

        Destroy(visualElement.gameObject);
        VisualElements.Remove(visualElement);
    }



    // ------------------------------------------------------------------------

    private void Source_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        _shouldRefresh = true;

        SourceChanged?.Invoke(this, e);
    }

    private void Items_CollectionChanged(object obj, NotifyCollectionChangedEventArgs e)
    {
        ItemsChanged?.Invoke(this, e);
    }

    // --

    private void Pagination_CurrentPageChanged(object obj)
    {
        _shouldRefresh = true;
    }

    private void Pagination_ItemPerPageChanged(object obj)
    {
        _shouldRefresh = true;
    }

    private void Selection_SelectionChanged(object obj, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (var unselectedValue in e.OldItems)
            {
                var item = VisualElements.FirstOrDefault(it => it.Value == unselectedValue);

                if (item != null)
                    item.Selected = false;
            }
        }

        if (e.NewItems != null)
        {
            foreach (var selectedValue in e.NewItems)
            {
                var item = VisualElements.FirstOrDefault(it => it.Value == selectedValue);

                if (item != null)
                    item.Selected = true;
            }
        }
    }

    // --

    private void Draggable_DragEnded(object sender, PointerEventData e)
    {
        if (sender is Draggable draggable)
        {
            VisualElement draggedItem = draggable.GetComponent<VisualElement>();

            if (draggedItem != null)
            {
                if (draggable.DropZone != null)
                {
                    // Dropped in this drop zone
                    if (DropZone != null && draggable.DropZone == DropZone)
                    {

                    }

                    // Droppe in another drop zone
                    else
                    {

                    }
                }
                else // Dropped in void
                {
                    Source.Insert(_draggedItemIndex, draggedItem.Value);
                }
            }
        }
    }

    private void Draggable_DragStarted(object sender, PointerEventData e)
    {
        if (sender is Draggable draggable)
        {
            VisualElement draggedItem = draggable.GetComponent<VisualElement>();

            if (draggedItem != null)
            {
                _draggedItemIndex = Source.IndexOf(draggedItem.Value);

                VisualElements.Remove(draggedItem);
                Source.Remove(draggedItem.Value);
            }
        }
    }

    // --

    private void DropZone_DraggableExit(object sender, GameObject draggedGameobject)
    {
        if (draggedGameobject != null)
        {
            VisualElement draggedItem = draggedGameobject.GetComponent<VisualElement>();

            if (draggedItem != null)
            {
                ItemPreviewValue = null;
            }
        }
    }

    private void DropZone_DraggableEnter(object sender, GameObject draggedGameobject)
    {
        if (draggedGameobject != null)
        {
            VisualElement draggedItem = draggedGameobject.GetComponent<VisualElement>();

            if (draggedItem != null)
            {
                ItemPreviewValue = draggedItem.Value;
                ItemPreviewIndex = FindObjectIndex(draggedItem.Value, draggedGameobject.transform.position);
            }
        }
    }

    private void DropZone_Dropped(object sender, GameObject draggedGameobject)
    {
        if (draggedGameobject != null)
        {
            VisualElement draggedItem = draggedGameobject.GetComponent<VisualElement>();

            if (draggedItem != null)
            {
                Source.Insert(FindObjectIndex(draggedItem.Value, draggedGameobject.transform.position), draggedItem.Value);
            }
        }
    }

    private void DropZone_DraggableMove(object sender, GameObject draggedGameobject)
    {
        if (draggedGameobject != null)
        {
            VisualElement draggedItem = draggedGameobject.GetComponent<VisualElement>();

            if (draggedItem != null)
            {
                ItemPreviewIndex = FindObjectIndex(draggedItem.Value, draggedGameobject.transform.position);
            }
        }
    }

    // --

    private int FindObjectIndex(object obj, Vector2 pos)
    {
        List<GameObject> items = VisualElements.Where(it => it.Value != obj).Where(it => it.Value != null).Select(it => it.gameObject).ToList();
        int maxIndex = VisualElements.Count(it => it.Value != null);

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].transform.position.x >= pos.x)
                return i;
        }

        return items.Count;
    }



    // ------------------------------------------------------------------------

    //ICollection<object> ICollectionView.Source => Source;

    IEnumerable<VisualElement> ICollectionView.Items => VisualElements;

    ICollection ICollectionView.Source => Source;

    // --

    private string FormatItemName(int itemIndex, string objName)
    {
        string itemName = "Item {index} {name}";

        return itemName
            .Replace("{index}", itemIndex.ToString())
            .Replace("{name}", objName);
    }

    private string FindObjectName(object obj)
    {
        if (obj == null)
            return string.Empty;

        if (obj is ScriptableObject scriptableObject)
            return scriptableObject.name;

        return obj.ToString();
    }

    // ------------------------------------------------------------------------

    //public void BindProperty<TSource, TSourceValue, TDest, TDestValue>(Expression<Func<TSource, TSourceValue>> sourceMember, Expression<Func<TDest, TDestValue>> destMember)
    //{
    //    string sourcePath = null;
    //    string destPath = null;

    //    // --

    //    MemberExpression sourceMemberSelectorExpression = sourceMember.Body as MemberExpression;

    //    if (sourceMemberSelectorExpression != null)
    //    {
    //        sourcePath = sourceMember.Body.ToString();

    //        if (sourcePath.IndexOf(".") > 0)
    //            sourcePath = sourcePath.Substring(sourcePath.IndexOf(".") + 1);
    //    }

    //    // --

    //    MemberExpression destMemberSelectorExpression = destMember.Body as MemberExpression;

    //    if (destMemberSelectorExpression != null)
    //    {
    //        destPath = destMember.Body.ToString();

    //        if (destPath.IndexOf(".") > 0)
    //            destPath = destPath.Substring(destPath.IndexOf(".") + 1);
    //    }

    //    // --

    //    Type sourceType = typeof(TSource);
    //    Type destType = typeof(TDest);

    //    _bindings.Add((sourceType, sourcePath, destType, destPath));

    //    _shouldRefresh = true;
    //}

    //public void ClearBindings()
    //{
    //    _bindings.Clear();

    //    _shouldRefresh = true;
    //}

    //private void PropagateBindings(TValue source, TListViewItem destGO)
    //{
    //    foreach (var binding in _bindings)
    //    {
    //        object dest = destGO.GetComponent(binding.destType);
    //        object destValue = Utils.Reflexion.GetProperty(source, binding.sourcePath);

    //        Utils.Reflexion.SetProperty(dest, binding.destPath, destValue);
    //    }

    //    _shouldRefresh = true;
    //}
}
