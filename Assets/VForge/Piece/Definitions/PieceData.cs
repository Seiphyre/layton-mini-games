using UnityEngine;

namespace VForge.BoardPieces.Definitions
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
