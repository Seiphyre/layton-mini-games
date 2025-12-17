namespace VForge.BoardPieces.Runtime
{
    public struct PiecePlacementValidatorResult
    {
        public bool IsValid;
        public string Reason;

        public static PiecePlacementValidatorResult Valid()
            => new() { IsValid = true };

        public static PiecePlacementValidatorResult Invalid(string reason)
            => new() { IsValid = false, Reason = reason };
    }
}
