using System.Linq;
using UnityEngine;
using VForge.BoardPieces.Runtime;
using VForge.Boards.Views;

namespace VForge.BoardPieces.Views
{
    public class PieceView : UIElement
    {
        [SerializeField] private RectTransform pieceRoot;
        [SerializeField] private PieceBlockView blockPrefab;

        private IBoardViewContext boardContext;
        private Piece piece;

        public void Initialize(Piece piece, IBoardViewContext viewContext)
        {
            if (this.piece != null)
            {
                Debug.LogWarning("PieceView already initialized.");
                return;
            }

            this.piece = piece;
            this.boardContext = viewContext;

            CreateView();
            UpdateView();
        }

        private void CreateView()
        {
            foreach (Transform c in pieceRoot)
                Destroy(c.gameObject);

            this.RectTransform.sizeDelta = GetBoundBox();

            foreach (var cell in piece.Definition.Shape.Cells)
            {
                var b = Instantiate(blockPrefab, pieceRoot);

                b.name = $"Block ({cell.x},{cell.y})";

                b.RectTransform.anchorMax = Vector2.zero;
                b.RectTransform.anchorMin = Vector2.zero;
                b.RectTransform.pivot = Vector2.zero;

                b.SetColor(piece.Definition.Style.Color);
                b.SetLocalOffset(cell, boardContext.CellSizePx);
                b.SetSize(new Vector2(boardContext.CellSizePx, boardContext.CellSizePx));
            }
        }

        public void UpdateView()
        {
            Vector2 pos = boardContext.CellPositionToLocalPosition(piece.CellPosition);
            transform.localPosition = pos;
        }

        public Vector2 GetBoundBox()
        {
            float maxX = piece.Definition.Shape.Cells.Max(cell => cell.x);
            float maxY = piece.Definition.Shape.Cells.Max(cell => cell.y);

            return new Vector2(1 + maxX, 1 + maxY) * boardContext.CellSizePx;
        }
    }
}
