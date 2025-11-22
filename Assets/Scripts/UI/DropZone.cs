using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IPointerMoveHandler
{
    public Action<object, GameObject> Dropped;
    public Action<object, GameObject> DraggableEnter;
    public Action<object, GameObject> DraggableExit;
    public Action<object, GameObject> DraggableMove;

    private CanvasGroup _canvasGroup;
    private bool _hovering = false;
    private bool Hovering
    {
        get { return _hovering; }
        set 
        {
            bool oldValue = _hovering;

            _hovering = value;

            if (oldValue != value)
                HoveringChanged();
        }
    }
    private GameObject _draggedObject;


    // ----------------------------------------------------

    private void Awake()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop");
        Hovering = false;
        _draggedObject = null;

        if (eventData.pointerDrag != null)
        {
            Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();

            if (draggable != null)
            {
                draggable.DropZone = this;
            }
        }

        Dropped?.Invoke(this, eventData.pointerDrag);
        OnPointerExit(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Draggable.IsDragging)
        {
            Debug.Log("PointerEnter");

            DraggableEnter?.Invoke(this, eventData.pointerDrag);

            //_draggedObject = eventData.pointerDrag;

            //bool isInside = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, eventData.position, eventData.pressEventCamera);

            //if (isInside)
            //    Hovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            Debug.Log("PointerExit");

            DraggableExit?.Invoke(this, eventData.pointerDrag);

            //bool isInside = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, eventData.position, eventData.pressEventCamera);

            //if (!isInside)
            //{
            //    Hovering = false;
            //    _draggedObject = null;
            //}
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            DraggableMove?.Invoke(this, eventData.pointerDrag);
        }
    }

    private void HoveringChanged()
    {
        //if (Hovering)
        //{
        //    DraggableEnter?.Invoke(this, _draggedObject);
        //    //Debug.Log("DraggableEnter");
        //}
        //else
        //{
        //    DraggableExit?.Invoke(this, _draggedObject);
        //    //Debug.Log("DraggableExit");
        //}
    }

    //private bool IsValidDrop(GameObject droppedObject)
    //{

    //}
}
