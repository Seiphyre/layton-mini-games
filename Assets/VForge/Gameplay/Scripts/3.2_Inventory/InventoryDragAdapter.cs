using System;
using System.Collections.Generic;

using UnityEngine;

using VForge.BoardPieces.Definitions;
using VForge.Inventories.UI;
using VForge.Inventories;



namespace VForge.Gameplay
{
    public sealed class InventoryDragAdapter : IDisposable
    {
        public event Action<InventoryItem<PieceDefinition>> DragStarted;
        public event Action DragEnded;

        // --

        private readonly DragController dragSystem;
        private readonly PieceInventoryView inventoryView;
        private readonly InventoryDragOptions options;

        // --

        private readonly HashSet<DragSource> _ownedSources = new();



        // -------------------------------------------------
        // Contructor & Destructor logic
        // -------------------------------------------------

        public InventoryDragAdapter(
            DragController dragSystem, 
            PieceInventoryView inventoryView, 
            InventoryDragOptions options = null)
        {
            this.dragSystem = dragSystem ?? throw new ArgumentNullException(nameof(dragSystem)); ;
            this.inventoryView = inventoryView ?? throw new ArgumentNullException(nameof(inventoryView)); ;
            this.options = options ?? throw new ArgumentNullException(nameof(options)); ;

            // --

            foreach (var itemView in inventoryView.ItemViews)
            {
                OnItemViewCreated(itemView);
            }

            // --

            inventoryView.OnItemViewCreated += OnItemViewCreated;
            inventoryView.OnItemViewDestroyed += OnItemViewDestroyed;

            dragSystem.DragStarted += OnDragStarted;
            dragSystem.DragEnded += OnDragEnded;
        }

        public void Dispose()
        {
            foreach (var itemView in inventoryView.ItemViews)
            {
                OnItemViewDestroyed(itemView);
            }
        }



        // -------------------------------------------------
        // DragSource lifecycle
        // -------------------------------------------------

        private void OnItemViewCreated(InventoryItemView<PieceDefinition> inventoryItemView)
        {
            var dragSource = inventoryItemView.gameObject.AddComponent<DragSource>();

            if (dragSource != null)
            {
                dragSource.Initialize(dragSystem);
                dragSource.Payload = inventoryItemView.TypedItem;
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
                UnityEngine.Object.Destroy(dragSource);

                _ownedSources.Remove(dragSource);
            }
        }



        // -------------------------------------------------
        // Drag logic
        // -------------------------------------------------

        private void OnDragStarted(DragSession session)
        {
            // 1. Check drag validity

            if (!_ownedSources.Contains(session.Source))
                return; // I don't own this drag source

            if (session.Payload == null || !(session.Payload is InventoryItem<PieceDefinition> inventoryItem))
                return; // The payload is wrong

            // 2. Preview item removal

            session.Source.gameObject.SetActive(false);

            // 3. Emit event

            DragStarted?.Invoke(inventoryItem);
        }

        private void OnDragEnded(DragSession session)
        {
            // 1. Check drag validity

            if (!_ownedSources.Contains(session.Source))
                return; // I don't own this drag source

            // 2. Restore item removal

            session.Source.gameObject.SetActive(true);

            // 3. Emit event

            DragEnded?.Invoke();
        }
    }

}
