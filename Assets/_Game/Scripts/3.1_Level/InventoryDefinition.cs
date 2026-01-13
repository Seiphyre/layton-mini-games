using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
{
    [CreateAssetMenu(menuName = "Piece/Data Set")]
    public class InventoryDefinition : ScriptableObject
    {
        public List<StartInventoryItem> Pieces = new();
    }
}
