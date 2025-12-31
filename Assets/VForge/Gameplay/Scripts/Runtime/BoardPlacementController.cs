using System;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.BoardPieces.Views;
using VForge.Boards.Views;
using VForge.Inventories;
using VForge.Inventories.UI;

namespace VForge.Gameplay
{
    /// <summary>
    /// Gameplay controller responsible for placement preview (ghost)
    /// while dragging a piece from the inventory.
    ///
    /// Phase 3.4 scope:
    /// - Show / move / hide preview
    /// - Validate placement
    /// - NO placement commit
    /// - NO inventory mutation
    /// - NO DropZone usage
    /// </summary>
    public sealed class BoardPlacementController : IBoardPlacementContext
    {

        private readonly PieceBoard board;

        public PieceDefinition ActiveDefinition { get; private set; } = null;
        public Piece ActivePiece { get; private set; } = null;
        public BoardPlacementIntent Intent { get; private set; } = BoardPlacementIntent.None;


        public bool IsPlacing => Intent != BoardPlacementIntent.None;


        // ----------------------------
        // Constructor
        // ----------------------------

        public BoardPlacementController(PieceBoard board)
        {
            this.board = board;
        }



        // ----------------------------
        // Drag lifecycle
        // ----------------------------

        public bool BeginPlacement(PieceDefinition defintion)
        {
            return BeginPlacementInternal(defintion);
        }

        public bool BeginPlacement(Piece piece)
        {
            return BeginPlacementInternal(piece);
        }

        private bool BeginPlacementInternal(object payload)
        {
            ClearState();

            switch (payload)
            {
                case PieceDefinition def:
                    Intent = BoardPlacementIntent.PlaceNew;
                    ActiveDefinition = def;
                    return true;

                case Piece piece:
                    Intent = BoardPlacementIntent.MoveExisting;
                    ActivePiece = piece;
                    return true;

                default:
                    return false;
            }
        }

        public void CancelPlacement()
        {
            ClearState();
        }

        public PieceBoardOperationResult Place(Vector2Int cellPosition)
        {
            if (!IsPlacing)
                return PieceBoardOperationResult.Fail("No active placement.");

            PieceBoardOperationResult result;

            switch (Intent)
            {
                case BoardPlacementIntent.PlaceNew:
                    result = board.TryPlace(ActiveDefinition, cellPosition, locked: false, out _);
                    break;

                case BoardPlacementIntent.MoveExisting:
                    result = board.TryMove(ActivePiece, cellPosition);
                    break;

                default:
                    result = PieceBoardOperationResult.Fail("Invalid placement intent.");
                    break;
            }

            ClearState();
            return result;
        }

        public PieceBoardOperationResult CanPlace(Vector2Int cellPosition)
        {
            if (!IsPlacing)
                return PieceBoardOperationResult.Fail("No active placement.");

            PieceBoardOperationResult result;

            switch (Intent)
            {
                case BoardPlacementIntent.PlaceNew:
                    result = board.CanPlace(ActiveDefinition, cellPosition);
                    break;

                case BoardPlacementIntent.MoveExisting:
                    result = board.CanPlace(ActivePiece.Definition, cellPosition);
                    break;

                default:
                    result = PieceBoardOperationResult.Fail("Invalid placement intent.");
                    break;
            }

            return result;
        }

        private void ClearState()
        {
            Intent = BoardPlacementIntent.None;
            ActiveDefinition = null;
            ActivePiece = null;
        }
    }
}
