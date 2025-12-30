using UnityEngine;
using VForge.BoardPieces.Definitions;

namespace VForge.BoardPieces.Views
{
    /// <summary>
    /// Visual-only ghost preview for piece placement.
    /// </summary>
    public sealed class PieceGhostView : MonoBehaviour
    {
        public void Show(PieceDefinition definition)
        {
            gameObject.SetActive(true);
            // Minimal: visuals can be added later
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetPosition(Vector2Int cell)
        {
            // Minimal: snap logic can be added later
        }

        public void SetValidity(bool isValid)
        {
            // Minimal: color feedback can be added later
        }
    }
}
