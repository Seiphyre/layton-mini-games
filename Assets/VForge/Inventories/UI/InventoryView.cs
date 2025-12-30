using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

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

        public event Action<InventoryItemViewBase> OnItemViewCreated;
        public event Action<InventoryItemViewBase> OnItemViewDestroyed;

        public IReadOnlyList<InventoryItemViewBase> ItemViews => _activeViews;



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
            Inventory.ItemsChanged += OnInventoryCollectionChanged;

            OnBind();
        }

        public override void Unbind()
        {
            if (Inventory == null)
                return;

            OnUnbind();

            Inventory.ItemsChanged -= OnInventoryCollectionChanged;
            Inventory = null;
        }

        protected virtual void OnBind()
        {
            BuildView();
        }

        protected virtual void OnUnbind()
        {
            ClearView();
        }



        // --------------------------------------------------
        // Pooling
        // --------------------------------------------------

        protected InventoryItemViewBase AcquireItemView(InventoryItem<T> item)
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

            view.Bind(item);

            return view;
        }

        protected void ReleaseItemView(InventoryItemViewBase view)
        {
            if (view == null)
                return;

            view.Unbind();
            view.gameObject.SetActive(false);

            _pooledViews.Push(view);
        }

        protected abstract InventoryItemViewBase CreateItemView();




        // --------------------------------------------------
        // View API
        // --------------------------------------------------

        protected void BuildView()
        {
            ClearView();

            if (Inventory == null)
                return;

            foreach (var item in Inventory.Items)
            {
                var itemView = AcquireItemView(item);
                _activeViews.Add(itemView);

                OnItemViewCreated?.Invoke(itemView);
            }
        }

        public override void ClearView()
        {
            for (int i = _activeViews.Count - 1; i >= 0; i--)
            {
                OnItemViewDestroyed?.Invoke(_activeViews[i]);

                ReleaseItemView(_activeViews[i]);
                _activeViews.RemoveAt(i);
            }

            //_activeViews.Clear();
        }



        // --------------------------------------------------
        // Event Helpers
        // --------------------------------------------------

        protected virtual void OnInventoryCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BuildView();
        }
    }
}
