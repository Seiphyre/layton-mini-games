using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem<T> : DataPresenter<T>
{
    [Space]

    [SerializeField] protected int Size = 192;
    protected int InnerSize => Size - (2 * OutlineThickness);

    protected LayoutElement LayoutElement => GetComponent<LayoutElement>();
    protected LayoutGroup Layout => GetComponent<LayoutGroup>();

    [Space]

    [SerializeField] protected Image BackgroundImg;
    [SerializeField] protected Color EmptyBackgroundColor;
    [SerializeField] protected Color OccupiedBackgroundColor;

    [Space]

    [SerializeField] protected Outline Outline;
    [SerializeField] protected Color OutlineColor;
    [SerializeField] protected int OutlineThickness;

    protected LayoutElement OutlineLayoutElement => Outline?.GetComponent<LayoutElement>();
    protected RectTransform OutlineRectTransform => Outline?.GetComponent<RectTransform>();

    [Space]

    [SerializeField] protected GameObject Content;
    [SerializeField] private TextAnchor ContentAlignment = TextAnchor.MiddleCenter;

    protected override void OnDataAssigned(T data)
    {
        base.OnDataAssigned(data);

        Refresh();
    }

    public virtual void Refresh()
    {
        // -- Transform / LayoutElement

        RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        RectTransform.sizeDelta = new Vector2(Size, Size);

        if (LayoutElement != null)
        {
            LayoutElement.minHeight = Size;
            LayoutElement.minWidth = Size;
            LayoutElement.preferredHeight = Size;
            LayoutElement.preferredWidth = Size;
            LayoutElement.flexibleHeight = 0;
            LayoutElement.flexibleWidth = 0;
        }

        // -- Layout

        if (Layout != null)
        {
            Layout.padding = new RectOffset(OutlineThickness, OutlineThickness, OutlineThickness, OutlineThickness);
            Layout.childAlignment = ContentAlignment;
        }

        // -- Background

        BackgroundImg.color = (Data != null) ? OccupiedBackgroundColor : EmptyBackgroundColor;

        // -- Outline

        Outline.effectColor = OutlineColor;
        Outline.effectDistance = new Vector2(OutlineThickness, OutlineThickness);

        if (OutlineLayoutElement != null)
        {
            OutlineLayoutElement.minHeight = InnerSize;
            OutlineLayoutElement.minWidth = InnerSize;
            OutlineLayoutElement.preferredHeight = InnerSize;
            OutlineLayoutElement.preferredWidth = InnerSize;
            OutlineLayoutElement.flexibleHeight = 0;
            OutlineLayoutElement.flexibleWidth = 0;
        }

        if (OutlineLayoutElement == null || OutlineLayoutElement.ignoreLayout)
        {
            OutlineRectTransform.anchorMin = Vector2.zero;
            OutlineRectTransform.anchorMax = Vector2.one;
            OutlineRectTransform.offsetMin = new Vector2(OutlineThickness, OutlineThickness);
            OutlineRectTransform.offsetMax = -1 * new Vector2(OutlineThickness, OutlineThickness);
        }

        // -- Content

        Content.SetActive(Data != null);
    }
}
