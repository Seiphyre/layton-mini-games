using UnityEngine;
using UnityEngine.UI;
using VForge.BoardPieces.Definitions;
using VForge.Inventories;
using VForge.Inventories.UI;

namespace VForge.Gameplay
{
    /// <summary>
    /// Visual representation of a single piece inventory item.
    /// </summary>
    public sealed class PieceInventoryItemView : InventoryItemView<PieceDefinition>
    {
        [SerializeField] private Image _background;




        // --

        protected override void OnBind()
        {
            // Assign icon, color, etc.
            _background.color = TypedItem.Data.Style.Color;
        }

        protected override void OnUnbind()
        {
            // Clear visuals
        }
    }
}
