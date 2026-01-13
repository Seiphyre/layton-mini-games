using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.Boards.Definitions;

namespace OneStopShop
{
    [CreateAssetMenu(menuName = "Level")]
    public class LevelData : ScriptableObject
    {
        public string Name;
        public BoardDefinition BoardDefinition;
        public StartPiece StartPiece;
        public List<StartInventoryItem> StartInventory = new();
    }
}
