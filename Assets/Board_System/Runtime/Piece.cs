namespace BoardSystem
{
    public class Piece
    {
        public int X { get; internal set; }
        public int Y { get; internal set; }



        // --------------------------------
        // Constructors
        // --------------------------------

        internal Piece(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        internal Piece(Piece other)
        {
            this.X = other.X;
            this.Y = other.Y;
        }



        // --------------------------------
        // Utils
        // --------------------------------

        internal Piece Clone() => new Piece(this);

        internal PieceData ToSaveData()
        {
            return new PieceData(X, Y);
        }
    }
}
