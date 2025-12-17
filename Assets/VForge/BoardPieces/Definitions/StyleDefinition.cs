using UnityEngine;

namespace VForge.BoardPieces.Definitions
{
    [CreateAssetMenu(menuName = "Piece/Style")]
    public class StyleDefinition : ScriptableObject
    {
        public Color Color = Color.white;
    }
}
