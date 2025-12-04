using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class DataPresenter<T> : UIElement
{
    [SerializeField] private T _data;
    public T Data => _data;



    // ---------------------------------------------------

    public virtual void SetData(T value)
    {
        if (Equals(_data, value))
            return;

        _data = value;
        OnDataAssigned(value);
    }

    protected virtual void OnDataAssigned(T data) { }
}
