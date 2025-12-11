using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoardSystem
{
    [CreateAssetMenu(fileName = "Board", menuName = "Data/Board System/Board")]
    public class BoardData : ScriptableObject
    {
        public int Width = 8;
        public int Height = 8;

        public List<TileData> Tiles = new();
        public List<WallData> Walls = new();
        public List<PieceData> Pieces = new();

        public event Action OnBoardChanged;



#if UNITY_EDITOR
        private void OnValidate()
        {
            // Notify listeners (BoardView)
            OnBoardChanged?.Invoke();
        }
#endif
    }
}
