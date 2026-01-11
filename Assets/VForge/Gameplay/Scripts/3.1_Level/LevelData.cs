using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.Boards.Definitions;
using VForge.Gameplay;

namespace VForge.Gameplay
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
