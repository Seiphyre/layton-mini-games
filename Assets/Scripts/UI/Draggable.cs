using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
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

    private Vector2 _offset;

    private GameObject _clone;

    // --

    public event Action<object, PointerEventData> DragStarted;
    public event Action<object, PointerEventData> DragEnded;

    private Vector2 _pointerDownPos;

    private bool _shouldDestroyOnEndDrag = true;
    private bool _useClone = false;
    private CanvasGroup _canvasGroup;

    private GameObject Target
    {
        get
        {
            if (_useClone)
                return _clone;

            return gameObject;
        }
    }

    public DropZone DropZone { get; set; }

    public static bool IsDragging = false;


    // -------------------------------------------------------------

    // Note: We are using the position given in OnPointerDown to get a better offset while dragging.
    // the position given in OnBeginDrag is the position given once the user dragged the object a little bit.
    // using the position given in OnBeginDrag result in a visual tiny "jump" of the gameobject. 
    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDownPos = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
        IsDragging = true;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, _pointerDownPos, eventData.pressEventCamera, out _offset);

        // --

        if (_useClone)
        {
            _clone = Instantiate(gameObject);
            _clone.name = $"{gameObject.name}_Clone";

            ((RectTransform)_clone.transform).sizeDelta = RectTransform.sizeDelta;

             // --

            var cloneDraggable = _clone.GetComponent<Draggable>();

            if (cloneDraggable != null)
                cloneDraggable.enabled = false;
        }

        Target.transform.SetParent(Canvas.transform);
        Target.transform.SetAsLastSibling();

        _canvasGroup = Target.AddComponent<CanvasGroup>();
        _canvasGroup.blocksRaycasts = false;

        // --

        DragStarted?.Invoke(this, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Canvas == null)
            return;

        // --

        bool insideCanvas = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 canvasLocalPos);

        if (insideCanvas)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                Target.transform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 parentLocalPos);

            Target.transform.localPosition = parentLocalPos - _offset;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_shouldDestroyOnEndDrag)
        {
            Destroy(Target);
        }

        if (_clone != null)
        {
            _clone = null;
        }

        Destroy(_canvasGroup);

        // --

        DragEnded?.Invoke(this, eventData);

        // -- 

        //StartCoroutine(ResetIsDraggingNextFrame());

        IsDragging = false;
        Debug.Log("EndDrag");
    }

    //IEnumerator ResetIsDraggingNextFrame()
    //{
    //    yield return null;

    //    IsDragging = false;
    //}

    private Vector2 CanvasLocalToParentLocal(Vector2 canvasLocalPos, RectTransform canvasRect, RectTransform parentRect)
    {
        Vector3 worldPos = canvasRect.TransformPoint(canvasLocalPos);

        Vector2 parentLocalPos = parentRect.InverseTransformPoint(worldPos);

        return parentLocalPos;
    }
}
