using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace VForge.BoardPieces.Views
{
    public class PieceBoardView : MonoBehaviour
    {
        [SerializeField] private PieceView piecePrefab;
        [SerializeField] private RectTransform container;

        private PieceBoard board;
        private readonly Dictionary<int, PieceView> views = new();

        public void Initialize(PieceBoard board)
        {
            this.board = board;
            board.OnPiecePlaced += OnPlaced;
            board.OnPieceMoved += OnMoved;
            board.OnPieceRemoved += OnRemoved;
            Rebuild();
        }

        private void Rebuild()
        {
            foreach (var v in views.Values)
                Destroy(v.gameObject);
            views.Clear();

            foreach (var p in board.PlacedPieces)
                CreateView(p);
        }

        private void OnPlaced(Piece p) => CreateView(p);
        private void OnMoved(Piece p) => views[p.Id].UpdatePosition();
        private void OnRemoved(Piece p)
        {
            Destroy(views[p.Id].gameObject);
            views.Remove(p.Id);
        }

        private void CreateView(Piece p)
        {
            var v = Instantiate(piecePrefab, container);
            v.Initialize(p);
            views[p.Id] = v;
        }
    }
}
