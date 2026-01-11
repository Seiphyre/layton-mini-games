using UnityEngine;
using VForge.Boards.Runtime;
using VForge.Boards.Views;
using VForge.BoardPieces.Runtime;
using VForge.Gameplay;
using VForge.BoardPieces.Views;
using System.Linq;
using VForge.BoardPieces.Definitions;
using VForge.Inventories;
using VForge.Boards.Definitions;
using VForge.Inventories.UI;


namespace VForge.Gameplay
{
    public class GameStartup : MonoBehaviour, ILevelResetService
    {


        [Header("Settings"), Space]
        [SerializeField] private BoardDefinition BoardDefinition;
        [SerializeField] private InventoryDefinition InventoryDefinition;

        [Space]
        [SerializeField] private PieceBoardView PieceBoardViewPrefab;

        [Space]
        [SerializeField] private DragProxyFactory DragProxyFactory;

        [Header("References")]
        [SerializeField] private DragController DragController;

        [Space]
        [SerializeField] private GameHudView GameHudView;
        [SerializeField] private BoardView BoardView;
        [SerializeField] private PieceInventoryView InventoryView;

        // --

        private Board _board;
        private PieceBoard _pieceBoard;
        private Inventory<PieceDefinition> _inventory;

        private int _startPieceId = -1;

        // --

        private PieceBoardView _pieceBoardView;

        // --

        private BoardPlacementController _boardPlacementController;
        private InventoryUsageController _inventoryUsageController;

        // --

        private BoardDropAdapter _boardDropAdapter;
        private InventoryDragAdapter _inventoryDragAdapter;
        private PieceDragAdapter _pieceDragAdapter;

        // --

        private VictoryValidator _victoryValidator;
        private GameplayController _gameplayController;
        private GameHudPresenter _gameHudPresenter;


        // --------------------------------------------------------
        // Initialization
        // --------------------------------------------------------

        private void Start()
        {
            LoadLevel();

            // Todo: Refactoring of BoardDropAdapter to export placement logic in the gameplayController
            // Todo: Merging PieceDragAdapter and BoardDropAdapter into BoardDragNDropAdapter
            // Todo: Renaming InventoryDragAdapter to InventoryDragNDropAdapter
            // Todo: Merging Board and PieceBoard into GameBoard
            // Todo: Create alias GameInventory/GameInventoryItem, for Inventory<PieceDefinition>/InventoryItem<PieceDefinition>
            // Todo: Rename PieceInventoryView to GameInventoryView & PieceInventoryItemView to GameInventoryItemView
            // Todo: Remake InventoryDefinition/InventoryItemData to a proper LevelData
        }



        // --------------------------------------------------------
        // Public API
        // --------------------------------------------------------

        public void ResetLevel()
        {
            UnloadLevel();
            LoadLevel();
        }



        // --------------------------------------------------------
        // Internal Helpers
        // --------------------------------------------------------

        private void LoadLevel()
        {
            // ---------------------------------
            // 1. Create runtime data (level initialization)
            // ---------------------------------

            // 1.1 Build Board

            _board = new Board(BoardDefinition);

            // 1.2 Build Piece Board

            _pieceBoard = new PieceBoard(_board);

            // 1.4 Create Inventory

            _inventory = new Inventory<PieceDefinition>();

            // 1.5 Place pieces on board

            var boardPieces = InventoryDefinition.Pieces.Where(p => p.HasStartingPosition);
            foreach (var boardPiece in boardPieces)
            {
                var result = _pieceBoard.TryPlace(
                    boardPiece.Definition,
                    boardPiece.StartingPosition,
                    boardPiece.Locked,
                    out var piece);

                if (!result.Success)
                {
                    Debug.LogError($"Failed to place starting piece {piece.Id}: {result.Reason}");
                    continue;
                }

                if (_startPieceId == -1)
                    _startPieceId = piece.Id;
            }

            // 1.6 Place pieces in nventory

            var inventoryitems = InventoryDefinition.Pieces.Where(p => !p.HasStartingPosition);
            foreach (var inventoryItem in inventoryitems)
            {
                _inventory.Add(new InventoryItem<PieceDefinition>(null, inventoryItem.Definition));
            }



            // ---------------------------------
            // 2. Bind data to views (bind views)
            // ---------------------------------

            // 2.1 Initialize board view

            BoardView.BoardData = BoardDefinition;

            // 2.2 Create piece board view

            _pieceBoardView = Instantiate(PieceBoardViewPrefab);
            _pieceBoardView.name = "Pieces";

            BoardView.AttachToLayer(BoardViewLayer.Pieces, _pieceBoardView.RectTransform);

            _pieceBoardView.Initialize(_pieceBoard, BoardView);

            // 2.3 Initialize inventory view

            InventoryView.Bind(_inventory);



            // ---------------------------------
            // 3. Create system controllers, UI interactions and gameplay (Gameplay bootsrap)
            // ---------------------------------

            // 3.1.1 Create board placement controller

            _boardPlacementController = new BoardPlacementController(_pieceBoard);

            // 3.1.2 Create inventory usage controller

            _inventoryUsageController = new InventoryUsageController(_inventory);

            // ---------------------------------

            // 3.2.1 Create inventory view interactions

            _inventoryDragAdapter = new InventoryDragAdapter(DragController, InventoryView, new InventoryDragOptions()
            {
                CreateProxy = DragProxyFactory != null,
                ProxyFactory = DragProxyFactory
            });

            // 3.2.2 Create board view interactions

            _pieceDragAdapter = new PieceDragAdapter(DragController, _pieceBoardView, new PieceDragOptions()
            {
                CreateProxy = DragProxyFactory != null,
                ProxyFactory = DragProxyFactory
            });

            _boardDropAdapter = new BoardDropAdapter(_boardPlacementController, DragController, _pieceBoardView, BoardView);

            // ---------------------------------

            // 3.3.1 Create game rules

            _victoryValidator = new VictoryValidator(new IVictoryRule[]
            {
                new EmptyInventoryRule(),
                new SingleChainOnBoardRule(
                    _startPieceId,
                    new OrMatchRule(new IMatchRule[]
                    {
                        new ColorMatchRule(),
                        new TagMatchRule()
                    }))
            });

            // 3.3.2 Create gameplay

            _gameplayController = new GameplayController(
                _board,
                _pieceBoard,
                _inventory,
                _boardPlacementController, 
                _inventoryUsageController, 
                _victoryValidator,
                _boardDropAdapter, 
                _pieceDragAdapter, 
                _inventoryDragAdapter, 
                this);

            _gameHudPresenter = new GameHudPresenter(GameHudView, _gameplayController);
            _gameHudPresenter.Initialize();
        }

        private void UnloadLevel()
        {
            // 1. Dispose gameplay orchestrator
            _gameHudPresenter?.Dispose();
            _gameHudPresenter = null;

            _gameplayController?.Dispose();
            _gameplayController = null;

            _victoryValidator = null;

            // 2. Dispose gameplay controllers
            _boardPlacementController?.Dispose();
            _inventoryUsageController?.Dispose();

            // 3. Dispose UI adapters
            _boardDropAdapter?.Dispose();
            _pieceDragAdapter?.Dispose();
            _inventoryDragAdapter?.Dispose();

            // 4. Unbind views
            //_pieceBoardView.Unbind();
            //BoardView.Unbind();
            InventoryView.Unbind();

            // 5. Drop runtime data (GC will clean)
            _board = null;
            _pieceBoard = null;
            _inventory = null;

            _startPieceId = -1;
        }
    }
}
