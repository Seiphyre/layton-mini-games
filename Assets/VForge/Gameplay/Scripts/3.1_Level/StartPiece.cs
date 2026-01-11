using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;

namespace VForge.Gameplay
{
    [System.Serializable]
    public class StartPiece
    {
        public PieceDefinition Definition;
        public Vector2Int Position;
    }
}
