using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.Boards.Definitions;

namespace OneStopShop
{
    [CreateAssetMenu(menuName = "Level")]
    public class LevelDefinition : ScriptableObject
    {
        public string Name;
        public Sprite Thumbail;
        public BoardDefinition BoardDefinition;
        public StartPiece StartPiece;
        public List<StartInventoryItem> StartInventory = new();
    }
}
