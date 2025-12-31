using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;

namespace VForge.Gameplay

{
    public interface IBoardPlacementContext
    {
        PieceDefinition ActiveDefinition { get; } 
        Piece ActivePiece { get; }
        BoardPlacementIntent Intent { get; }


        public bool IsPlacing { get; }
        PieceBoardOperationResult CanPlace(Vector2Int cellPosition);
    }
}
