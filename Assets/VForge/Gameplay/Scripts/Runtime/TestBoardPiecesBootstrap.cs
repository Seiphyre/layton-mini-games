using UnityEngine;
using VForge.Boards.Runtime;
using VForge.Boards.Views;
using VForge.BoardPieces.Runtime;
using VForge.Gameplay;
using VForge.BoardPieces.Views;
using System.Linq;
using VForge.BoardPieces.Definitions;
using VForge.Inventories;

public class TestBoardPiecesBootstrap : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private BoardView boardView;
    [SerializeField] private PieceInventoryPresenter pieceInventoryPresenter;

    [Header("Data References")]
    [SerializeField] private PieceDataSet pieceDataSet;

    [Header("Prefab References")]
    [SerializeField] private PieceBoardView pieceBoardViewPrefab;

    private PieceBoard pieceBoard;

    private void Start()
    {
        // 1. Build runtime board from BoardData
        var boardData = boardView.BoardData;
        var board = new Board(boardData);

        // 2. Build runtime pieceBoard and view from board
        pieceBoard = new PieceBoard(board);

        var pieceBoardView = Instantiate(pieceBoardViewPrefab);
        pieceBoardView.name = "Pieces";
        pieceBoardView.Initialize(pieceBoard, boardView);

        boardView.AttachToLayer(BoardViewLayer.Pieces, pieceBoardView.RectTransform);

        // 3. Load starting pieces
        PieceBoardInitializer.LoadStartingPieces(pieceBoard, pieceDataSet);

        // 4.
        var inventory = new Inventory<PieceDefinition>();

        foreach (var piece in pieceDataSet.Pieces)
        {
            inventory.Add(new InventoryItem<PieceDefinition>(null, piece.Definition));
        }

        pieceInventoryPresenter.SetList(inventory.Items);
    }

    Piece piece;
    Piece Piece
    {
        get
        {
            if (piece == null)
            {
                var inventoryItem = pieceDataSet.Pieces.First();
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
