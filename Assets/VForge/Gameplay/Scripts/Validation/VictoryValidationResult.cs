using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace VForge.Gameplay
{
    public class VictoryValidationResult
    {
        public bool IsValid;
        public string FailureReason;

        public IReadOnlyList<Piece> InvalidPieces;

        private VictoryValidationResult(bool isValid, string failureReason, IEnumerable<Piece> invalidPieces)
        {
            IsValid = isValid;
            FailureReason = failureReason;
            InvalidPieces = new List<Piece>(invalidPieces);
        }

        public static VictoryValidationResult Success() => new VictoryValidationResult(true, null, new List<Piece>());
        public static VictoryValidationResult Fail(string reason) => new VictoryValidationResult(false, reason, new List<Piece>());
        public static VictoryValidationResult Fail(string reason, IEnumerable<Piece> pieces) => new VictoryValidationResult(false, reason, pieces);
        public static VictoryValidationResult Fail(string reason, Piece piece) => new VictoryValidationResult(false, reason, new List<Piece>() { piece });
    }
}
