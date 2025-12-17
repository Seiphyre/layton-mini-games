using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.Inventories;

namespace VForge.Gameplay
{
    /// <summary>
    /// Pure gameplay controller responsible for placing pieces on the board.
    /// Knows nothing about UI, dragging, or views.
    /// </summary>
    public sealed class PiecePlacementController
    {
        private readonly PieceBoard _pieceBoard;
        private readonly Inventory<PieceDefinition> _inventory;

        private InventoryItem<PieceDefinition> _activeItem;

        public bool HasActivePlacement => _activeItem != null;

        public PiecePlacementController(
            PieceBoard pieceBoard,
            Inventory<PieceDefinition> inventory)
        {
            _pieceBoard = pieceBoard;
            _inventory = inventory;
        }

        // --------------------------------------------------
        // Placement lifecycle
        // --------------------------------------------------

        public InventoryOperationResult BeginPlacement(
            InventoryItem<PieceDefinition> item)
        {
            if (item == null)
                return InventoryOperationResult.Fail("Item is null.");

            if (HasActivePlacement)
                return InventoryOperationResult.Fail("Placement already active.");

            var canUse = _inventory.CanUse(item);
            if (!canUse.Success)
                return canUse;

            _activeItem = item;
            return InventoryOperationResult.Ok();
        }

        public InventoryOperationResult CanPlaceAt(Vector2Int cell)
        {
            if (_activeItem == null)
                return InventoryOperationResult.Fail("No active item.");

            var def = _activeItem.Data;

            var res = _pieceBoard.CanPlace(def, cell);
            return res.Success
                ? InventoryOperationResult.Ok()
                : InventoryOperationResult.Fail(res.Reason);
        }

        public InventoryOperationResult CommitPlacement(Vector2Int cell)
        {
            if (_activeItem == null)
                return InventoryOperationResult.Fail("No active item.");

            var def = _activeItem.Data;

            var placeResult = _pieceBoard.TryPlace(
                def,
                cell,
                locked: false,
                out var piece);

            if (!placeResult.Success)
                return InventoryOperationResult.Fail(placeResult.Reason);

            var removeResult = _inventory.Remove(_activeItem);
            if (!removeResult.Success)
            {
                // rollback
                _pieceBoard.TryRemove(piece);
                return removeResult;
            }

            _activeItem = null;
            return InventoryOperationResult.Ok();
        }

        public void CancelPlacement()
        {
            _activeItem = null;
        }
    }
}
