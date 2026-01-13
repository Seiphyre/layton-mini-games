using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace OneStopShop
{
    public sealed class PlacementOperationResult
    {
        public bool Success { get; }
        public string Reason { get; }

        private PlacementOperationResult(bool success, string reason)
        {
            Success = success;
            Reason = reason;
        }

        public static PlacementOperationResult Ok()
            => new(true, null);

        public static PlacementOperationResult Fail(string reason)
            => new(false, reason);

        public static PlacementOperationResult FromBoard(PieceBoardOperationResult boardResult)
            => boardResult.Success
                ? Ok()
                : Fail(boardResult.Reason);
    }
}