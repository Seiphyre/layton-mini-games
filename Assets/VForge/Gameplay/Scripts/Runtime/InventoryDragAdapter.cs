using System;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.Inventories.UI;
using VForge.Inventories;

namespace VForge.Gameplay
{
    public sealed class InventoryDragAdapter
    {
        public event Action<InventoryItem<PieceDefinition>> DragStarted;
        public event Action DragEnded;

        private readonly DragController dragSystem;
        private readonly PieceInventoryView inventoryView;
        private readonly InventoryDragOptions options;

        private bool isDragging = false;

        public InventoryDragAdapter(DragController dragSystem, PieceInventoryView inventoryView, InventoryDragOptions options = null)
        {
            this.dragSystem = dragSystem ?? throw new ArgumentNullException(nameof(dragSystem)); ;
            this.inventoryView = inventoryView ?? throw new ArgumentNullException(nameof(inventoryView)); ;
            this.options = options ?? throw new ArgumentNullException(nameof(options)); ;

             // --

            inventoryView.OnItemViewCreated += OnItemViewCreated;
            inventoryView.OnItemViewDestroyed += OnItemViewDestroyed;

            foreach ( var itemView in inventoryView.ItemViews)
            {
                OnItemViewCreated(itemView);
            }

            // --

            dragSystem.DragStarted += OnDragStarted;
            dragSystem.DragEnded += OnDragEnded;
        }

        private void OnItemViewCreated(InventoryItemView<PieceDefinition> view)
        {
            var dragSource = ComponentUtils.GetOrAddComponent<DragSource>(view.gameObject);

            if (dragSource != null)
            {
                dragSource.Initialize(dragSystem);
                dragSource.Payload = view.TypedItem;
                dragSource.CreateProxy = options.CreateProxy;
                dragSource.ProxyFactory = options.ProxyFactory;
            }
        }

        private void OnItemViewDestroyed(InventoryItemView<PieceDefinition> view)
        {
            var ds = view.GetComponent<DragSource>();

            if (ds != null)
                ds.Payload = null;
        }

        private void OnDragStarted(DragSession session)
        {
            if (session.Payload != null && session.Payload is InventoryItem<PieceDefinition> inventoryItem)
            {
                isDragging = true;
                DragStarted?.Invoke(inventoryItem);
            }
        }

        private void OnDragEnded(DragSession session)
        {
            if (!isDragging)
                return;

            DragEnded?.Invoke();

            isDragging = false;
        }

    }

}
