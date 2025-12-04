using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selectable : UIElement
{
    private bool _value = false;

    public bool Value 
    { 
        get 
        { 
            return _value; 
        } 
        set 
        {
            if (_value == value) return; 

            _value = value;

            onValueChanged(this, value);
        } 
    }

    public event Action<Selectable, bool> onValueChanged;



    // ----------------------------------------------------

    public void Toggle()
    {
        Value = !Value;
    }
}
