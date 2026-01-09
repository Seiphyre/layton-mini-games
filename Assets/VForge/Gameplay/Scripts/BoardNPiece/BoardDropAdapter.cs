using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.BoardPieces.Views;
using VForge.Boards.Views;
using VForge.Inventories;

namespace VForge.Gameplay
{
    public sealed class BoardDropAdapter
    {
        private readonly DragController dragSystem;
        private readonly PieceBoardView pieceBoardView;
        private readonly BoardView boardView;
        private readonly IPlacementContext placement;

        private readonly DropTarget dropTarget;
        private readonly BoardDropPayloadResolver payloadResolver = new BoardDropPayloadResolver();

        private Vector2Int? dragHoveredCell;
        private bool isDragOnBoard;

        public event Action<BoardDropPayloadInfo, Vector2Int> DragDropped;

        private bool IsDragOnBoard
        {
            get { return isDragOnBoard; }
            set
            {
                if (isDragOnBoard == value)
                    return;

                isDragOnBoard = value;

                if (isDragOnBoard == true)
                {
                    OnDragEnterBoard();
                }

                if (isDragOnBoard == false)
                {
                    OnDragExitBoard();
                }
            }
        }

        private Vector2Int? DragHoveredCell
        {
            get { return dragHoveredCell; }
            set
            {
                if (dragHoveredCell == value)
                    return;

                dragHoveredCell = value;

                if (dragHoveredCell.HasValue)
                {
                    OnDragHoverCell(dragHoveredCell.Value);
                }
            }
        }



        // -----------------------------------------
        // Constructor
        // -----------------------------------------

        public BoardDropAdapter(
            IPlacementContext placement,
            DragController dragSystem,
            PieceBoardView pieceBoardView,
            BoardView boardView)
        {
            this.placement = placement ?? throw new ArgumentNullException(nameof(placement));
            this.dragSystem = dragSystem ?? throw new ArgumentNullException(nameof(dragSystem));
            this.pieceBoardView = pieceBoardView ?? throw new ArgumentNullException(nameof(pieceBoardView));
            this.boardView = boardView ?? throw new ArgumentNullException(nameof(boardView));


            // --

            this.dropTarget = boardView.GetComponent<DropTarget>();

            if (dropTarget == null)
                return;

            dropTarget.ClearRules();
            dropTarget.AddRule(payload => payloadResolver.TryResolve(payload, out _));

            dragSystem.DragStarted += OnDragStarted;
            dragSystem.DragUpdated += OnDragUpdated;
            dragSystem.DragDropped += OnDragDropped;
            dragSystem.DragEnded += OnDragEnded;
        }

        // -----------------------------------------
        // Drag Lifecycle
        // -----------------------------------------

        private void OnDragStarted(DragSession session)
        {
            CreatePreview();

            OnDragUpdated(session);
        }

        private void OnDragUpdated(DragSession session)
        {
            if (!IsRelevantDropTarget(session.HoverTarget))
                return;

            IsDragOnBoard = boardView.TryScreenPositionToCellPosition(session.ScreenPosition, out var cellPosition);

            if (!IsRelevantPayload(session.Payload))
                return;

            DragHoveredCell = IsDragOnBoard ? cellPosition : null;
        }

        private void OnDragDropped(DragSession session)
        {
            if (!IsRelevantDropTarget(session.HoverTarget))
                return;

            if (!DragHoveredCell.HasValue)
                return;

            if (!payloadResolver.TryResolve(session.Payload, out BoardDropPayloadInfo boardPayload))
                return;

            DragDropped?.Invoke(boardPayload, DragHoveredCell.Value);
        }

        private void OnDragEnded(DragSession _)
        {
            ClearDragState();
            DestroyPreview();
        }

        // --

        private void OnDragExitBoard()
        {
            HidePreviewAndShowProxy();
        }

        private void OnDragEnterBoard()
        {
            ShowPreviewAndHideProxy();
        }

        private void OnDragHoverCell(Vector2Int cellPosition)
        {
            MovePreviewAndSetValidity(cellPosition);
        }



        // -----------------------------------------
        // Preview Lifecycle
        // -----------------------------------------

        private void CreatePreview()
        {
            var activeDefinition = placement.CurrentPlacement.Kind switch
            {
                PlacementType.Create => placement.CurrentPlacement.Definition,
                PlacementType.Move => placement.CurrentPlacement.Piece.Definition,
                PlacementType.None => null,

                _ => throw new InvalidOperationException("Invalid placement kind. Enable to create preview.")
            };

            // Create Preview
            if (activeDefinition != null)
            {
                pieceBoardView.CreatePreview(activeDefinition);
                pieceBoardView.HidePreview();
            }
        }

        private void MovePreviewAndSetValidity(Vector2Int cellPosition)
        {
            // Move Preview
            pieceBoardView.SetPreviewPosition(cellPosition);

            // Set Preview Validity
            pieceBoardView.SetPreviewValidity(placement.ValidatePlacementAt(cellPosition).Success);
        }

        private void DestroyPreview()
        {
            // Destroy Preview
            pieceBoardView.DestroyPreview();
        }

        private void ShowPreviewAndHideProxy()
        {
            // Hide Proxy
            if (dragSystem.Current != null && dragSystem.Current.HasProxy)
                dragSystem.Current.Proxy.Hide();

            // Show Preview
            pieceBoardView.ShowPreview();
        }

        private void HidePreviewAndShowProxy()
        {
            // Show Proxy
            if (dragSystem.Current != null && dragSystem.Current.HasProxy)
                dragSystem.Current.Proxy.Show();

            // Hide Preview
            pieceBoardView.HidePreview();
        }



        // -----------------------------------------
        // Internal Helpers
        // -----------------------------------------

        private bool IsRelevantPayload(object payload)
        {
            return dropTarget.CanAccept(payload);
        }

        private bool IsRelevantDropTarget(DropTarget dropTarget)
        {
            return this.dropTarget == dropTarget;
        }

        private void ClearDragState()
        {
            IsDragOnBoard = false;
            DragHoveredCell = null;
        }
    }

}