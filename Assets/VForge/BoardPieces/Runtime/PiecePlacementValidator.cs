using UnityEngine;
using System.Collections.Generic;
using VForge.BoardPieces.Definitions;
using VForge.Boards;
using VForge.Boards.Runtime;
using VForge.Boards.Definitions;

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

        public PiecePlacementValidatorResult Validate(PieceDefinition def, Vector2Int origin, Piece ignoredPiece = null)
        {
            if (def == null || def.Shape == null)
                return PiecePlacementValidatorResult.Invalid("Invalid piece definition.");

            // Precompute candidate occupied cells
            var candidateCells = new HashSet<Vector2Int>();

            foreach (var localCell in def.Shape.Cells)
            {
                var cell = origin + localCell;

                // 1. Bounds check
                if (!board.IsInsideCell(cell))
                    return PiecePlacementValidatorResult.Invalid(
                        $"Cell {cell} is outside board bounds.");

                // 2. Tile existence check
                if (!board.HasTile(cell))
                    return PiecePlacementValidatorResult.Invalid(
                        $"No tile at cell {cell}.");

                candidateCells.Add(cell);
            }

            // 3. Overlap check against existing pieces
            foreach (var piece in pieces)
            {
                if (piece == ignoredPiece)
                    continue;

                foreach (var occupied in piece.GetOccupiedCells())
                {
                    if (candidateCells.Contains(occupied))
                    {
                        return PiecePlacementValidatorResult.Invalid(
                            $"Cell {occupied} is already occupied by another piece.");
                    }
                }
            }

            // 4. Wall blocking check (crossing forbidden, touching allowed)
            foreach (var cell in candidateCells)
            {
                // Check 4 directions
                if (IsWallBlocking(candidateCells, cell, Vector2Int.up, EdgeAxis.Horizontal, cell.x, cell.y + 1) ||
                    IsWallBlocking(candidateCells, cell, Vector2Int.down, EdgeAxis.Horizontal, cell.x, cell.y) ||
                    IsWallBlocking(candidateCells, cell, Vector2Int.left, EdgeAxis.Vertical, cell.x, cell.y) ||
                    IsWallBlocking(candidateCells, cell, Vector2Int.right, EdgeAxis.Vertical, cell.x + 1, cell.y))
                {
                    return PiecePlacementValidatorResult.Invalid($"Wall blocks piece at cell {cell}.");
                }
            }


            return PiecePlacementValidatorResult.Valid();
        }

        private bool IsWallBlocking(HashSet<Vector2Int> candidateCells, Vector2Int cell, Vector2Int dir, EdgeAxis axis, int edgeX, int edgeY)
        {
            var neighbor = cell + dir;

            // Internal edge: both cells belong to the same piece
            if (!candidateCells.Contains(neighbor))
                return false;

            // Boundary edge → wall blocks crossing
            return board.HasWall(edgeX, edgeY, axis);
        }
    }
}
