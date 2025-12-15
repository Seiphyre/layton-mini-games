using System;
using UnityEngine;

namespace VForge.Boards.Definitions
{
    [Serializable]
    public class TileData
    {
        public int X;
        public int Y;

        public TileData(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}