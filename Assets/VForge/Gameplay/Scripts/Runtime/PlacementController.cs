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
    public sealed class PlacementController : IPlacementContext
    {

        private readonly PieceBoard board;

        public PlacementInfo CurrentPlacement { get; private set; } = PlacementInfo.None();


        public bool IsPlacing => CurrentPlacement.Kind != PlacementType.None;


        // ----------------------------
        // Constructor
        // ----------------------------

        public PlacementController(PieceBoard board)
        {
            this.board = board;
        }



        // ----------------------------
        // Placement lifecycle
        // ----------------------------

        public void BeginCreatePlacement(PieceDefinition defintion)
        {
            CurrentPlacement = PlacementInfo.Create(defintion);
        }

        public void BeginMovePlacement(Piece piece)
        {
            CurrentPlacement = PlacementInfo.Move(piece);
        }

        public void EndPlacement()
        {
            CurrentPlacement = PlacementInfo.None();
        }

        public PlacementOperationResult ConfirmPlacement(Vector2Int cellPosition)
        {
            if (!IsPlacing)
                return PlacementOperationResult.Fail("No active placement.");

            PlacementOperationResult placementResult;

            switch (CurrentPlacement.Kind)
            {
                case PlacementType.Create:
                    var placeResult = board.TryPlace(CurrentPlacement.Definition, cellPosition, locked: false, out _);
                    placementResult = PlacementOperationResult.FromBoard(placeResult);
                    break;

                case PlacementType.Move:
                    var moveResult = board.TryMove(CurrentPlacement.Piece, cellPosition);
                    placementResult = PlacementOperationResult.FromBoard(moveResult);
                    break;

                default:
                    throw new InvalidOperationException("Invalid Placement Kind. Enable to confirm placement.");
            }

            CurrentPlacement = PlacementInfo.None();

            return placementResult;
        }



        // ----------------------------
        // IPlacementContext API
        // ----------------------------

        public PlacementOperationResult CanConfirmPlacement(Vector2Int cellPosition)
        {
            return ValidatePlacementAt(cellPosition);
        }

        public PlacementOperationResult ValidatePlacementAt(Vector2Int cellPosition)
        {
            if (!IsPlacing)
                return PlacementOperationResult.Fail("No active placement.");

            switch (CurrentPlacement.Kind)
            {
                case PlacementType.Create:
                    var placeResult = board.CanPlace(CurrentPlacement.Definition, cellPosition);
                    return PlacementOperationResult.FromBoard(placeResult);

                case PlacementType.Move:
                    var moveResult = board.CanPlace(CurrentPlacement.Piece.Definition, cellPosition);
                    return PlacementOperationResult.FromBoard(moveResult);

                default:
                    return PlacementOperationResult.Fail("Invalid placement intent.");
            }
        }
    }
}
