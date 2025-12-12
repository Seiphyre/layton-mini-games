using UnityEngine;
using UnityEngine.UI;

namespace VForge.BoardPieces.Views
{
    public class PieceBlockView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private RectTransform rect;

        public void SetColor(Color c) => image.color = c;

        public void SetLocalOffset(Vector2Int cell)
        {
            rect.anchoredPosition = new Vector2(cell.x, cell.y);
        }
    }
}
