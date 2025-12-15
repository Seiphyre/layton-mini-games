using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Runtime;
using VForge.Boards.Views;

namespace VForge.BoardPieces.Views
{
    public class PieceBoardView : UIElement
    {
        [SerializeField] private RectTransform boardRoot;
        [SerializeField] private PieceView piecePrefab;

        private IBoardViewContext viewContext;
        private PieceBoard board;
        private readonly Dictionary<int, PieceView> pieceViews = new();

        public void Initialize(PieceBoard board, IBoardViewContext viewContext)
        {
            if (this.board != null)
            {
                Debug.LogWarning("PieceBoardView already initialized.");
                return;
            }

            this.board = board;
            this.viewContext = viewContext;

            board.OnPiecePlaced += OnPlaced;
            board.OnPieceMoved += OnMoved;
            board.OnPieceRemoved += OnRemoved;

            Rebuild();
        }

        private void OnDestroy()
        {
            if (board == null)
                return;

            board.OnPiecePlaced -= OnPlaced;
            board.OnPieceMoved -= OnMoved;
            board.OnPieceRemoved -= OnRemoved;
        }

        private void Rebuild()
        {
            foreach (var v in pieceViews.Values)
                Destroy(v.gameObject);
            pieceViews.Clear();

            ResizeView();

            foreach (var p in board.PlacedPieces)
                CreatePieceView(p);
        }

        private void ResizeView()
        {
            SetSize(viewContext.BoardSizePx);
        }

        private void OnPlaced(Piece p) => CreatePieceView(p);

        private void OnMoved(Piece p)
        {
            if (pieceViews.TryGetValue(p.Id, out var view))
                view.UpdateView();
        }

        private void OnRemoved(Piece p)
        {
            Destroy(pieceViews[p.Id].gameObject);
            pieceViews.Remove(p.Id);
        }

        private void CreatePieceView(Piece p)
        {
            var v = Instantiate(piecePrefab, boardRoot);

            v.name = $"Piece {p.Id} ({p.CellPosition.x},{p.CellPosition.y})";

            v.RectTransform.anchorMin = Vector2.zero;
            v.RectTransform.anchorMax = Vector2.zero;
            v.RectTransform.pivot = Vector2.zero;

            v.Initialize(p, viewContext);

            pieceViews[p.Id] = v;
        }
    }
}
