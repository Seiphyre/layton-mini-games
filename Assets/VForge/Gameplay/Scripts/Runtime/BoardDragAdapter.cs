using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Views;
using VForge.Boards.Views;
using VForge.Inventories.UI;

namespace VForge.Gameplay
{
    public sealed class BoardDragAdapter
    {
        private readonly DragController dragSystem;
        private readonly DropTarget dropTarget;
        private readonly PieceBoardView pieceBoardView;
        private readonly BoardView boardView;

        private Vector2Int? dragHoveredCell;
        private bool isDragOnBoard;

        public event Action<Vector2Int> HoverCell;
        public event Action<Vector2Int> DropOnCell;
        public event Action EnterBoard;
        public event Action ExitBoard;

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
                    EnterBoard?.Invoke();
                }

                if (isDragOnBoard == false)
                {
                    ExitBoard?.Invoke();
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
                    HoverCell?.Invoke(dragHoveredCell.Value);
                }
            }
        }



        // ----------------------------------------------

        public BoardDragAdapter(DragController dragSystem, PieceBoardView pieceBoardView, BoardView boardView)
        {
            this.dragSystem = dragSystem;
            this.pieceBoardView = pieceBoardView;
            this.boardView = boardView;

            this.dropTarget = boardView.GetComponent<DropTarget>();

            if (dropTarget == null)
                return;

            // --

            dropTarget.ClearRules();
            dropTarget.AddRule(payload => payload is PieceInventoryItemView);

            dragSystem.DragMoved += OnDragMoved;
            dragSystem.DragDropped += OnDragDropped;
        }



        // ----------------------------------------------

        private void OnDragMoved(DragSession session)
        {

            IsDragOnBoard = boardView.TryScreenPositionToCellPosition(session.ScreenPosition, out var cellPosition);

            if (!IsRelevantDropTarget(session.HoverTarget) || !IsRelevantPayload(session.Payload))
                return;

            DragHoveredCell = IsDragOnBoard ? cellPosition : null;
        }

        private void OnDragDropped(DragSession session)
        {
            if (!IsRelevantDropTarget(session.HoverTarget) || !IsRelevantPayload(session.Payload))
                return;

            if (DragHoveredCell.HasValue)
                DropOnCell?.Invoke(DragHoveredCell.Value);
        }

        bool IsRelevantPayload(object payload)
        {
            return dropTarget.CanAccept(payload);
        }

        bool IsRelevantDropTarget(DropTarget dropTarget)
        {
            return this.dropTarget == dropTarget;
        }
    }

}