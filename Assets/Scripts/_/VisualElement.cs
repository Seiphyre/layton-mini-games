using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualElement : MonoBehaviour
{
    [field: SerializeField] public Toggle SelectionToggle { get; private set; }

    // --

    private object _value;
    
    public object Value
    {
        get { return _value; }
        set
        { 
            object oldValue = _value;

            _value = value;

            if (oldValue != value)
                OnValueChanged();
        }
    }

    public event Action<object> ValueChanged;

    public bool Selected
    {
        get 
        { 
            return SelectionToggle?.isOn ?? false; 
        }

        set 
        { 
            if (SelectionToggle) 
                SelectionToggle.isOn = value; 
        }
    }

    public event Action<object> SelectionChanged;

    // --

    private Draggable _draggable;
    public Draggable Draggable
    {
        get
        {
            if (_draggable == null)
                _draggable = GetComponent<Draggable>();

            return _draggable;
        }
    }
    public bool IsDraggable
    {
        get
        {
            return (Draggable != null);
        }
    }

    // --

    private RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            return _rectTransform;
        }
    }

    // --

    protected bool _shouldRefresh = false;



    // ----------------------------------------------------

    private void OnEnable()
    {
        if (SelectionToggle != null)
            SelectionToggle.onValueChanged.AddListener(Selection_OnValueChanged);
    }

    private void OnDisable()
    {
        if (SelectionToggle != null)
            SelectionToggle.onValueChanged.RemoveListener(Selection_OnValueChanged);
    }

    protected virtual void Update()
    {
        if (_shouldRefresh)
            Refresh();
    }



    // ----------------------------------------------------

    public virtual void Refresh()
    {
        _shouldRefresh = false;
    }



    // ----------------------------------------------------

    private void Selection_OnValueChanged(bool value)
    {
        SelectionChanged?.Invoke(this);
    }
    
    protected virtual void OnValueChanged()
    {
        _shouldRefresh = true;

        ValueChanged?.Invoke(this);
    }



    // ----------------------------------------------------

    protected virtual void OnValidate()
    {
        Refresh();
    }
}
