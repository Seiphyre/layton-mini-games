using UnityEngine;
using System.Collections.Generic;
using VForge.BoardPieces.Definitions;
using VForge.Boards;
using VForge.Boards.Runtime;

namespace VForge.BoardPieces.Runtime
{
    public class PiecePlacementValidator
    {
        private readonly Board board;
        private readonly IReadOnlyList<Piece> pieces;

        public PiecePlacementValidator(Board board, IReadOnlyList<Piece> pieces)
        {
            this.board = board;
            this.pieces = pieces;
        }

        public PiecePlacementResult Validate(PieceDefinition def, Vector2Int origin)
        {
            // TODO:
            // - Bounds check
            // - Tile existence
            // - Wall blocking
            // - Piece overlap
            return PiecePlacementResult.Valid();
        }
    }
}
