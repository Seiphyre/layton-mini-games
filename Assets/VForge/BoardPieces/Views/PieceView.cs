using System.Linq;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.Boards.Views;

namespace VForge.BoardPieces.Views
{
    public class PieceView : UIElement
    {
        [SerializeField] private PieceDefinitionView definitionView;

        private IBoardViewContext boardContext;
        private Piece piece;

        public Piece Piece { get { return piece; } }



        public void Initialize(Piece piece, IBoardViewContext viewContext)
        {
            if (this.piece != null)
            {
                Debug.LogWarning("PieceView already initialized.");
                return;
            }

            this.piece = piece;
            this.boardContext = viewContext;

            definitionView.Initialize(piece.Definition, viewContext.CellSizePx);
        }

        public void RefreshView()
        {

        }

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

            foreach (var block in definitionView.blockViews)
            {
                block.SetColor(tint);
            }
        }

    }
}
