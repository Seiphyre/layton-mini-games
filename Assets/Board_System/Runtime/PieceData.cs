using System;

namespace BoardSystem
{
    [Serializable]
    public class PieceData
    {
        public int X;
        public int Y;


        public PieceData(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}