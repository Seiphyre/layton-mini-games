using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VForge.BoardPieces.Definitions;

namespace VForge.BoardPieces.Views
{
    public class PieceDefinitionView : UIElement
    {
        [Header("Blocks Settings"), Space]
        [SerializeField] private RectTransform blocksRoot;
        [SerializeField] private PieceBlockView blockPrefab;
        [SerializeField] private bool showBlocks = true;

        [Header("Piece Settings"), Space]
        [SerializeField] private Image coloredVisual;
        [SerializeField] private Image maskVisual;
        [SerializeField] private bool showPiece = true;

        private float blockSize;
        private PieceDefinition definition;

        protected readonly List<PieceBlockView> _blockViews = new();
        public IReadOnlyList<PieceBlockView> blockViews => _blockViews;

        // --

        public void Initialize(PieceDefinition definition, float blockSize)
        {
            this.definition = definition;
            this.blockSize = blockSize;

            CreateView();
        }

        private void CreateView()
        {
            foreach (Transform c in blocksRoot)
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
                var b = Instantiate(blockPrefab, blocksRoot);

                b.name = $"Block ({cell.x},{cell.y})";

                b.RectTransform.anchorMax = Vector2.zero;
                b.RectTransform.anchorMin = Vector2.zero;
                b.RectTransform.pivot = Vector2.zero;

                b.SetColor(definition.Style.Color.WithOpacity(GetBlockOpacity()));
                b.SetLocalOffset(cell, blockSize);
                b.SetSize(new Vector2(blockSize, blockSize));
                b.SetLayoutSize(new Vector2(blockSize, blockSize));

                _blockViews.Add(b);
            }

            if (coloredVisual != null)
            {
                coloredVisual.sprite = definition.Visual.ColoredSprite;
            }

            if (maskVisual != null)
            {
                maskVisual.sprite = definition.Visual.MaskSprite;
                maskVisual.color = definition.Style.Color;
            }
        }

        public Vector2 GetBoundBox()
        {
            float maxX = definition.Shape.Cells.Max(cell => cell.x);
            float maxY = definition.Shape.Cells.Max(cell => cell.y);

            return new Vector2(1 + maxX, 1 + maxY) * blockSize;
        }

        private float GetBlockOpacity()
        {
            return (showBlocks, showPiece) switch {
                (showBlocks: true, showPiece: false) => 1,
                (showBlocks: true, showPiece: true) => 0.5f,
                _ => 0,
            };
        }
    }
}
