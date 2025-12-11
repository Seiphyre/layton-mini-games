using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : InventoryItem<PieceDefinition>
{
    [Space]

    [SerializeField] private Image NormalImg1;
    [SerializeField] private Image NormalImg2;
    [SerializeField] private bool ShowNormalSprite = true;

    [Space]

    [SerializeField] private Image SimplifiedImg;
    [SerializeField] private bool ShowSimplifiedSprite = false;

    [Space]

    [SerializeField] private Image InteractionImg;
    [SerializeField] private bool InteractionSprite = false;



    // ---------------------------------------------

    public override void Refresh()
    {
        base.Refresh();

        // --

        if (Data != null)
        {
            // -- Set content view

            NormalImg1.sprite = Data.Shape.NormalSprite1;
            NormalImg1.color = Data.Color.Color;
            NormalImg1.gameObject.SetActive(ShowNormalSprite);

            NormalImg2.sprite = Data.Shape.NormalSprite2;
            NormalImg2.gameObject.SetActive(ShowNormalSprite);

            SimplifiedImg.sprite = Data.Shape.SimplifiedSprite;
            SimplifiedImg.color = Data.Color.Color;
            SimplifiedImg.gameObject.SetActive(ShowSimplifiedSprite);

            InteractionImg.sprite = Data.Shape.InteractionSprite;
            InteractionImg.color = new Color(Data.Color.Color.r, Data.Color.Color.g, Data.Color.Color.b, 0.5f);
            InteractionImg.gameObject.SetActive(InteractionSprite);

            // -- Set content size

            if (Content != null)
            {
                LayoutElement contentLayoutElement = Content.GetComponent<LayoutElement>();

                float contentMinWidth = (InnerSize * Data.Shape.Width) / ShapeData.MaxWidth;
                float contentMinHeight = (InnerSize * Data.Shape.Height) / ShapeData.MaxHeight;

                float contentPreferredWidth = (InnerSize * Data.Shape.Width) / ShapeData.MaxWidth;
                float contentPreferredHeight = (InnerSize * Data.Shape.Height) / ShapeData.MaxHeight;

                contentLayoutElement.minHeight = contentPreferredHeight;
                contentLayoutElement.minWidth = contentPreferredWidth;
                contentLayoutElement.preferredHeight = contentMinHeight;
                contentLayoutElement.preferredWidth = contentMinWidth;
                contentLayoutElement.flexibleHeight = 0;
                contentLayoutElement.flexibleWidth = 0;
            }
        }
    }
}
