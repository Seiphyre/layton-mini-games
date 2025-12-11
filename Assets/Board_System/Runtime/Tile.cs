namespace BoardSystem
{
    public class Tile
    {
        public int X { get; }
        public int Y { get; }



        // --------------------------------
        // Constructors
        // --------------------------------

        internal Tile(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        internal Tile(Tile other)
        {
            this.X = other.X;
            this.Y = other.Y;
        }



        // --------------------------------
        // Utils
        // --------------------------------

        internal Tile Clone() => new Tile(this);

        internal TileData ToSaveData()
        {
            return new TileData(X, Y);
        }
    }
}