using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;

namespace VForge.BoardPieces.Runtime
{
    public class Piece
    {
        private static int NextId = 1;

        public int Id { get; }
        public PieceDefinition Definition { get; }
        public Vector2Int CellPosition { get; internal set; }
        public bool IsLocked { get; internal set; }


        public Piece(PieceDefinition definition, Vector2Int cellPosition, bool locked)
        {
            Id = NextId++;
            Definition = definition;
            CellPosition = cellPosition;
            IsLocked = locked;
        }

        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;

        public IEnumerable<Vector2Int> GetOccupiedCells()
        {
            foreach (var c in Definition.Shape.Cells)
                yield return CellPosition + c;
        }
    }
}
