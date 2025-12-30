using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base component for all UI elements in the game.
/// Provides cached references and shared UI utilities.
/// </summary>
public class UIElement : MonoBehaviour
{
    private RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = (RectTransform)transform;

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

    /// <summary>
    /// Sets the size of the RectTransform in pixels.
    /// </summary>
    public void SetSize(Vector2 size)
    {
        RectTransform.sizeDelta = size;
    }

    /// <summary>
    /// Sets the size and updates the LayoutElement so that parent LayoutGroups
    /// correctly layout this UI element.
    /// </summary>
    public void SetLayoutSize(Vector2 size)
    {
        SetSize(size);

        LayoutElement le = null;
        ComponentUtils.GetOrAddComponent(this, ref le);

        le.minWidth = size.x;
        le.minHeight = size.y;
        le.preferredWidth = size.x;
        le.preferredHeight = size.y;
        le.flexibleWidth = 0f;
        le.flexibleHeight = 0f;
    }

    /// <summary>
    /// Sets the local position of this UI element.
    /// </summary>
    public void SetLocalPosition(Vector2 pos)
    {
        RectTransform.localPosition = new Vector3(pos.x, pos.y, 0);
    }

    public void SetLocalPositionPivotAware(Vector2 worldPos)
    {
        Vector2 pivotOffset = new Vector2(
            RectTransform.rect.width * (RectTransform.pivot.x - 0.5f),
            RectTransform.rect.height * (RectTransform.pivot.y - 0.5f)
        );

        RectTransform.localPosition = (Vector3)(worldPos - pivotOffset);
    }

    public void SetOpacity(float opacity)
    {
        var canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        canvasGroup.alpha = opacity;
    }
}
