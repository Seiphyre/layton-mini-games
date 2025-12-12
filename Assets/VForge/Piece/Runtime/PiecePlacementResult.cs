namespace VForge.BoardPieces.Runtime
{
    public struct PiecePlacementResult
    {
        public bool IsValid;
        public string Reason;

        public static PiecePlacementResult Valid()
            => new() { IsValid = true };

        public static PiecePlacementResult Invalid(string reason)
            => new() { IsValid = false, Reason = reason };
    }
}
