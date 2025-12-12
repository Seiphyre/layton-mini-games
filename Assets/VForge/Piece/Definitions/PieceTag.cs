using UnityEngine;

namespace VForge.BoardPieces.Definitions
{
    [CreateAssetMenu(menuName = "Piece/Tag")]
    public class PieceTag : ScriptableObject
    {
        public string Id;
        public string DisplayName;
    }
}
