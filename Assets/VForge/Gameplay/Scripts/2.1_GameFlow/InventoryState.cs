using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.Inventories;

namespace VForge.Gameplay
{
    public class InventoryState
    {
        public IReadOnlyList<InventoryItem<PieceDefinition>> InventoryItems { get; }



        // --------------------------------------------------
        // Constructor
        // --------------------------------------------------

        public InventoryState(Inventory<PieceDefinition> inventory)
        {
            InventoryItems = inventory.Items;
        }




        // --------------------------------------------------
        // Helpers
        // --------------------------------------------------

        public int ItemCount => InventoryItems.Count;
    }
}