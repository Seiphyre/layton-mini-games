using System;

namespace VForge.Inventories.UI
{
    /// <summary>
    /// Generic inventory item view logic.
    /// </summary>
    public abstract class InventoryItemView<T> : InventoryItemViewBase
    {
        public InventoryItem<T> Item { get; private set; }


        public override void Bind(object item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (item is not InventoryItem<T> typedItem)
                throw new ArgumentException($"Invalid item type. Expected InventoryItem<{typeof(T).Name}>.");

            Item = typedItem;
            OnBind();
        }

        public override void Unbind()
        {
            if (Item == null)
                return;

            OnUnbind();
            Item = null;
        }



        // --------------------------------------------------
        // Hooks for concrete implementations
        // --------------------------------------------------

        /// <summary>
        /// Called after BoundItem has been assigned.
        /// Update visuals here.
        /// </summary>
        protected abstract void OnBind();

        /// <summary>
        /// Called before BoundItem is cleared.
        /// Cleanup visuals here.
        /// </summary>
        protected abstract void OnUnbind();
    }
}
