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
using VForge.Gameplay.UI;

namespace VForge.Gameplay
{
    public class GameStartup : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private BoardView boardView;
        //[SerializeField] private InventoryView pieceInventoryPresenter;
        //[SerializeField] private BoardDropAdapter boardDropAdapter;
        //[SerializeField] private InventoryDragAdapter inventoryDragAdapter;

        [Header("Data References")]
        [SerializeField] private InventoryDefinition piecesSetData;
        [SerializeField] private BoardDefinition boardData;

        [Header("Prefab References")]
        [SerializeField] private PieceBoardView pieceBoardViewPrefab;

        private PieceBoard pieceBoard;

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

            // 3. Build runtime inventory and link it to view
            var inventory = new Inventory<PieceDefinition>();

            foreach (var piece in piecesSetData.Pieces)
            {
                inventory.Add(new InventoryItem<PieceDefinition>(null, piece.Definition));
            }

            //pieceInventoryPresenter.SetList(inventory.Items);

            // 4. Load starting pieces
            LoadStartingPieces(pieceBoard, piecesSetData);

            // 5. Placement / Drag & Drop
            var placePieceController = new PiecePlacementController(pieceBoard, inventory);

            //inventoryDragAdapter.Initialize(inventory);
            //boardDropAdapter.Initialize(placePieceController);
        }

        public void LoadStartingPieces(PieceBoard board, InventoryDefinition dataSet)
        {
            if (board == null || dataSet == null)
                return;

            foreach (var data in dataSet.Pieces)
            {
                if (!data.HasStartingPosition)
                    continue;

                var result = board.TryPlace(
                    data.Definition,
                    data.StartingPosition,
                    data.Locked,
                    out var piece);

                if (!result.Success)
                {
                    Debug.LogError($"Failed to place starting piece {data.Id}: {result.Reason}");
                    continue;
                }
            }
        }

        Piece piece;
        Piece Piece
        {
            get
            {
                if (piece == null)
                {
                    var inventoryItem = piecesSetData.Pieces.First();
                    piece = pieceBoard.GetPieceAt(inventoryItem.StartingPosition.x, inventoryItem.StartingPosition.y);
                }

                return piece;
            }
        }

        private void Update()
        {
            Vector2Int dir = Vector2Int.zero;

            if (Input.GetKeyDown(KeyCode.DownArrow))
                dir = Vector2Int.down;

            if (Input.GetKeyDown(KeyCode.UpArrow))
                dir = Vector2Int.up;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                dir = Vector2Int.left;

            if (Input.GetKeyDown(KeyCode.RightArrow))
                dir = Vector2Int.right;

            if (dir != Vector2Int.zero)
            {
                Debug.Log("Move");
                var res = pieceBoard.TryMove(Piece, Piece.CellPosition + dir);

                if (!res.Success)
                {
                    Debug.Log(res.Reason);
                }
            }
        }
    }
}
