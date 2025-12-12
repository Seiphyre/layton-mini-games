using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// - Generic component for any draggable element (not specific to this game).
/// - Raises events:
///   - OnBeginDrag
///   - OnDrag
///   - OnEndDrag
/// - Doesn't know game rules.
///
/// </summary>
public class Draggable : UIElement, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform dragRectTransform;

    [Space]

    public UnityEvent onDragStart;
    public UnityEvent<Vector2> onDragging;
    public UnityEvent<DropZone> onDragEnd;

    // --

    private Vector2 _offset;
    private CanvasGroup _canvasGroup;
    private bool _originalBlocksRaycasts;
    private Transform _originalParent;
    private int _originalSiblingIndex;

    public bool IsDragging { get; private set; } = false;

    public object Payload { get; set; }
    public DropZone DropZone { get; set; }



    // -------------------------------------------------
    private void Awake()
    {
        Payload = null;

        ComponentUtils.AssignIfNull(this, ref dragRectTransform);
        ComponentUtils.GetOrAddComponent(dragRectTransform, ref _canvasGroup);
    }

    // -----------------------------
    // Calculate offset on pointer down
    // -----------------------------
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Canvas == null) 
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragRectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out _offset
        );
    }

    // -----------------------------
    // Start drag after threshold
    // -----------------------------
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Canvas == null) 
            return;

        IsDragging = true;

        _originalParent = dragRectTransform.parent;
        _originalSiblingIndex = dragRectTransform.GetSiblingIndex();
        dragRectTransform.SetParent(Canvas.transform);
        dragRectTransform.SetAsLastSibling();

        _originalBlocksRaycasts = _canvasGroup.blocksRaycasts;
        _canvasGroup.blocksRaycasts = false;

        onDragStart?.Invoke();
    }

    // -----------------------------
    // Drag movement
    // -----------------------------
    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDragging || Canvas == null) 
            return;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                Canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint))
        {
            dragRectTransform.localPosition = localPoint - _offset;
            onDragging?.Invoke(dragRectTransform.localPosition);
        }

        DropZone = null;
    }

    // -----------------------------
    // End drag
    // -----------------------------
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsDragging) 
            return;

        IsDragging = false;

        if (DropZone == null)
        {
            dragRectTransform.SetParent(_originalParent);
            dragRectTransform.SetSiblingIndex(_originalSiblingIndex);
        }
        else
        {
            dragRectTransform.SetParent(DropZone.transform);
            dragRectTransform.SetAsLastSibling();
        }

        _canvasGroup.blocksRaycasts = _originalBlocksRaycasts;

        Payload = null;

        onDragEnd?.Invoke(DropZone);
    }
}
