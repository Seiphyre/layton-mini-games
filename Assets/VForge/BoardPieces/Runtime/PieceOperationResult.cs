namespace VForge.BoardPieces.Runtime
{
    public struct PieceOperationResult
    {
        public bool Success;
        public string Reason;

        public static PieceOperationResult Ok() => new() { Success = true };

        public static PieceOperationResult Fail(string reason) => new() { Success = false, Reason = reason };
    }
}
