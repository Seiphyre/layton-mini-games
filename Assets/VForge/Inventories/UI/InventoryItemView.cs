using System;

namespace VForge.Inventories.UI
{
    /// <summary>
    /// Generic inventory item view logic.
    /// </summary>
    public abstract class InventoryItemView<T> : InventoryItemViewBase
    {
        public InventoryItem<T> TypedItem { get; private set; }



        // --------------------------------------------------
        // Data Binding API
        // --------------------------------------------------

        public override void Bind(object item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (item is not InventoryItem<T> typedItem)
                throw new ArgumentException($"Invalid item type. Expected InventoryItem<{typeof(T).Name}>.");

            Item = item;
            TypedItem = typedItem;

            OnBind();
        }

        public override void Unbind()
        {
            if (TypedItem == null)
                return;

            OnUnbind();

            TypedItem = null;
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
