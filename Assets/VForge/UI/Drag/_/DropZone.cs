using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 
/// - Generic drop target.
/// - Checks if a draggable is compatible.
/// - Fires events:
///   - OnDropValid
///   - OnDropInvalid
///
/// </summary>
public class DropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IPointerMoveHandler
{
    [Space]

    public UnityEvent<Draggable> onEnter;
    public UnityEvent<Draggable> onMove;
    public UnityEvent<Draggable> onExit;
    public UnityEvent<Draggable> onDropped;
    public UnityEvent<Draggable> onDropRejected;

    private readonly List<Func<object, bool>> _validationRules = new();
    private bool _isPointerInside = false;



    // --------- Validation rules ----------

    public void AddValidationRule(Func<object, bool> rule)
    {
        _validationRules.Add(rule);
    }

    public void ClearValidationRules()
    {
        _validationRules.Clear();
    }



    // ----------------------------------------------------

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TryGetDragged(eventData, out var draggable, out object payload))
        {
            if (!IsValid(payload)) return;

            _isPointerInside = true;

            onEnter?.Invoke(draggable);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!_isPointerInside) return;

        if (TryGetDragged(eventData, out var draggable, out object payload))
        {
            if (!IsValid(payload)) return;

            onMove?.Invoke(draggable);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TryGetDragged(eventData, out var draggable, out object payload))
        {
            if (!IsValid(payload)) return;

            _isPointerInside = false;

            onExit?.Invoke(draggable);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (TryGetDragged(eventData, out var draggable, out object payload))
        {
            if (IsValid(payload))
            {
                //draggable.transform.SetParent(transform, false);
                //draggable.transform.SetAsLastSibling();

                draggable.DropZone = this;

                onDropped?.Invoke(draggable);
            }
            else
            {
                onDropRejected?.Invoke(draggable);
            }

            OnPointerExit(eventData);
        }

        _isPointerInside = false;
    }



    // -------------------------------------------------------

    private bool IsValid(object payload)
    {
        foreach (var rule in _validationRules)
        {
            if (!rule(payload))
                return false;
        }

        return true;
    }

    private bool TryGetDragged(PointerEventData eventData, out Draggable draggable, out object payload)
    {
        payload = null;
        draggable = null;

        if (eventData.pointerDrag == null)
            return false;

        draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable == null || !draggable.IsDragging)
            return false;

        payload = draggable.Payload;

        return true;
    }
}
