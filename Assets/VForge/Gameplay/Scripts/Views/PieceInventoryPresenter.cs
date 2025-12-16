using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.Inventories;

namespace VForge.Gameplay
{
    public class PieceInventoryPresenter : ListPresenter<InventoryItem<PieceDefinition>>
    {
        protected override void OnListAssigned()
        {
            Debug.Log($"PopulateItems called on {name}");
            base.OnListAssigned();
        }
    }
}