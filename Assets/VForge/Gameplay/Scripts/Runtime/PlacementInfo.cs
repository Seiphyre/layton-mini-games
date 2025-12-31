using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;

namespace VForge.Gameplay
{
    public readonly struct PlacementInfo
    {
        public PlacementType Kind { get; }
        public PieceDefinition Definition { get; }
        public Piece Piece { get; }

        private PlacementInfo(
            PlacementType kind,
            PieceDefinition definition,
            Piece piece)
        {
            Kind = kind;
            Definition = definition;
            Piece = piece;
        }

        public static PlacementInfo None()
            => new(PlacementType.None, null, null);

        public static PlacementInfo Create(PieceDefinition definition)
            => new(PlacementType.Create, definition, null);

        public static PlacementInfo Move(Piece piece)
            => new(PlacementType.Move, null, piece);

        public static PlacementInfo Remove(Piece piece)
            => new(PlacementType.Remove, null, piece);
    }
}
