using System.Collections.Generic;
using UnityEngine;

namespace VForge.Gameplay
{
    [CreateAssetMenu(menuName = "Piece/Data Set")]
    public class InventoryDefinition : ScriptableObject
    {
        public List<StartInventoryItem> Pieces = new();
    }
}
