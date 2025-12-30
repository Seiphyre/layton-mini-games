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
        public abstract void ClearView();
    }
}
