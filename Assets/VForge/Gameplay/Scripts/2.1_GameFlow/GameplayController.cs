using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.Boards.Runtime;
using VForge.Inventories;

namespace VForge.Gameplay
{
    public class GameplayController : IDisposable
    {
        private BoardDropAdapter _boardDropAdapter;
        private InventoryDragAdapter _inventoryDragAdapter;
        private PieceDragAdapter _pieceDragAdapter;

        private BoardPlacementController _placementController;
        private InventoryUsageController _inventoryUsageController;


        // -----------------------------------------------------
        // Constructor
        // -----------------------------------------------------

        public GameplayController(
            BoardPlacementController boardPlacementController,
            InventoryUsageController inventoryUsageController,
            BoardDropAdapter boardDropAdapter,
            PieceDragAdapter pieceDragAdapter,
            InventoryDragAdapter inventoryDragAdapter)
        {
            _boardDropAdapter = boardDropAdapter;
            _pieceDragAdapter = pieceDragAdapter;
            _inventoryDragAdapter = inventoryDragAdapter;

            _placementController = boardPlacementController;
            _inventoryUsageController = inventoryUsageController;



            // --------------------------------------------------------------

            _inventoryDragAdapter.DragStarted += (inventoryItem) =>
            {
                _inventoryUsageController.BeginUsage(inventoryItem);
                _placementController.BeginCreatePlacement(inventoryItem.Data);
            };

            _inventoryDragAdapter.DragEnded += () =>
            {
                _inventoryUsageController.EndUsage();
                _placementController.EndPlacement();
            };



            // --------------------------------------------------------------

            _pieceDragAdapter.DragStarted += (piece) =>
            {
                _placementController.BeginMovePlacement(piece);
            };

            _pieceDragAdapter.DragEnded += () =>
            {
                _placementController.EndPlacement();
            };

            _pieceDragAdapter.DragCancelled += (reason) =>
            {
                if (reason != DragCancelReason.ReleasedNoTarget)
                    return;

                if (_placementController.CurrentPlacement.Kind == PlacementType.Move)
                {
                    var piece = _placementController.CurrentPlacement.Piece;

                    _placementController.BeginRemovePlacement(piece);
                    _placementController.ConfirmPlacement();
                    _inventoryUsageController.ReturnItem(new InventoryItem<PieceDefinition>(null, piece.Definition));
                }
            };



            // --------------------------------------------------------------

            _boardDropAdapter.DragDropped += (payload, cellPosition) =>
            {
                var placementOpResult = _placementController.ValidatePlacementAt(cellPosition);
                if (!placementOpResult.Success)
                    return;

                // Resolve inventory usage
                if (_placementController.CurrentPlacement.Kind == PlacementType.Create)
                {
                    var inventoryOpresult = _inventoryUsageController.CanConfirmUsage();
                    if (!inventoryOpresult.Success)
                        return;

                    _inventoryUsageController.ConfirmUsage();
                }

                _placementController.SetPlacementPosition(cellPosition);
                _placementController.ConfirmPlacement();
            };
        }

        public void Dispose()
        {
            
        }
    }
}
