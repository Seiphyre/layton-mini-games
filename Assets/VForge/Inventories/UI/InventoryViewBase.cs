using System;

namespace VForge.Inventories.UI
{
    /// <summary>
    /// Base class for all inventory views.
    /// Runtime-only. Editor preview is driven externally.
    /// </summary>
    public abstract class InventoryViewBase : UIElement
    {
        /// <summary>
        /// Raised when a drag starts from an inventory item.
        /// </summary>
        public event Action<object> ItemDragStarted;

        /// <summary>
        /// Raised when a drag ends from an inventory item.
        /// </summary>
        public event Action<object> ItemDragEnded;

        /// <summary>
        /// Bind this view to a runtime inventory.
        /// </summary>
        public abstract void Bind(object inventory);

        /// <summary>
        /// Unbind the currently bound inventory, if any.
        /// </summary>
        public abstract void Unbind();

        /// <summary>
        /// Clear all item visuals.
        /// </summary>
        public abstract void Clear();

        // --------------------------------------------------
        // Protected helpers for derived classes
        // --------------------------------------------------

        protected void RaiseItemDragStarted(object item) => ItemDragStarted?.Invoke(item);

        protected void RaiseItemDragEnded(object item) => ItemDragEnded?.Invoke(item);
    }
}
