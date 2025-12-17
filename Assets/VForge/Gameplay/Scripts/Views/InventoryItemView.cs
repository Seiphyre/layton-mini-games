using UnityEngine;
using UnityEngine.UI;
using VForge.BoardPieces.Definitions;
using VForge.Inventories;

namespace VForge.Gameplay
{
    public class InventoryItemView : DataPresenter<InventoryItem<PieceDefinition>>
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image background;

        public override void SetData(InventoryItem<PieceDefinition> item)
        {
            if (item == null || item.Data == null)
            {
                if (background != null)
                {
                    background.color = Color.white;
                    background.raycastTarget = false;
                }

                return;
            }

            gameObject.SetActive(true);

            var def = item.Data;

            if (background != null)
            {
                background.color = def.Style.Color;
                background.raycastTarget = true;
            }

            // optional later:
            // icon.sprite = def.Style.Icon;
        }
    }
}