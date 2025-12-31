using System;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.Inventories.UI;
using VForge.Inventories;
using VForge.BoardPieces.Views;
using VForge.BoardPieces.Runtime;

namespace VForge.Gameplay
{
    public sealed class PieceDragAdapter
    {
        public event Action<Piece> DragStarted;
        public event Action DragEnded;
        public event Action<DragCancelReason> DragCancelled;

        private readonly DragController dragSystem;
        private readonly PieceBoardView pieceBoardView;
        private readonly PieceDragOptions options;

        private bool isDragging = false;

        public PieceDragAdapter(DragController dragSystem, PieceBoardView pieceBoardView, PieceDragOptions options = null)
        {
            this.dragSystem = dragSystem ?? throw new ArgumentNullException(nameof(dragSystem)); ;
            this.pieceBoardView = pieceBoardView ?? throw new ArgumentNullException(nameof(pieceBoardView)); ;
            this.options = options ?? throw new ArgumentNullException(nameof(options)); ;

            // --

            pieceBoardView.OnPieceViewCreated += OnPieceViewCreated;
            pieceBoardView.OnPieceViewDestroyed += OnPieceViewDestroyed;

            foreach (var itemView in pieceBoardView.PieceViews)
            {
                OnPieceViewCreated(itemView);
            }

            // --

            dragSystem.DragStarted += OnDragStarted;
            dragSystem.DragEnded += OnDragEnded;
            dragSystem.DragCancelled += OnDragCancelled;
        }

        private void OnPieceViewCreated(PieceView view)
        {
            if (view.Piece.IsLocked)
                return;

            var dragSource = ComponentUtils.GetOrAddComponent<DragSource>(view.gameObject);

            if (dragSource != null)
            {
                dragSource.Initialize(dragSystem);
                dragSource.Payload = view.Piece;
                dragSource.CreateProxy = options.CreateProxy;
                dragSource.ProxyFactory = options.ProxyFactory;
            }
        }

        private void OnPieceViewDestroyed(PieceView view)
        {
            var ds = view.GetComponent<DragSource>();

            if (ds != null)
                ds.Payload = null;
        }

        private void OnDragStarted(DragSession session)
        {
            if (session.Payload != null && session.Payload is Piece piece)
            {
                isDragging = true;
                DragStarted?.Invoke(piece);
            }
        }

        private void OnDragEnded(DragSession session)
        {
            if (!isDragging)
                return;

            DragEnded?.Invoke();

            isDragging = false;
        }

        private void OnDragCancelled(DragSession session, DragCancelReason reason)
        {
            if (!isDragging)
                return;

            DragCancelled?.Invoke(reason);
        }
    }

}
