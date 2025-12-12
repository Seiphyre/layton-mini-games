using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace VForge.BoardPieces.Views
{
    public class PieceView : MonoBehaviour
    {
        [SerializeField] private RectTransform blocksRoot;
        [SerializeField] private PieceBlockView blockPrefab;

        private Piece piece;

        public void Initialize(Piece piece)
        {
            this.piece = piece;
            Build();
            UpdatePosition();
        }

        private void Build()
        {
            foreach (Transform c in blocksRoot)
                Destroy(c.gameObject);

            foreach (var cell in piece.Definition.Shape.Cells)
            {
                var b = Instantiate(blockPrefab, blocksRoot);
                b.SetColor(piece.Definition.Style.Color);
                b.SetLocalOffset(cell);
            }
        }

        public void UpdatePosition()
        {
            transform.localPosition = new Vector3(piece.Origin.x, piece.Origin.y, 0);
        }
    }
}
