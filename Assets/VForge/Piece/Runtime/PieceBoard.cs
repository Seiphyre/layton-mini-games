using System;
using System.Collections.Generic;
using UnityEngine;
using VForge.Boards;
using VForge.BoardPieces.Definitions;
using VForge.Boards.Runtime;

namespace VForge.BoardPieces.Runtime
{
    public class PieceBoard
    {
        private readonly Board board;
        private readonly List<Piece> pieces = new();
        private readonly PiecePlacementValidator validator;

        public IReadOnlyList<Piece> PlacedPieces => pieces;

        public event Action<Piece> OnPiecePlaced;
        public event Action<Piece> OnPieceMoved;
        public event Action<Piece> OnPieceRemoved;

        public PieceBoard(Board board)
        {
            this.board = board;
            validator = new PiecePlacementValidator(board, pieces);
        }

        public PieceOperationResult TryPlace(PieceData data, Vector2Int origin, out Piece piece)
        {
            var res = validator.Validate(data.Definition, origin);
            if (!res.IsValid)
            {
                piece = null;
                return PieceOperationResult.Fail(res.Reason);
            }

            piece = new Piece(data.Definition, origin, data.Locked);
            pieces.Add(piece);
            OnPiecePlaced?.Invoke(piece);

            return PieceOperationResult.Ok();
        }

        public PieceOperationResult TryMove(Piece piece, Vector2Int origin)
        {
            if (piece.IsLocked)
                return PieceOperationResult.Fail("Piece is locked.");

            var res = validator.Validate(piece.Definition, origin);
            if (!res.IsValid)
                return PieceOperationResult.Fail(res.Reason);

            piece.SetOrigin(origin);
            OnPieceMoved?.Invoke(piece);
            return PieceOperationResult.Ok();
        }

        public PieceOperationResult TryRemove(Piece piece)
        {
            if (piece.IsLocked)
                return PieceOperationResult.Fail("Piece is locked.");

            if (!pieces.Remove(piece))
                return PieceOperationResult.Fail("Piece not found.");

            OnPieceRemoved?.Invoke(piece);
            return PieceOperationResult.Ok();
        }

        public Piece GetPieceAt(int x, int y)
        {
            foreach (var p in pieces)
                foreach (var c in p.GetOccupiedCells())
                    if (c.x == x && c.y == y)
                        return p;
            return null;
        }
    }
}
