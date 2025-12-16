using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VForge.Inventories
{
    /// <summary>
    /// Generic runtime inventory.
    /// Owns items and enforces rules via policies.
    /// </summary>
    public sealed class Inventory<T>
    {
        private readonly ObservableCollection<InventoryItem<T>> _items = new();
        private readonly ReadOnlyObservableCollection<InventoryItem<T>> _readOnlyItems;
        private readonly List<IInventoryPolicy<T>> _policies = new();

        public ReadOnlyObservableCollection<InventoryItem<T>> Items => _readOnlyItems;
        public IReadOnlyList<IInventoryPolicy<T>> Policies => _policies;

        public Inventory(params IInventoryPolicy<T>[] policies)
        {
            _readOnlyItems = new ReadOnlyObservableCollection<InventoryItem<T>>(_items);

            if (policies != null && policies.Length > 0)
                _policies.AddRange(policies);
            else
                _policies.Add(new AllowAllPolicy<T>());
        }

        // ============================
        // Add
        // ============================

        public InventoryOperationResult CanAdd(InventoryItem<T> item)
            => Evaluate(
                p => p.CanAdd(this, item, out var r),
                out var reason)
                ? InventoryOperationResult.Ok()
                : InventoryOperationResult.Fail(reason);

        public InventoryOperationResult Add(InventoryItem<T> item)
        {
            var res = CanAdd(item);
            if (!res.Success)
                return res;

            _items.Add(item);
            return InventoryOperationResult.Ok();
        }

        // ============================
        // Remove
        // ============================

        public InventoryOperationResult CanRemove(InventoryItem<T> item)
            => Evaluate(
                p => p.CanRemove(this, item, out var r),
                out var reason)
                ? InventoryOperationResult.Ok()
                : InventoryOperationResult.Fail(reason);

        public InventoryOperationResult Remove(InventoryItem<T> item)
        {
            if (item == null)
                return InventoryOperationResult.Fail("Item is null.");

            var res = CanRemove(item);
            if (!res.Success)
                return res;

            if (!_items.Remove(item))
                return InventoryOperationResult.Fail("Item not found in inventory.");

            return InventoryOperationResult.Ok();
        }

        // ============================
        // Use (validation only for now)
        // ============================

        public InventoryOperationResult CanUse(InventoryItem<T> item)
            => Evaluate(
                p => p.CanUse(this, item, out var r),
                out var reason)
                ? InventoryOperationResult.Ok()
                : InventoryOperationResult.Fail(reason);

        public InventoryOperationResult Use(InventoryItem<T> item)
        {
            // Phase 5: validation only
            return CanUse(item);
        }

        // ============================
        // Policy evaluation helper
        // ============================

        private bool Evaluate(
            System.Func<IInventoryPolicy<T>, bool> check,
            out string reason)
        {
            foreach (var policy in _policies)
            {
                if (!check(policy))
                {
                    reason = "Inventory policy rejected operation.";
                    return false;
                }
            }

            reason = null;
            return true;
        }
    }
}