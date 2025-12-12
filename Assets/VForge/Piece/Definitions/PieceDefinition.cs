using UnityEngine;

namespace VForge.BoardPieces.Definitions
{
    [CreateAssetMenu(menuName = "Piece/Definition")]
    public class PieceDefinition : ScriptableObject
    {
        public string Id;
        public PieceShape Shape;
        public PieceStyle Style;
        public PieceTag Tag;
    }
}
