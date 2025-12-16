namespace VForge.Inventories
{
    /// <summary>
    /// Defines rules for inventory operations.
    /// Policies are evaluated in order; first failure stops.
    /// </summary>
    public interface IInventoryPolicy<T>
    {
        bool CanAdd(Inventory<T> inventory, InventoryItem<T> item, out string reason);
        bool CanRemove(Inventory<T> inventory, InventoryItem<T> item, out string reason);
        bool CanUse(Inventory<T> inventory, InventoryItem<T> item, out string reason);
    }
}
