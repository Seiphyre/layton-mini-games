using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
{
    [CreateAssetMenu(menuName = "Game Config")]
    public class AppConfig : ScriptableObject
    {
        public int StartLevel = 0;
        public List<LevelDefinition> Levels = new();
    }
}
