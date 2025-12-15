using UnityEngine;
using VForge.Boards.Runtime;
using VForge.Boards.Views;
using VForge.BoardPieces.Runtime;
using VForge.Gameplay;
using VForge.BoardPieces.Views;
using System.Linq;

public class TestBoardPiecesBootstrap : MonoBehaviour
{
    [SerializeField] private BoardView boardView;
    [SerializeField] private PieceDataSet pieceDataSet;
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
        pieceBoardView.name = "Pieces Board";

        boardView.AttachToLayer(BoardViewLayer.Pieces, pieceBoardView.RectTransform);

        pieceBoardView.Initialize(pieceBoard, boardView);

        // 3. Load starting pieces
        PieceBoardInitializer.LoadStartingPieces(pieceBoard, pieceDataSet);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            var inventoryItem = pieceDataSet.Pieces.First();
            var piece = pieceBoard.GetPieceAt(inventoryItem.StartingPosition.x, inventoryItem.StartingPosition.y);

            pieceBoard.TryMove(piece, piece.CellPosition + Vector2Int.up);
            // move first piece by 1 cell right
            // (just for testing)
        }
    }
}
