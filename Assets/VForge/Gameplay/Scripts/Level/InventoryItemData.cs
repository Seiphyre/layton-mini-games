using UnityEngine;
using VForge.BoardPieces.Definitions;

namespace VForge.Gameplay
{
    [System.Serializable]
    public class InventoryItemData
    {
        public string Id;
        public PieceDefinition Definition;

        public bool Locked;
        public bool HasStartingPosition;
        public Vector2Int StartingPosition;
    }
}
