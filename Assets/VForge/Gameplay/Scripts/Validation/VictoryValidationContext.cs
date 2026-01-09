using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Runtime;

public class VictoryValidationContext
{
    public PieceBoard PieceBoard { get; }

    public VictoryValidationContext(PieceBoard pieceBoard)
    {
        this.PieceBoard = pieceBoard;
    }
}
