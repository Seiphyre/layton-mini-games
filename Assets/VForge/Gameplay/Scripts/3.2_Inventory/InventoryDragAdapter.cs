using System;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.Inventories.UI;
using VForge.Inventories;
using System.Collections.Generic;
using VForge.BoardPieces.Runtime;
using VForge.BoardPieces.Views;

namespace VForge.Gameplay
{
    public sealed class InventoryDragAdapter : IDisposable
    {
        public event Action<InventoryItem<PieceDefinition>> DragStarted;
        public event Action DragEnded;

        private readonly DragController dragSystem;
        private readonly PieceInventoryView inventoryView;
        private readonly InventoryDragOptions options;

        private readonly HashSet<DragSource> _ownedSources = new();

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

                _ownedSources.Add(dragSource);
            }
        }

        private void OnItemViewDestroyed(InventoryItemView<PieceDefinition> view)
        {
            var dragSource = view.GetComponent<DragSource>();

            if (dragSource != null)
            {
                dragSource.Payload = null;

                _ownedSources.Remove(dragSource);
            }
        }

        private void OnDragStarted(DragSession session)
        {
            if (!_ownedSources.Contains(session.Source))
                return;

            if (session.Payload == null || !(session.Payload is InventoryItem<PieceDefinition> inventoryItem))
                return;

            // --

            session.Source.gameObject.SetActive(false);

            // --

            DragStarted?.Invoke(inventoryItem);
        }

        private void OnDragEnded(DragSession session)
        {
            if (!_ownedSources.Contains(session.Source))
                return;

            // --

            session.Source.gameObject.SetActive(true);

            // --

            DragEnded?.Invoke();
        }

        public void Dispose()
        {
            
        }
    }

}
