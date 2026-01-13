using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace OneStopShop
{
    public sealed class BoardPlacementOperationResult
    {
        public bool Success { get; }
        public string Reason { get; }

        private BoardPlacementOperationResult(bool success, string reason)
        {
            Success = success;
            Reason = reason;
        }

        public static BoardPlacementOperationResult Ok()
            => new(true, null);

        public static BoardPlacementOperationResult Fail(string reason)
            => new(false, reason);

        public static BoardPlacementOperationResult FromBoard(PieceBoardOperationResult boardResult)
            => boardResult.Success
                ? Ok()
                : Fail(boardResult.Reason);
    }
}