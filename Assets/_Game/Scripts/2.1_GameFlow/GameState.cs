using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.Inventories;

namespace OneStopShop
{
    public class GameState
    {
        public bool Started { get; }
        public BoardState BoardState { get; }
        public InventoryState InventoryState { get; }



        //// --------------------------------------------------
        //// Constructor
        //// --------------------------------------------------

        public GameState(PieceBoard board, Inventory<PieceDefinition> inventory, bool started)
        {
            Started = started;
            BoardState = new BoardState(board);
            InventoryState = new InventoryState(inventory);
        }
    }
}
