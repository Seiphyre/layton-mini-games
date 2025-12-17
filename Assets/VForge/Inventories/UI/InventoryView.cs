using System;

namespace VForge.Inventories.UI
{
    /// <summary>
    /// Generic inventory view logic.
    /// Not directly referenced by Unity.
    /// </summary>
    public abstract class InventoryView<T> : InventoryViewBase
    {
        public Inventory<T> Inventory { get; private set; }

        public override void Bind(object inventory)
        {
            if (inventory == null)
                throw new ArgumentNullException(nameof(inventory));

            if (inventory is not Inventory<T> typedInventory)
                throw new ArgumentException($"Invalid inventory type. Expected Inventory<{typeof(T).Name}>.");

            Unbind();

            Inventory = typedInventory;
            OnBind();
        }

        public override void Unbind()
        {
            if (Inventory == null)
                return;

            OnUnbind();
            Inventory = null;
        }

        // --------------------------------------------------
        // Hooks for concrete implementations
        // --------------------------------------------------

        /// <summary>
        /// Called after Inventory has been assigned.
        /// Subscribe to events, build views, etc.
        /// </summary>
        protected abstract void OnBind();

        /// <summary>
        /// Called before Inventory is cleared.
        /// Unsubscribe, clear views, etc.
        /// </summary>
        protected abstract void OnUnbind();
    }
}
