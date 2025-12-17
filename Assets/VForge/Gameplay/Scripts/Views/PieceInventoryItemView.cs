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
        protected override void OnBind()
        {
            // Assign icon, color, etc.
        }

        protected override void OnUnbind()
        {
            // Clear visuals
        }
    }
}
