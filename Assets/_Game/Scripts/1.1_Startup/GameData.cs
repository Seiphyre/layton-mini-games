using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
{
    [CreateAssetMenu(menuName = "Game Config")]
    public class GameConfig : ScriptableObject
    {
        public int StartLevel = 0;
        public List<LevelData> Levels = new();
    }
}
