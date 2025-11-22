using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualShopItem : VisualElement
{
    [Space]

    [SerializeField] private Image m_NormalImg1;
    [SerializeField] private Image m_NormalImg2;
    [SerializeField] private Image m_SimplifiedImg;
    [SerializeField] private Image m_InteractionImg;

    [Space]

    [SerializeField] private bool _showNormalSprite = true;
    [SerializeField] private bool _showSimplifiedSprite = false;
    [SerializeField] private bool _showInteractionSprite = false;



    // ---------------------------------------------

    public override void Refresh()
    {
        base.Refresh();

        // --

        if (Value != null && Value is ShopItem shopItem)
        {
            m_NormalImg1.sprite = shopItem.Shape.NormalSprite1;
            m_NormalImg1.color = shopItem.Color.Color;
            m_NormalImg2.sprite = shopItem.Shape.NormalSprite2;
            m_SimplifiedImg.sprite = shopItem.Shape.SimplifiedSprite;
            m_SimplifiedImg.color = shopItem.Color.Color;
            m_InteractionImg.sprite = shopItem.Shape.InteractionSprite;
            m_InteractionImg.color = new Color(shopItem.Color.Color.r, shopItem.Color.Color.g, shopItem.Color.Color.b, 0.5f);
        }

        // --

        if (m_NormalImg1 != null) m_NormalImg1?.gameObject.SetActive(_showNormalSprite);
        if (m_NormalImg2 != null) m_NormalImg2?.gameObject.SetActive(_showNormalSprite);
        if (m_SimplifiedImg != null) m_SimplifiedImg?.gameObject.SetActive(_showSimplifiedSprite);
        if (m_InteractionImg != null) m_InteractionImg?.gameObject.SetActive(_showInteractionSprite);
    }
}
