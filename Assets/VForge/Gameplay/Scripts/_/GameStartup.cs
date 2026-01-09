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
    public class GameStartup : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private DragController dragController;
        [SerializeField] private DragProxyFactory dragProxyFactory;

        [Header("Board")]
        [SerializeField] private BoardView boardView;
        [SerializeField] private PieceBoardView pieceBoardViewPrefab;
        [SerializeField] private BoardDefinition boardData;

        [Header("Inventory")]
        [SerializeField] private PieceInventoryView pieceInventoryView;
        [SerializeField] private InventoryDefinition piecesSetData;

        private PieceBoard pieceBoard;
        private Inventory<PieceDefinition> inventory;
        private VictoryValidator victoryValidator;



        private void Start()
        {
            // 1. Build runtime board and link it to view
            var board = new Board(boardData);

            boardView.BoardData = boardData;

            // 2. Build runtime pieceBoard and link it to view
            pieceBoard = new PieceBoard(board);

            var pieceBoardView = Instantiate(pieceBoardViewPrefab);
            pieceBoardView.name = "Pieces";
            pieceBoardView.Initialize(pieceBoard, boardView);

            boardView.AttachToLayer(BoardViewLayer.Pieces, pieceBoardView.RectTransform);

            // 3. Build runtime inventory / board and link it to view
            inventory = new Inventory<PieceDefinition>();
            int startPieceId = -1;

            foreach (var inventoryItem in piecesSetData.Pieces)
            {
                if (inventoryItem.HasStartingPosition)
                {
                    var result = pieceBoard.TryPlace(
                        inventoryItem.Definition,
                        inventoryItem.StartingPosition,
                        inventoryItem.Locked,
                        out var piece);

                    if (!result.Success)
                    {
                        Debug.LogError($"Failed to place starting piece {inventoryItem.Id}: {result.Reason}");
                        continue;
                    }

                    startPieceId = piece.Id;
                }
                else
                {
                    inventory.Add(new InventoryItem<PieceDefinition>(null, inventoryItem.Definition));
                }
            }

            pieceInventoryView.Bind(inventory);

            // Placement

            var piecePlacementController = new PlacementController(pieceBoard);
            var inventoryUsageController = new InventoryUsageController(inventory);

            // --

            var inventoryDragAdapter = new InventoryDragAdapter(dragController, pieceInventoryView, new InventoryDragOptions()
            {
                CreateProxy = dragProxyFactory != null,
                ProxyFactory = dragProxyFactory
            });

            inventoryDragAdapter.DragStarted += (inventoryItem) =>
            {
                inventoryUsageController.BeginUsage(inventoryItem);
                piecePlacementController.BeginCreatePlacement(inventoryItem.Data);
            };

            inventoryDragAdapter.DragEnded += () =>
            {
                inventoryUsageController.EndUsage();
                piecePlacementController.EndPlacement();
            };

            // --

            var pieceDragAdapter = new PieceDragAdapter(dragController, pieceBoardView, new PieceDragOptions()
            {
                CreateProxy = dragProxyFactory != null,
                ProxyFactory = dragProxyFactory
            });

            pieceDragAdapter.DragStarted += (piece) =>
            {
                piecePlacementController.BeginMovePlacement(piece);
            };

            pieceDragAdapter.DragEnded += () =>
            {
                piecePlacementController.EndPlacement();
            };

            pieceDragAdapter.DragCancelled += (reason) =>
            {
                if (reason != DragCancelReason.ReleasedNoTarget)
                    return;

                if (piecePlacementController.CurrentPlacement.Kind == PlacementType.Move)
                {
                    var piece = piecePlacementController.CurrentPlacement.Piece;

                    piecePlacementController.BeginRemovePlacement(piece);
                    piecePlacementController.ConfirmPlacement();
                    inventoryUsageController.ReturnItem(new InventoryItem<PieceDefinition>(null, piece.Definition));
                }
            };

            // --

            var boardDragAdapter = new BoardDropAdapter(piecePlacementController, dragController, pieceBoardView, boardView);
            boardDragAdapter.DragDropped += (payload, cellPosition) =>
            {
                var placementOpResult = piecePlacementController.ValidatePlacementAt(cellPosition);
                if (!placementOpResult.Success)
                    return;

                // Resolve inventory usage
                if (piecePlacementController.CurrentPlacement.Kind == PlacementType.Create)
                {
                    var inventoryOpresult = inventoryUsageController.CanConfirmUsage();
                    if (!inventoryOpresult.Success)
                        return;

                    inventoryUsageController.ConfirmUsage();
                }

                piecePlacementController.SetPlacementPosition(cellPosition);
                piecePlacementController.ConfirmPlacement();
            };

            // --

            victoryValidator = new VictoryValidator(new IVictoryRule[]
            {
                new EmptyInventoryRule(),
                new SingleChainOnBoardRule(
                    startPieceId,
                    new OrMatchRule(new IMatchRule[]
                    {
                        new ColorMatchRule(),
                        new TagMatchRule()
                    }))
            });
        }
    }
}
