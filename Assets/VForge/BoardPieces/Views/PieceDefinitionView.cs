using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VForge.BoardPieces.Definitions;

namespace VForge.BoardPieces.Views
{
    public class PieceDefinitionView : UIElement
    {
        [SerializeField] private RectTransform pieceRoot;
        [SerializeField] private PieceBlockView blockPrefab;

        private float blockSize;
        private PieceDefinition definition;

        protected readonly List<PieceBlockView> _blockViews = new();
        public IReadOnlyList<PieceBlockView> blockViews => _blockViews;

        // --

        public void Initialize(PieceDefinition definition, float blockSize)
        {
            if (this.definition != null)
            {
                //Debug.LogWarning("PieceView already initialized.");
                return;
            }

            this.definition = definition;
            this.blockSize = blockSize;

            CreateView();
        }

        private void CreateView()
        {
            foreach (Transform c in pieceRoot)
            {
                var block = c.GetComponent<PieceBlockView>();
                if (block != null)
                    _blockViews.Remove(block);

                Destroy(c.gameObject);
            }

            var boundBox = GetBoundBox();

            SetSize(boundBox);
            SetLayoutSize(boundBox);

            foreach (var cell in definition.Shape.Cells)
            {
                var b = Instantiate(blockPrefab, pieceRoot);

                b.name = $"Block ({cell.x},{cell.y})";

                b.RectTransform.anchorMax = Vector2.zero;
                b.RectTransform.anchorMin = Vector2.zero;
                b.RectTransform.pivot = Vector2.zero;

                b.SetColor(definition.Style.Color);
                b.SetLocalOffset(cell, blockSize);
                b.SetSize(new Vector2(blockSize, blockSize));
                b.SetLayoutSize(new Vector2(blockSize, blockSize));

                _blockViews.Add(b);
            }
        }

        public Vector2 GetBoundBox()
        {
            float maxX = definition.Shape.Cells.Max(cell => cell.x);
            float maxY = definition.Shape.Cells.Max(cell => cell.y);

            return new Vector2(1 + maxX, 1 + maxY) * blockSize;
        }
    }
}
