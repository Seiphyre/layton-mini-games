using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]
public class OLD_ShopItemView : VisualElement
{

    [Space]

    [SerializeField] private GameObject m_ContentPanel;
    [SerializeField] private GameObject m_ItemPanel;
    [SerializeField] private Image m_NormalImg1;
    [SerializeField] private Image m_NormalImg2;
    [SerializeField] private Image m_SimplifiedImg;

    [Space]

    [SerializeField] private bool _showNormalSprite = true;
    [SerializeField] private bool _showSimplifiedSprite = false;



    // ---------------------------------------------

    [ContextMenu("Refresh")]
    public override void Refresh()
    {
        base.Refresh();

        // --

        if (Value != null && Value is ShopItem shopItem)
        {
            m_ItemPanel.transform.localScale = GetTransformScaleFromDimensions(shopItem.Shape.Width, shopItem.Shape.Height);
            m_NormalImg1.sprite = shopItem.Shape.NormalSprite1;
            m_NormalImg1.color = shopItem.Color.Color;
            m_NormalImg2.sprite = shopItem.Shape.NormalSprite2;
            m_SimplifiedImg.sprite = shopItem.Shape.SimplifiedSprite;
            m_SimplifiedImg.color = shopItem.Color.Color;
        }

        // --

        if (m_ContentPanel != null)
            m_ContentPanel.SetActive(Value != null);

        if (m_NormalImg1 != null) m_NormalImg1?.gameObject.SetActive(_showNormalSprite);
        if (m_NormalImg2 != null) m_NormalImg2?.gameObject.SetActive(_showNormalSprite);
        if (m_SimplifiedImg != null) m_SimplifiedImg?.gameObject.SetActive(_showSimplifiedSprite);
    }

    private Vector2 GetTransformScaleFromDimensions(int width, int height)
    {
        int maxWidth = 3;
        int maxHeight = 3;

        float xScale = (width / (float)maxWidth);
        float yScale = (height / (float)maxHeight);

        return new Vector2(xScale, yScale);
    }
}
