using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

using UnityEngine;



public class Selector : MonoBehaviour
{
    [SerializeField] private SelectionMode m_selectionMode = SelectionMode.SingleSelection;
    public SelectionMode SelectionMode
    {
        get { return m_selectionMode; }
        set
        {
            SelectionMode prevValue = m_selectionMode;

            m_selectionMode = value;

            if (prevValue != value)
            {
                Values.RemoveAll();

                switch (value)
                {
                    case SelectionMode.SingleSelection:
                        Values.Add(SingleSelection);
                        break;

                    case SelectionMode.MultiSelection:
                        Values.AddAll(MultipleSelection);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }

    // --

    private object _singleSelection;

    private object SingleSelection
    {
        get { return _singleSelection; }
        set 
        {
            object prevValue = _singleSelection;

            _singleSelection = value; 

            if (prevValue != value)
                SingleSelection_ValueChanged(prevValue, value);
        }
    }

    private event Action<object> SingleSelectionChanged;

    // --

    private ICollection<object> _multipleSelection = new ObservableCollection<object>();
    
    private ICollection<object> MultipleSelection
    {
        get { return _multipleSelection; }
    }

    private event Action<object, NotifyCollectionChangedEventArgs> MultipleSelectionChanged;

    // --

    private ICollection<object> _values = new ObservableCollection<object>();
    public ICollection<object> Values
    {
        get { return _values; }
    }

    public event Action<object, NotifyCollectionChangedEventArgs> SelectionChanged;



    // ----------------------------------------------

    private void OnEnable()
    {
        if (Values != null)
        {
            ((INotifyCollectionChanged)Values).CollectionChanged += Values_CollectionChanged;
        }

        if (MultipleSelection != null)
        {
            ((INotifyCollectionChanged)MultipleSelection).CollectionChanged += MultipleSelection_CollectionChanged;
        }
    }

    private void OnDisable()
    {
        if (Values != null)
        {
            ((INotifyCollectionChanged)Values).CollectionChanged -= Values_CollectionChanged;
        }

        if (MultipleSelection != null)
        {
            ((INotifyCollectionChanged)MultipleSelection).CollectionChanged -= MultipleSelection_CollectionChanged;
        }
    }



    // ----------------------------------------------

    public void Select(object obj)
    {
        SetSelection(obj, true);
    }

    public void Select(IEnumerable<object> objs)
    {
        var itemsToSelect = objs.ToList();

        foreach (var item in itemsToSelect)
            Select(item);
    }

    public void Unselect(object obj)
    {
        SetSelection(obj, false);
    }

    public void Unselect(IEnumerable<object> objs)
    {
        var itemsToUnselect = objs.ToList();

        foreach (var item in itemsToUnselect)
            Unselect(item);
    }

    private void SetSelection(object obj, bool isSelected)
    {
        if (SelectionMode == SelectionMode.SingleSelection)
        {
            if (isSelected && SingleSelection != obj)
            {
                SingleSelection = obj;
            }

            else if (!isSelected && SingleSelection == obj)
            {
                SingleSelection = null;
            }
        }
        else
        {
            if (isSelected && !MultipleSelection.Contains(obj))
            {
                MultipleSelection.Add(obj);
            }

            if (!isSelected && MultipleSelection.Contains(obj))
            {
                MultipleSelection.Remove(obj);
            }
        }
    }


    // ----------------------------------------------

    private void Values_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        SelectionChanged?.Invoke(this, e);
    }
    
    private void MultipleSelection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            Values.RemoveAll(e.OldItems.OfType<object>());
        }

        if (e.NewItems != null)
        {
            Values.AddAll(e.NewItems.OfType<object>());
        }

        MultipleSelectionChanged?.Invoke(this, e);
    }

    private void SingleSelection_ValueChanged(object oldObj, object newObj)
    {
        Values.RemoveAll();

        if (newObj != null)
            Values.Add(newObj);

        SingleSelectionChanged?.Invoke(this);
    }
}
