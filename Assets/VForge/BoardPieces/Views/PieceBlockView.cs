using UnityEngine;
using UnityEngine.UI;
using VForge.Boards.Views;

namespace VForge.BoardPieces.Views
{
    public class PieceBlockView : UIElement
    {
        [SerializeField] private RectTransform blockRoot;
        [SerializeField] private Image image;



        private void Awake()
        {
            if (blockRoot == null)
                blockRoot = GetComponent<RectTransform>();

            if (image == null)
                image = GetComponent<Image>();
        }

        public void SetColor(Color c) => image.color = c;

        public void SetLocalOffset(Vector2Int cell, float cellSize)
        {
            blockRoot.anchoredPosition = new Vector2(
                cell.x * cellSize,
                cell.y * cellSize
            );
        }
    }
}
