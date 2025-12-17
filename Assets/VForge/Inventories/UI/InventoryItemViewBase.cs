using System;

namespace VForge.Inventories.UI
{
    /// <summary>
    /// Base class for all inventory item views.
    /// Purely visual.
    /// </summary>
    public abstract class InventoryItemViewBase : UIElement
    {
        /// <summary>
        /// Bind this view to a runtime inventory item.
        /// </summary>
        public abstract void Bind(object item);

        /// <summary>
        /// Unbind this view from its item.
        /// </summary>
        public abstract void Unbind();
    }
}
