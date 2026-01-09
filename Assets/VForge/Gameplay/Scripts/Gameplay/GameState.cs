using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.Inventories;

namespace VForge.Gameplay
{
    public class GameState
    {
        public BoardState BoardState { get; }
        public InventoryState InventoryState { get; }



        // --------------------------------------------------
        // Constructor
        // --------------------------------------------------

        public GameState(PieceBoard board, Inventory<PieceDefinition> inventory) 
        {
            BoardState = new BoardState(board);
            InventoryState = new InventoryState(inventory);
        }
    }
}
