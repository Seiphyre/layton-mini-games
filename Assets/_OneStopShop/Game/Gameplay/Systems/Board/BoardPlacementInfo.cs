using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;

namespace OneStopShop
{
    public readonly struct BoardPlacementInfo
    {
        public BoardPlacementType Kind { get; }
        public PieceDefinition Definition { get; }
        public Piece Piece { get; }

        private BoardPlacementInfo(
            BoardPlacementType kind,
            PieceDefinition definition,
            Piece piece)
        {
            Kind = kind;
            Definition = definition;
            Piece = piece;
        }

        public static BoardPlacementInfo None()
            => new(BoardPlacementType.None, null, null);

        public static BoardPlacementInfo Create(PieceDefinition definition)
            => new(BoardPlacementType.Create, definition, null);

        public static BoardPlacementInfo Move(Piece piece)
            => new(BoardPlacementType.Move, null, piece);

        public static BoardPlacementInfo Remove(Piece piece)
            => new(BoardPlacementType.Remove, null, piece);
    }
}
