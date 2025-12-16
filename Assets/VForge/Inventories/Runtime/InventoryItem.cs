namespace VForge.Inventories
{
    /// <summary>
    /// Runtime inventory item wrapper providing stable identity.
    /// </summary>
    public sealed class InventoryItem<T>
    {
        public string Id { get; }
        public T Data { get; }

        public InventoryItem(string id, T data)
        {
            Id = string.IsNullOrWhiteSpace(id)
                ? System.Guid.NewGuid().ToString("N")
                : id;

            Data = data;
        }

        public override string ToString()
            => $"{typeof(T).Name}({Id})";
    }
}