using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.Inventories;

namespace VForge.Gameplay.UI
{
    /// <summary>
    /// Connects inventory UI items to Draggable.
    /// No gameplay logic.
    /// </summary>
    public sealed class InventoryItemDragAdapter : MonoBehaviour
    {
        private Inventory<PieceDefinition> _inventory;

        public void Initialize(Inventory<PieceDefinition> inventory)
        {
            _inventory = inventory;
        }

        /// <summary>
        /// Called by the inventory item UI (e.g. via ListPresenter).
        /// </summary>
        public void AttachPayload(
            Draggable draggable,
            InventoryItem<PieceDefinition> item)
        {
            if (draggable == null || item == null)
                return;

            draggable.Payload = item;
        }
    }
}
