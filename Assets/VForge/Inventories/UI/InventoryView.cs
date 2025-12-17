using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace VForge.Inventories.UI
{
    /// <summary>
    /// Generic inventory view logic.
    /// Not directly referenced by Unity.
    /// </summary>
    public abstract class InventoryView<T> : InventoryViewBase
    {
        public Inventory<T> Inventory { get; private set; }

        protected readonly List<InventoryItemViewBase> _activeViews = new();
        protected readonly Stack<InventoryItemViewBase> _pooledViews = new();



        // --------------------------------------------------
        // Data Binding API
        // --------------------------------------------------

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



        // --------------------------------------------------
        // Pooling
        // --------------------------------------------------

        protected InventoryItemViewBase AcquireItemView()
        {
            InventoryItemViewBase view;

            if (_pooledViews.Count > 0)
            {
                view = _pooledViews.Pop();
                view.gameObject.SetActive(true);
            }
            else
            {
                view = CreateItemView();
            }

            _activeViews.Add(view);
            return view;
        }

        protected void ReleaseItemView(InventoryItemViewBase view)
        {
            if (view == null)
                return;

            view.Unbind();
            view.gameObject.SetActive(false);

            _activeViews.Remove(view);
            _pooledViews.Push(view);
        }

        protected abstract InventoryItemViewBase CreateItemView();



        // --------------------------------------------------
        // View API
        // --------------------------------------------------

        protected void RebuildAllItems()
        {
            Clear();

            if (Inventory == null)
                return;

            foreach (var item in Inventory.Items)
            {
                var view = AcquireItemView();
                view.Bind(item);
            }
        }

        public override void Clear()
        {
            for (int i = _activeViews.Count - 1; i >= 0; i--)
            {
                ReleaseItemView(_activeViews[i]);
            }

            _activeViews.Clear();
        }

        protected void OnInventoryCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Simple & safe strategy for now:
            // full rebuild on any structural change
            RebuildAllItems();
        }
    }
}
