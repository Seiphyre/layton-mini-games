namespace VForge.BoardPieces.Runtime
{
    public struct PieceBoardOperationResult
    {
        public bool Success;
        public string Reason;

        public static PieceBoardOperationResult Ok() => new() { Success = true };

        public static PieceBoardOperationResult Fail(string reason) => new() { Success = false, Reason = reason };
    }
}
