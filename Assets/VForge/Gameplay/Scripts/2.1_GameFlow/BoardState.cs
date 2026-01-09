using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace VForge.Gameplay
{
    public class BoardState
    {
        public IReadOnlyList<Piece> PlacedPieces{ get; }



        // --------------------------------------------------
        // Constructor
        // --------------------------------------------------

        public BoardState(PieceBoard pieceBoard)
        {
            PlacedPieces = pieceBoard.PlacedPieces;
        }



        // --------------------------------------------------
        // Helpers
        // --------------------------------------------------

        private readonly Vector2Int[] CardinalDirections =
{
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        public IEnumerable<Vector2Int> GetNeighborCells(Piece piece)
        {
            var neighbors = new HashSet<Vector2Int>();
            var occupiedCells = piece.GetOccupiedCells();
            var candidateDirections = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };

            foreach (var cell in occupiedCells)
            {
                foreach (var direction in candidateDirections)
                {
                    var candidate = cell + direction;

                    // Exclude cells already part of the piece
                    if (!occupiedCells.Contains(candidate))
                        neighbors.Add(candidate);
                }
            }

            return neighbors;
        }

        public IEnumerable<Piece> GetNeighborPieces(Piece piece)
        {
            var occupied = piece.GetOccupiedCells(); // cached HashSet
            var neighborCells = GetNeighborCells(piece);

            var neighbors = new HashSet<Piece>();

            foreach (var cell in neighborCells)
            {
                var other = GetPieceAt(cell);
                if (other != null && other != piece)
                    neighbors.Add(other);
            }

            return neighbors;
        }

        private Piece GetPieceAt(Vector2Int cell)
        {
            foreach(var piece in PlacedPieces)
            {
                foreach(var occupiedCell in piece.GetOccupiedCells())
                    if (occupiedCell == cell)
                        return piece;
            }

            return null;
        }
    }
}
