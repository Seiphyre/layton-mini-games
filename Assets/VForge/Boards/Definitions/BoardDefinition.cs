using System;
using System.Collections.Generic;
using UnityEngine;

namespace VForge.Boards.Definitions
{
    [CreateAssetMenu(fileName = "Board", menuName = "Data/Board System/Board")]
    public class BoardDefinition : ScriptableObject
    {
        public int Width = 8;
        public int Height = 8;

        public List<TileData> Tiles = new();
        public List<WallData> Walls = new();
    }
}
