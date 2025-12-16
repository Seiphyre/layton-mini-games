using UnityEngine;
using VForge.BoardPieces.Definitions;

namespace VForge.Gameplay
{
    [System.Serializable]
    public class PieceData
    {
        public string Id;
        public PieceDefinition Definition;

        public bool Locked;
        public bool HasStartingPosition;
        public Vector2Int StartingPosition;
    }
}
