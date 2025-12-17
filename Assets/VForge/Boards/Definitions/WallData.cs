using System;
using UnityEngine;
using VForge.Boards.Runtime;

namespace VForge.Boards.Definitions
{
    [Serializable]
    public class WallData
    {
        public int X;
        public int Y;

        public EdgeAxis Axis;

        public WallData(EdgeAxis axis, int x, int y)
        {
            this.Axis = axis;
            this.X = x;
            this.Y = y;
        }
    }
}