using System;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.Inventories.UI;
using VForge.Inventories;
using VForge.BoardPieces.Views;
using VForge.BoardPieces.Runtime;
using System.Collections.Generic;

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

        private readonly HashSet<DragSource> _ownedSources = new();

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

                _ownedSources.Add(dragSource);
            }
        }

        private void OnPieceViewDestroyed(PieceView view)
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

            if (session.Payload == null || !(session.Payload is Piece piece))
                return;

            // --

            session.Source.gameObject.SetActive(false);

            // --

            DragStarted?.Invoke(piece);
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

        private void OnDragCancelled(DragSession session, DragCancelReason reason)
        {
            if (!_ownedSources.Contains(session.Source))
                return;

            DragCancelled?.Invoke(reason);
        }
    }

}
