using UnityEngine;

namespace VForge.BoardPieces.Definitions
{
    [CreateAssetMenu(menuName = "Piece/Definition")]
    public class PieceDefinition : ScriptableObject
    {
        public string Id;
        public ShapeDefinition Shape;
        public PieceVisualDefinition Visual;
        public StyleDefinition Style;
    }
}
