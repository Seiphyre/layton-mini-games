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
        private VictoryValidator _victoryValidator;
        private ILevelResetService _levelController;

        private Board _board;
        private PieceBoard _pieceBoard;
        private Inventory<PieceDefinition> _inventory;

        public bool _gameStarted = false;


        public string LevelTitle { get; }
        public InventoryState InventoryState => new InventoryState(_inventory);
        public BoardState BoardState => new BoardState(_pieceBoard);
        public GameState GameState => new GameState(_pieceBoard, _inventory, _gameStarted);



        public event Action GameStarted;
        public event Action GameEnded;

        public event Action<VictoryValidationResult> BoardValidated;

        public event Action<InventoryState> InventoryStateChanged;
        public event Action<GameState> GameStateChanged;



        // -----------------------------------------------------
        // Constructor
        // -----------------------------------------------------

        public GameplayController(
            Board board,
            PieceBoard pieceBoard,
            Inventory<PieceDefinition> inventory,
            BoardPlacementController boardPlacementController,
            InventoryUsageController inventoryUsageController,
            VictoryValidator victoryValidator,
            BoardDropAdapter boardDropAdapter,
            PieceDragAdapter pieceDragAdapter,
            InventoryDragAdapter inventoryDragAdapter,
            ILevelResetService levelController)
        {
            _boardDropAdapter = boardDropAdapter;
            _pieceDragAdapter = pieceDragAdapter;
            _inventoryDragAdapter = inventoryDragAdapter;

            _placementController = boardPlacementController;
            _inventoryUsageController = inventoryUsageController;
            _victoryValidator = victoryValidator;
            _levelController = levelController;

            _board = board;
            _pieceBoard = pieceBoard;
            _inventory = inventory;

            LevelTitle = "The Fruit Shop~";


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

            // --

            _inventory.ItemsChanged += (sender, args) =>
            {
                InventoryStateChanged.Invoke(new InventoryState(_inventory));
            };

            // --

            StartGame();
        }

        public void Dispose()
        {
            
        }



        // -----------------------------------------------------
        // Public API
        // -----------------------------------------------------

        public void StartGame()
        {
            _gameStarted = true;
            GameStarted?.Invoke();
            GameStateChanged?.Invoke(GameState);
        }

        public void EndGame()
        {
            _gameStarted = false;
            GameEnded?.Invoke();
            GameStateChanged?.Invoke(GameState);
        }

        public void ResetGame()
        {
            EndGame();
            _levelController.ResetLevel();
        }

        public void ValidateBoard()
        {
            var result = _victoryValidator.Validate(GameState);
            BoardValidated?.Invoke(result);
        }
    }
}
