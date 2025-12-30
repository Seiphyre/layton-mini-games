using System.Linq;
using UnityEngine;
using VForge.BoardPieces.Definitions;
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

        public Piece Piece { get { return piece; } }

        //private bool isPreview;
        //private PieceDefinition previewDefinition;

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
            //UpdateView();
        }

        //public void InitializePreview(PieceDefinition definition, IBoardViewContext viewContext)
        //{
        //    if (isPreview)
        //        return;

        //    isPreview = true;
        //    previewDefinition = definition;
        //    boardContext = viewContext;

        //    CreatePreviewView();
        //}

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

        //private void CreatePreviewView()
        //{
        //    foreach (Transform c in pieceRoot)
        //        Destroy(c.gameObject);

        //    RectTransform.sizeDelta = GetPreviewBoundBox();

        //    foreach (var cell in previewDefinition.Shape.Cells)
        //    {
        //        var b = Instantiate(blockPrefab, pieceRoot);

        //        b.name = $"Preview Block ({cell.x},{cell.y})";

        //        b.RectTransform.anchorMin = Vector2.zero;
        //        b.RectTransform.anchorMax = Vector2.zero;
        //        b.RectTransform.pivot = Vector2.zero;

        //        b.SetColor(previewDefinition.Style.Color);
        //        b.SetLocalOffset(cell, boardContext.CellSizePx);
        //        b.SetSize(new Vector2(boardContext.CellSizePx, boardContext.CellSizePx));
        //    }
        //}

        public void RefreshView()
        {

        }

        //public void UpdateView()
        //{
        //    Vector2 pos = boardContext.CellPositionToLocalPosition(piece.CellPosition);
        //    transform.localPosition = pos;
        //}

        public Vector2 GetBoundBox()
        {
            float maxX = piece.Definition.Shape.Cells.Max(cell => cell.x);
            float maxY = piece.Definition.Shape.Cells.Max(cell => cell.y);

            return new Vector2(1 + maxX, 1 + maxY) * boardContext.CellSizePx;
        }

        //private Vector2 GetPreviewBoundBox()
        //{
        //    float maxX = previewDefinition.Shape.Cells.Max(cell => cell.x);
        //    float maxY = previewDefinition.Shape.Cells.Max(cell => cell.y);

        //    return new Vector2(1 + maxX, 1 + maxY) * boardContext.CellSizePx;
        //}


        //public void SetCellPosition(Vector2Int cell)
        //{
        //    if (!isPreview)
        //        return;

        //    transform.localPosition =
        //        boardContext.CellPositionToLocalPosition(cell);
        //}

        public void SetPreviewMode(bool preview)
        {
            //if (!isPreview)
            //    return;

            if (TryGetComponent<CanvasGroup>(out var cg))
                cg.alpha = preview ? 0.5f : 1f;
        }

        public void SetValidity(bool isValid)
        {
            //if (!isPreview)
            //    return;

            Color tint = isValid ? piece.Definition.Style.Color : Color.red;

            foreach (Transform c in pieceRoot)
            {
                var block = c.GetComponent<PieceBlockView>();
                if (block != null)
                    block.SetColor(tint);
            }
        }

    }
}
