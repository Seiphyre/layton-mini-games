using VForge.Inventories;
using VForge.BoardPieces.Definitions;

namespace VForge.Gameplay
{
    /// <summary>
    /// Handles inventory side-effects of gameplay actions
    /// (reserve / commit / cancel).
    /// </summary>
    public sealed class InventoryUsageController
    {
        private readonly Inventory<PieceDefinition> inventory;


        private InventoryItem<PieceDefinition> reservedItem;
        public bool HasReservedItem => reservedItem != null;



        // ----------------------------
        // Constructor
        // ----------------------------

        public InventoryUsageController(Inventory<PieceDefinition> inventory)
        {
            this.inventory = inventory;
        }



        // ----------------------------
        // public API
        // ----------------------------

        public bool BeginUsage(InventoryItem<PieceDefinition> item)
        {
            if (item == null)
                return false;

            if (reservedItem != null)
                return false;

            reservedItem = item;
            return true;
        }

        public void ConfirmUsage()
        {
            if (reservedItem == null)
                return;

            inventory.Remove(reservedItem);
            reservedItem = null;
        }

        public void EndUsage()
        {
            reservedItem = null;
        }

        public InventoryOperationResult CanConfirmUsage()
        {
            if (reservedItem == null)
                return InventoryOperationResult.Fail("No active item.");

            return inventory.CanRemove(reservedItem);
        }
    }
}
