using System;
using System.Collections.Generic;

using UnityEngine;

using VForge.BoardPieces.Views;
using VForge.BoardPieces.Runtime;



namespace VForge.Gameplay
{
    public sealed class PieceDragAdapter : IDisposable
    {
        public event Action<Piece> DragStarted;
        public event Action DragEnded;
        public event Action<DragCancelReason> DragCancelled;

        // --

        private readonly DragController dragSystem;
        private readonly PieceBoardView pieceBoardView;
        private readonly PieceDragOptions options;

        // --

        private readonly HashSet<DragSource> _ownedSources = new();



        // -------------------------------------------------
        // Contructor & Destructor logic
        // -------------------------------------------------

        public PieceDragAdapter(
            DragController dragSystem, 
            PieceBoardView pieceBoardView, 
            PieceDragOptions options = null)
        {
            this.dragSystem = dragSystem ?? throw new ArgumentNullException(nameof(dragSystem)); ;
            this.pieceBoardView = pieceBoardView ?? throw new ArgumentNullException(nameof(pieceBoardView)); ;
            this.options = options ?? throw new ArgumentNullException(nameof(options)); ;
        }

        public void Initialize()
        {
            foreach (var itemView in pieceBoardView.PieceViews)
            {
                OnPieceViewCreated(itemView);
            }

            // --

            pieceBoardView.OnPieceViewCreated += OnPieceViewCreated;
            pieceBoardView.OnPieceViewDestroyed += OnPieceViewDestroyed;

            dragSystem.DragStarted += OnDragStarted;
            dragSystem.DragEnded += OnDragEnded;
            dragSystem.DragCancelled += OnDragCancelled;
        }

        public void Dispose()
        {
            foreach (var itemView in pieceBoardView.PieceViews)
            {
                OnPieceViewDestroyed(itemView);
            }

            // --

            pieceBoardView.OnPieceViewCreated -= OnPieceViewCreated;
            pieceBoardView.OnPieceViewDestroyed -= OnPieceViewDestroyed;

            dragSystem.DragStarted -= OnDragStarted;
            dragSystem.DragEnded -= OnDragEnded;
            dragSystem.DragCancelled -= OnDragCancelled;
        }



        // -------------------------------------------------
        // DragSource lifecycle
        // -------------------------------------------------

        private void OnPieceViewCreated(PieceView view)
        {
            // 1. Validation

            if (view.Piece.IsLocked)
                return; // The piece is locked, we should not be able to interact with it

            // 2. Create DragSource

            var dragSource = view.gameObject.AddComponent<DragSource>();

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
            // 1. Destroy DragSource

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
            // 1. Validation

            if (!_ownedSources.Contains(session.Source))
                return; // I don't own this drag source

            if (session.Payload == null || !(session.Payload is Piece piece))
                return; // Wrong payload

            // 2. Preview piece removal

            session.Source.gameObject.SetActive(false);

            // 3. Emit event

            DragStarted?.Invoke(piece);
        }

        private void OnDragEnded(DragSession session)
        {
            // 1. Validation

            if (!_ownedSources.Contains(session.Source))
                return; // I don't own this drag source

            // 2. Restore piece removal

            session.Source.gameObject.SetActive(true);

            // 3. Emit event

            DragEnded?.Invoke();
        }

        private void OnDragCancelled(DragSession session, DragCancelReason reason)
        {
            // 1. Validation

            if (!_ownedSources.Contains(session.Source))
                return; // I don't own this drag source

            // 2. Emit event

            DragCancelled?.Invoke(reason);
        }

    }

}
