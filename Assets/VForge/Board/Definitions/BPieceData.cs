using System;

namespace VForge.Boards.Definitions
{
    [Serializable]
    public class BPieceData
    {
        public int X;
        public int Y;


        public BPieceData(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}