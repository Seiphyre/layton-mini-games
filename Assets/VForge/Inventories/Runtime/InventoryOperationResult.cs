namespace VForge.Inventories
{
    public readonly struct InventoryOperationResult
    {
        public bool Success { get; }
        public string Reason { get; }

        private InventoryOperationResult(bool success, string reason)
        {
            Success = success;
            Reason = reason;
        }

        public static InventoryOperationResult Ok()
            => new(true, null);

        public static InventoryOperationResult Fail(string reason)
            => new(false, reason ?? "Operation failed.");
    }
}
