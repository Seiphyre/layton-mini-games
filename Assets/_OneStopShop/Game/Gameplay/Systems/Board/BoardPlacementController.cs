using System;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.BoardPieces.Views;
using VForge.Boards.Views;
using VForge.Inventories;
using VForge.Inventories.UI;

namespace OneStopShop
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
    public sealed class BoardPlacementController : IBoardPlacementContext, IDisposable
    {

        private readonly PieceBoard board;

        public BoardPlacementInfo CurrentPlacement { get; private set; } = BoardPlacementInfo.None();

        private Vector2Int? _placementPosition;

        public bool IsPlacing => CurrentPlacement.Kind != BoardPlacementType.None;


        // ----------------------------
        // Constructor
        // ----------------------------

        public BoardPlacementController(PieceBoard board)
        {
            this.board = board;
        }



        // ----------------------------
        // Placement lifecycle
        // ----------------------------

        public void BeginCreatePlacement(PieceDefinition defintion)
        {
            CurrentPlacement = BoardPlacementInfo.Create(defintion);
        }

        public void BeginMovePlacement(Piece piece)
        {
            CurrentPlacement = BoardPlacementInfo.Move(piece);
        }

        public void BeginRemovePlacement(Piece piece)
        {
            CurrentPlacement = BoardPlacementInfo.Remove(piece);
        }

        public void EndPlacement()
        {
            CurrentPlacement = BoardPlacementInfo.None();
        }

        public void SetPlacementPosition(Vector2Int position)
        {
            if (!IsPlacing)
                return;

            _placementPosition = position;
        }

        public BoardPlacementOperationResult ConfirmPlacement()
        {
            if (!IsPlacing)
                return BoardPlacementOperationResult.Fail("No active placement.");

            BoardPlacementOperationResult placementResult;

            switch (CurrentPlacement.Kind)
            {
                case BoardPlacementType.Create:
                    if (_placementPosition == null)
                        return BoardPlacementOperationResult.Fail("No target cell.");
                    var placeResult = board.TryPlace(CurrentPlacement.Definition, _placementPosition.Value, locked: false, out _);
                    placementResult = BoardPlacementOperationResult.FromBoard(placeResult);
                    break;

                case BoardPlacementType.Move:
                    if (_placementPosition == null)
                        return BoardPlacementOperationResult.Fail("No target cell.");
                    var moveResult = board.TryMove(CurrentPlacement.Piece, _placementPosition.Value);
                    placementResult = BoardPlacementOperationResult.FromBoard(moveResult);
                    break;


                case BoardPlacementType.Remove:
                    var removeResult = board.TryRemove(CurrentPlacement.Piece);
                    placementResult = BoardPlacementOperationResult.FromBoard(removeResult);
                    break;

                default:
                    throw new InvalidOperationException("Invalid Placement Kind. Enable to confirm placement.");
            }

            CurrentPlacement = BoardPlacementInfo.None();

            return placementResult;
        }



        // ----------------------------
        // IPlacementContext API
        // ----------------------------

        public BoardPlacementOperationResult CanConfirmPlacement(Vector2Int cellPosition)
        {
            return ValidatePlacementAt(cellPosition);
        }

        public BoardPlacementOperationResult ValidatePlacementAt(Vector2Int cellPosition)
        {
            if (!IsPlacing)
                return BoardPlacementOperationResult.Fail("No active placement.");

            switch (CurrentPlacement.Kind)
            {
                case BoardPlacementType.Create:
                    var placeResult = board.CanPlace(CurrentPlacement.Definition, cellPosition);
                    return BoardPlacementOperationResult.FromBoard(placeResult);

                case BoardPlacementType.Move:
                    var moveResult = board.CanMove(CurrentPlacement.Piece, cellPosition);
                    return BoardPlacementOperationResult.FromBoard(moveResult);

                case BoardPlacementType.Remove:
                    var removeResult = board.CanRemove(CurrentPlacement.Piece);
                    return BoardPlacementOperationResult.FromBoard(removeResult);

                default:
                    return BoardPlacementOperationResult.Fail("Invalid placement intent.");
            }
        }

        public void Dispose()
        {
            
        }
    }
}
