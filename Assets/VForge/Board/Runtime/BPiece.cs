using VForge.Boards.Definitions;

namespace VForge.Boards.Runtime
{
    public class BPiece
    {
        public int X { get; internal set; }
        public int Y { get; internal set; }



        // --------------------------------
        // Constructors
        // --------------------------------

        internal BPiece(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        internal BPiece(BPiece other)
        {
            this.X = other.X;
            this.Y = other.Y;
        }



        // --------------------------------
        // Utils
        // --------------------------------

        internal BPiece Clone() => new BPiece(this);

        internal BPieceData ToSaveData()
        {
            return new BPieceData(X, Y);
        }
    }
}
