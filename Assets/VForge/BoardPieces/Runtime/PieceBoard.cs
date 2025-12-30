using System;
using System.Collections.Generic;
using UnityEngine;
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

        public bool Contains(Piece piece)
        {
            return piece != null && pieces.Contains(piece);
        }

        public PieceBoardOperationResult CanPlace(PieceDefinition definition, Vector2Int origin)
        {
            var res = validator.Validate(definition, origin);
            return res.IsValid
                ? PieceBoardOperationResult.Ok()
                : PieceBoardOperationResult.Fail(res.Reason);
        }

        public PieceBoardOperationResult TryPlace(PieceDefinition definition, Vector2Int origin, bool locked, out Piece piece)
        {
            var res = validator.Validate(definition, origin);
            if (!res.IsValid)
            {
                piece = null;
                return PieceBoardOperationResult.Fail(res.Reason);
            }

            piece = new Piece(definition, origin, locked);
            pieces.Add(piece);
            OnPiecePlaced?.Invoke(piece);

            return PieceBoardOperationResult.Ok();
        }

        public PieceBoardOperationResult TryMove(Piece piece, Vector2Int cellPosition)
        {
            if (piece.IsLocked)
                return PieceBoardOperationResult.Fail("Piece is locked.");

            var res = validator.Validate(piece.Definition, cellPosition, piece);
            if (!res.IsValid)
                return PieceBoardOperationResult.Fail(res.Reason);

            piece.CellPosition = cellPosition;
            OnPieceMoved?.Invoke(piece);

            return PieceBoardOperationResult.Ok();
        }

        public PieceBoardOperationResult TryRemove(Piece piece)
        {
            if (piece.IsLocked)
                return PieceBoardOperationResult.Fail("Piece is locked.");

            if (!pieces.Remove(piece))
                return PieceBoardOperationResult.Fail("Piece not found.");

            OnPieceRemoved?.Invoke(piece);
            return PieceBoardOperationResult.Ok();
        }

        public Piece GetPieceAt(int x, int y)
        {
            foreach (var p in pieces)
                foreach (var c in p.GetOccupiedCells())
                    if (c.x == x && c.y == y)
                        return p;
            return null;
        }

        public PieceBoardOperationResult TryLock(Piece piece)
        {
            if (piece == null)
                return PieceBoardOperationResult.Fail("Piece is null.");

            if (!pieces.Contains(piece))
                return PieceBoardOperationResult.Fail("Piece not found on board.");

            if (piece.IsLocked)
                return PieceBoardOperationResult.Fail("Piece is already locked.");

            piece.Lock();
            return PieceBoardOperationResult.Ok();
        }

        public PieceBoardOperationResult TryUnlock(Piece piece)
        {
            if (piece == null)
                return PieceBoardOperationResult.Fail("Piece is null.");

            if (!pieces.Contains(piece))
                return PieceBoardOperationResult.Fail("Piece not found on board.");

            if (!piece.IsLocked)
                return PieceBoardOperationResult.Fail("Piece is not locked.");

            piece.Unlock();
            return PieceBoardOperationResult.Ok();
        }

        public void Clear()
        {
            pieces.Clear();
        }
    }
}
