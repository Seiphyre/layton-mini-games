namespace VForge.Inventories
{
    /// <summary>
    /// Default permissive policy.
    /// </summary>
    public sealed class AllowAllPolicy<T> : IInventoryPolicy<T>
    {
        public bool CanAdd(Inventory<T> inventory, InventoryItem<T> item, out string reason)
        {
            reason = null;
            return true;
        }

        public bool CanRemove(Inventory<T> inventory, InventoryItem<T> item, out string reason)
        {
            reason = null;
            return true;
        }

        public bool CanUse(Inventory<T> inventory, InventoryItem<T> item, out string reason)
        {
            reason = null;
            return true;
        }
    }
}