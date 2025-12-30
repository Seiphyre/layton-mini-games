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
    public sealed class PiecePlacementController
    {

        private readonly PieceBoardView _pieceBoardView;
        private readonly InventoryView<PieceDefinition> _inventoryView;
        private readonly BoardView _boardView;

        private InventoryItem<PieceDefinition> activeItem;

        public bool HasActivePlacement => activeItem != null;
        public PieceDefinition ActiveDefinition => activeItem?.Data;



        // --------------------------------------------------
        // Constructor
        // --------------------------------------------------

        public PiecePlacementController(
            PieceBoardView pieceBoardView,
            BoardView boardView,
            InventoryView<PieceDefinition> inventoryView)
        {
            _inventoryView = inventoryView;
            _pieceBoardView = pieceBoardView;
            _boardView = boardView;

            // Inventory drag intent
            //_inventoryView.ItemDragStarted += OnItemDragStarted;
            //_inventoryView.ItemDragEnded += OnItemDragEnded;

            // Board hover intent
            //_boardView.HoverStarted += OnBoardHoverStarted;
            //_boardView.CellHovered += OnBoardCellHovered;
            //_boardView.HoverExited += OnBoardHoverExited;
            //_boardView.Dropped += OnBoardDrop;
        }

        // --------------------------------------------------
        // Inventory callbacks
        // --------------------------------------------------

        public void BeginPlacement(InventoryItem<PieceDefinition> item)
        {
            activeItem = item;
        }

        public void CancelPlacement()
        {
            activeItem = null;
        }

        public void TryPlace(Vector2Int cellPosition)
        {
            if (activeItem == null)
                return;

             // --

            var boardOpResult = _pieceBoardView.PieceBoard.CanPlace(activeItem.Data, cellPosition);

            if (!boardOpResult.Success)
                return;

            var inventoryOpResult = _inventoryView.Inventory.CanRemove(activeItem);

            if (!inventoryOpResult.Success)
                return;

            // --

            _pieceBoardView.PieceBoard.TryPlace(
                activeItem.Data,
                cellPosition,
                locked: false,
                out _);

            _inventoryView.Inventory.Remove(activeItem);

            // --

            activeItem = null;
        }
    }
}
