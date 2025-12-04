using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base component for all UI components
/// </summary>
public class UIElement : MonoBehaviour
{
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

    private Canvas _canvas;
    public Canvas Canvas
    {
        get
        {
            if (_canvas == null)
                _canvas = GetComponentInParent<Canvas>();

            return _canvas;
        }
    }
}
