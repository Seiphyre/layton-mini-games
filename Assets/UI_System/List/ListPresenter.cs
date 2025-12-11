using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

using UnityEngine;

/// <summary>
/// 
/// - Generic list controller that:
///   - Binds a collection of data (List<T>) to a set of UI items (ListItem).
///   - Handles pagination via Pagination (optional).
///   - Handles reordering via Draggable items + Dropzone (optional)
///   - Handles add/remove items via drag gestures (allow external drag)
///   
/// </summary>
/// <typeparam name="T"></typeparam>


public abstract class ListPresenter<T> : UIElement
{

    [SerializeField] private ListData<T> listData;

    [SerializeField] private Transform itemContainer;

    [SerializeField] private GameObject itemTemplate;

    [SerializeField] private Pagination pagination;

    [SerializeField] private DropZone dropZone;



    // underlying list & observable wrapper

    private IList<T> _list;
    public IList<T> List => _list;

    private ObservableCollection<T> _observable;



    // runtime items representing visible real items (no ghost items included here)

    private readonly List<ListItem<T>> _items = new();
    private readonly List<ListItem<T>> _ghostItems = new();



    // state during dragging

    private ListItem<T> _previewItem = null;
    private int _previewVirtualIndex = -1; // index inside visible real items where preview is currently placed

    private ListItem<T> _draggedItem = null;
    private T _draggedData = default;
    private int _draggedSourceIndex = -1;   // absolute index in _list before removal
    private int _draggedItemVirtualIndex = -1;   // absolute index in _list before removal



    // -------------------------------------------------------------

    private void Awake()
    {
        if (_list == null && listData != null && listData.Items != null)
            SetList(listData.Items);
    }

    // --

    private void OnEnable()
    {
        SubscribeToObservable();

        if (pagination != null)
            pagination.onPageChanged.AddListener(OnListAssigned);

        if (dropZone != null)
        {
            dropZone.onEnter.AddListener(DropZone_DraggableEnter);
            dropZone.onExit.AddListener(DropZone_DraggableExit);
            dropZone.onDropped.AddListener(DropZone_Dropped);
            //dropZone.onDropRejected.AddListener(DropZone_DropRejected);
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromObservable();

        if (pagination != null)
            pagination.onPageChanged.RemoveListener(OnListAssigned);

        if (dropZone != null)
        {
            dropZone.onEnter.RemoveListener(DropZone_DraggableEnter);
            dropZone.onExit.RemoveListener(DropZone_DraggableExit);
            dropZone.onDropped.RemoveListener(DropZone_Dropped);
            //dropZone.onDropRejected.RemoveListener(DropZone_DropRejected);
        }
    }



    // ------------------------------------------------------------------------

    public void SetList(IList<T> value)
    {
        UnsubscribeFromObservable();

        _list = value;
        _observable = _list as ObservableCollection<T>;

        SubscribeToObservable();

        OnListAssigned();
    }



    // ------------------------------------------------------------------------

    protected virtual void PopulateItems()
    {
        if (_draggedItem != null || _previewItem != null)
            return; // avoid repopulating during drag & drop

        // Destroy existing items

        foreach (var item in _items)
            DestroyItem(item);

        _items.Clear();

        foreach (var ghost in _ghostItems)
            DestroyItem(ghost);

        _ghostItems.Clear();

        // Handle empty list

        if (_list == null || _list.Count == 0)
        {
            if (pagination != null)
            {
                // display full page of ghosts if empty list
                CreateGhostItems(pagination.ItemsPerPage);
            }

            return;
        }

        // Instantiate real items

        int startIndex = 0;
        int endIndex = _list.Count;

        if (pagination != null)
        {
            pagination.SetTotalItems(_list.Count);
            (startIndex, endIndex) = pagination.GetVisibleRange(false);
        }

        CreateRealItems(startIndex, endIndex);

        // Instantiate ghost items to fill remaining slots on the page

        if (pagination != null)
        {
            int visibleCount = endIndex - startIndex;
            int ghostCount = pagination.ItemsPerPage - visibleCount;

            CreateGhostItems(ghostCount);
        }
    }

    private void PaginateItems()
    {
        if (pagination == null)
            return;

        int itemCount = CountItems();
        int itemsPerPage = pagination.ItemsPerPage;
        int missingItemsCount = Mathf.Max(0, itemsPerPage - itemCount);
        int extraItemsCount = Mathf.Max(0, itemCount - itemsPerPage);

        for (int i = 0; i < missingItemsCount; i++)
            CreateMissingItem();

        for (int i = 0; i < extraItemsCount; i++)
            DestroyExtraItem();
    }

    // --

    protected virtual void CreateRealItems(int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            CreateRealItem(_list[i]);
        }
    }

    protected virtual void CreateGhostItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateGhostItem();
        }
    }

    // --

    protected virtual ListItem<T> CreateItem(T data)
    {
        if (itemTemplate == null)
        {
            Debug.LogWarning($"Cannot create item. Missing reference to {nameof(itemTemplate)}");
            return null;
        }

        if (itemContainer == null)
        {
            Debug.LogWarning($"Cannot create item. Missing reference to {nameof(itemContainer)}");
            return null;
        }

        // --

        var item = new ListItem<T>()
        {
            Data = data,
        };

        // -- Setup GameObject

        item.GameObject = Instantiate(itemTemplate, itemContainer);

        item.GameObject.SetActive(true);

        // -- Setup Draggable (if any)

        item.Draggable = item.GameObject.GetComponent<Draggable>();

        if (item.Draggable != null)
        {
            // wire actions so we can remove listeners later

            item.OnItemBeginDrag = () => Item_OnDragBegin(item);
            item.OnItemEndDrag = (dropzone) => Item_OnDragEnd(item);

            // Add UnityEvent listeners

            item.Draggable.onDragStart.AddListener(item.OnItemBeginDrag);
            item.Draggable.onDragEnd.AddListener(item.OnItemEndDrag);
        }

        // Bind data to presenter

        UpdateItem(item);

        // -- 

        return item;
    }

    protected virtual ListItem<T> CreateRealItem(T data)
    {
        var realItem = CreateItem(data);
        realItem.GameObject.name = ObjectUtils.FindName(data);

        _items.Add(realItem);

        return realItem;
    }

    protected virtual ListItem<T> CreateGhostItem()
    {
        var ghostItem = CreateItem(default);
        ghostItem.GameObject.name = $"Ghost Item";

        _ghostItems.Add(ghostItem);

        return ghostItem;
    }

    protected virtual ListItem<T> CreatePreviewItem(T data)
    {
        var previewItem = CreateItem(data);
        previewItem.GameObject.name = "Preview Item";

        // Make preview visually distinct (semi-transparent)
        var canvasGroup = ComponentUtils.GetOrAddComponent<CanvasGroup>(previewItem.GameObject);

        canvasGroup.alpha = 0.6f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Disable draggable on preview so it doesn't interfere
        if (previewItem.Draggable != null)
        {
            previewItem.Draggable.onDragStart.RemoveListener(previewItem.OnItemBeginDrag);
            previewItem.Draggable.onDragEnd.RemoveListener(previewItem.OnItemEndDrag);
            previewItem.Draggable.enabled = false;
            previewItem.Draggable = null;
        }

        return previewItem;
    }

    protected virtual ListItem<T> CreateMissingItem()
    {
        if (pagination == null)
            throw new InvalidOperationException("Pagination is required for FillItem creation.");

        // last page → Create ghost item
        if (pagination.IsLastPage)
        {
            return CreateGhostItem();
        }

        // Not last page → Get the first item of the next page
        (int start, int end) = pagination.GetVisibleRange();

        int nextPageFirstIndex = end + 1;
        T nextPageFirstData = _list[nextPageFirstIndex];

        return CreateRealItem(nextPageFirstData);
    }

    // --

    protected virtual void DestroyItem(ListItem<T> item)
    {
        if (item == null)
        {
            Debug.LogWarning($"Cannot destroy item. {nameof(item)} is null.");
            return;
        }

        if (item == null)
        {
            Debug.LogWarning($"Cannot destroy item. {nameof(item.GameObject)} is null.");
            return;
        }

        // --

        item.GameObject.SetActive(false);
        item.GameObject.transform.SetAsLastSibling(); // so it doesnt disturb other sibling during the destruction frame. (especially insertion)

        if (item.Draggable != null)
        {
            item.Draggable.onDragStart.RemoveListener(item.OnItemBeginDrag);
            item.Draggable.onDragEnd.RemoveListener(item.OnItemEndDrag);
        }

        Destroy(item.GameObject);
    }

    protected virtual bool DestroyExtraItem()
    {
        if (_ghostItems.Any())
        {
            var ghostItem = _ghostItems.Last();

            DestroyItem(ghostItem);
            _ghostItems.Remove(ghostItem);

            return true;
        }

        else if (_items.Any())
        {
            var realItem = _items.Last();

            DestroyItem(realItem);
            _items.Remove(realItem);

            return true;
        }

        return false;
    }

    // --

    protected virtual void UpdateItem(ListItem<T> item)
    {
        var presenter = item.GameObject.GetComponent<DataPresenter<T>>();

        if (presenter != null)
            presenter.SetData(item.Data);
    }



    // ------------------------------------------------------------------------

    protected virtual void OnListAssigned()
    {
        PopulateItems();
    }



    // ------------------------------------------------------------------------

    private void SubscribeToObservable()
    {
        if (_observable != null)
        {
            // Ensure we don't subscribe multiple times
            _observable.CollectionChanged -= OnListChanged;
            _observable.CollectionChanged += OnListChanged;
        }
    }

    private void UnsubscribeFromObservable()
    {
        if (_observable != null)
        {
            _observable.CollectionChanged -= OnListChanged;
        }
    }

    protected virtual void OnListChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        PopulateItems();
    }



    // ------------------------------------------------------------------------
    // Drag lifecycle from items (internal drags)
    // ------------------------------------------------------------------------

    private void Item_OnDragBegin(ListItem<T> item)
    {
        // Drag begins from this list's item:
        if (item == null || item.Data == null || _list == null)
            return;

        // 0. Set payload

        item.Draggable.Payload = item.Data;

        // 1. store state

        _draggedItem = item;
        _draggedData = item.Data;
        _draggedSourceIndex = SourceIndexOf(item.Data);
        _draggedItemVirtualIndex = VirtualIndexOf(item.Data);

        // 2. remove the visual item from _items list

        _items.Remove(item);

        item.GameObject.transform.SetParent(Canvas.transform, false);
        item.GameObject.transform.SetAsLastSibling();

        // 3. create preview only if this list has a dropZone (we show preview only on lists that accept drops)
        if (dropZone != null)
        {
            _previewItem = CreatePreviewItem(_draggedData);
            _previewVirtualIndex = VirtualIndexOf(_draggedData);

            InsertPreviewAtVirtualIndex(_previewVirtualIndex);
        }
        else
        {
            // no preview; user sees floating draggable from Draggable component
            _previewItem = null;
            _previewVirtualIndex = -1;
        }

        // if no preview, but pagination is required, we must adjust the number of visible items 
        if (pagination != null)
            PaginateItems();
    }

    private void Item_OnDragEnd(ListItem<T> item)
    {
        if (_previewItem != null)
        {
            DestroyItem(_previewItem);
            _previewItem = null;
            _previewVirtualIndex = -1;
        }

        if (_draggedItem != null)
        {
            // Cancel drop
            if (_draggedItem.Draggable.DropZone == null)
            {
                // Source list: no update.

                // Item list: the item exist, we only need to insert it back to the list
                _items.Insert(_draggedItemVirtualIndex, _draggedItem);

                item.GameObject.transform.SetParent(itemContainer, false);
                item.GameObject.transform.SetSiblingIndex(_draggedItemVirtualIndex);
            }

            // Dragged into a dropzone
            else
            {
                // Source list: we must remove data from the list.
                if (_list != null)
                    _list.RemoveAt(_draggedSourceIndex);

                // Item list: destroy the item.
                DestroyItem(_draggedItem);
            }

            _draggedItem = null;
            _draggedData = default;
            _draggedSourceIndex = -1;
            _draggedItemVirtualIndex = -1;
        }

        if (pagination != null)
            PaginateItems();

        //Debug.Log($"{string.Join(" => ", _list.Cast<PieceDefinition>().Select(item => $"{item.Color.Name} {item.Type.Name}"))}");
    }



    // ------------------------------------------------------------------------

    private void DropZone_DraggableEnter(Draggable draggable)
    {
        if (!(draggable.Payload is T payload))
            return;

        if (draggable != null)
            draggable.onDragging.AddListener(UpdatePreviewPositionFromCanvasLocal);

        // If we don't have a preview and dropZone exists, create one for external drags
        if (_previewItem == null)
        {
            _previewItem = CreatePreviewItem(payload);
            _previewVirtualIndex = -1;
        }

        if (pagination != null)
            PaginateItems();
    }

    private void DropZone_DraggableExit(Draggable draggable)
    {
        if (!(draggable.Payload is T payload))
            return;

        // Unsubscribe from dragging
        if (draggable != null)
            draggable.onDragging.RemoveListener(UpdatePreviewPositionFromCanvasLocal);

        if (_previewItem != null)
        {
            DestroyItem(_previewItem);

            _previewItem = null;
            _previewVirtualIndex = -1;
        }

        if (pagination != null)
            PaginateItems();

        // If dragging internal and user left the zone, keep the preview but mark as "outside" visually if you want.
        // If you want to hide the preview while outside, you can DestroyPreview() here.
        // For UX we keep preview but we do not accept a drop unless drop happens inside this zone.
    }

    private void DropZone_Dropped(Draggable draggable)
    {
        if (!(draggable.Payload is T payload))
            return;

        // Compute absolute insertion index (visible pages only)
        var (visibleStart, visibleEnd) = GetVisibleRangeSafe();

        int virtualIndex = Mathf.Clamp(_previewVirtualIndex, 0, GetVisibleRealCount());
        int sourceIndex = visibleStart + virtualIndex;

        // Insert the payload into the source list
        if (_list != null)
        {
            _list.Insert(sourceIndex, payload);

            if (pagination != null)
                pagination.SetTotalItems(_list.Count);
        }

        // Manually create a new Item
        var newItem = CreateItem(payload);

        _items.Insert(_previewVirtualIndex, newItem);

        newItem.GameObject.transform.SetParent(itemContainer, false);
        newItem.GameObject.transform.SetSiblingIndex(_previewVirtualIndex);

        // --

        if (_draggedData != null && _draggedSourceIndex > sourceIndex)
        {
            // We must shift the index because the dropped item as been insert before, so this index will shift right (increment)
            _draggedSourceIndex++;
        }
    }

    private void DropZone_DropRejected(Draggable draggable)
    {
        
    }



    // ------------------------------------------------------------------------
    // Utilities: Preview Item
    // ------------------------------------------------------------------------



    /// <summary>
    /// Place the preview GameObject at the requested relative index among visible real items
    /// </summary>
    private void InsertPreviewAtVirtualIndex(int virtualIndex)
    {
        if (_previewItem == null)
            return;

        // clamp
        var (visibleStart, visibleEnd) = GetVisibleRangeSafe();
        int visibleCount = Mathf.Max(0, visibleEnd - visibleStart);
        int clampedRelativeIndex = Mathf.Clamp(virtualIndex, 0, visibleCount);

        // Determine sibling index in the container:
        // if preview is to be inserted before item i (relativeIndex == i): siblingIndex = index of _items[i] in container
        // if preview is appended at end: siblingIndex = after last real item's sibling index
        int siblingIndex = itemContainer.childCount; // default append

        if (clampedRelativeIndex < _items.Count && _items[clampedRelativeIndex] != null && _items[clampedRelativeIndex].GameObject != null)
        {
            siblingIndex = _items[clampedRelativeIndex].GameObject.transform.GetSiblingIndex();
        }
        else
        {
            // place after last real item on the page (or at start if no real items)
            if (_items.Count > 0)
            {
                var last = _items[_items.Count - 1];
                siblingIndex = last.GameObject.transform.GetSiblingIndex() + 1;
            }
            else
            {
                // when no real items on page, we place preview at the end of container (or first)
                siblingIndex = 0;
            }
        }

        // set preview's parent and sibling index
        _previewItem.GameObject.transform.SetParent(itemContainer, false);
        _previewItem.GameObject.transform.SetSiblingIndex(siblingIndex);
    }


    /// <summary>
    /// Called repeatedly while dragging to compute insertion index and move preview accordingly
    /// canvasLocalPosition: draggable's RectTransform.localPosition relative to Canvas.transform
    /// </summary>
    private void UpdatePreviewPositionFromCanvasLocal(Vector2 canvasLocalPosition)
    {
        if (_previewItem == null)
            return;

        // convert canvas local -> world -> itemContainer local
        var canvasTransform = Canvas != null ? Canvas.transform : null;
        if (canvasTransform == null)
            return;

        // get world pos from canvas local
        Vector3 worldPos = canvasTransform.TransformPoint(new Vector3(canvasLocalPosition.x, canvasLocalPosition.y, 0f));
        Vector3 localInContainer = itemContainer.InverseTransformPoint(worldPos);

        // Determine insertion relative index among visible real items
        int newRelativeIndex = ComputeInsertionIndexFromLocalPosition(localInContainer);

        // If pointer is over ghost area (beyond last real item), clamp to end of real items
        var (visibleStart, visibleEnd) = GetVisibleRangeSafe();
        int visibleRealCount = visibleEnd - visibleStart;
        newRelativeIndex = Mathf.Clamp(newRelativeIndex, 0, visibleRealCount);

        if (newRelativeIndex != _previewVirtualIndex)
        {
            _previewVirtualIndex = newRelativeIndex;
            InsertPreviewAtVirtualIndex(_previewVirtualIndex);
        }
    }


    /// <summary>
    /// Computes insertion index (relative to the visible real items) based on a local position in itemContainer.
    /// Assumes list is ordered top to bottom. This implementation compares the world Y of item centers against pointer Y.
    /// If you use a horizontal layout, change this logic accordingly.
    /// </summary>
    private int ComputeInsertionIndexFromLocalPosition(Vector3 localInContainer)
    {
        // Convert localInContainer back to world for simpler comparison with TransformPoint centers
        Vector3 pointerWorld = itemContainer.TransformPoint(localInContainer);

        for (int i = 0; i < _items.Count; i++)
        {
            var it = _items[i];
            if (it == null || it.GameObject == null) continue;

            var rt = it.GameObject.transform as RectTransform;
            if (rt == null) continue;

            // item center world position
            Vector3 itemCenterWorld = rt.TransformPoint(rt.rect.center);

            // If pointer is above the item center (higher Y), we want to insert before this item.
            // This assumes list top has higher Y (standard Unity UI when anchored top). If your layout differs,
            // invert the sign or use X axis for horizontal lists.
            if (pointerWorld.x < itemCenterWorld.x)
            {
                return i;
            }
        }

        // If pointer is not above any item, insertion at end of visible real items
        return _items.Count;
    }



    // ------------------------------------------------------------------------
    // Utilities: other
    // ------------------------------------------------------------------------

    private int CountItems()
    {
        return _items.Count + _ghostItems.Count + (_previewItem != null ? 1 : 0);
    }

    private int GetVisibleRealCount()
    {
        var (start, end) = GetVisibleRangeSafe();
        return Mathf.Max(0, end - start);
    }

    private (int start, int end) GetVisibleRangeSafe()
    {
        if (pagination == null)
        {
            if (_list == null)
                return (0, 0);

            return (0, _list.Count);
        }

        return pagination.GetVisibleRange(false);
    }

    protected virtual int SourceIndexOf(T data)
    {
        return _list.IndexOf(data);
    }

    protected virtual int VirtualIndexOf(T data)
    {
        int sourceIndex =  SourceIndexOf(data);

        var (visibleStart, visibleEnd) = GetVisibleRangeSafe();
        int virtualIndex = Mathf.Clamp(sourceIndex - visibleStart, 0, Mathf.Max(0, visibleEnd - visibleStart));

        return virtualIndex;
    }
}
