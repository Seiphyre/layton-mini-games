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
        public Vector2Int Origin { get; private set; }
        public bool IsLocked { get; private set; }

        public Piece(PieceDefinition definition, Vector2Int origin, bool locked)
        {
            Id = NextId++;
            Definition = definition;
            Origin = origin;
            IsLocked = locked;
        }

        public void SetOrigin(Vector2Int origin) => Origin = origin;
        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;

        public IEnumerable<Vector2Int> GetOccupiedCells()
        {
            foreach (var c in Definition.Shape.Cells)
                yield return Origin + c;
        }
    }
}
