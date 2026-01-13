using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.Inventories;

namespace OneStopShop
{
    public readonly struct BoardDropPayloadInfo
    {
        public BoardDropPayloadType Type { get; }
        public InventoryItem<PieceDefinition> InventoryItem { get; }
        public Piece Piece { get; }



        // -------------------------------------
        // Constructor
        // -------------------------------------

        private BoardDropPayloadInfo(
            BoardDropPayloadType type,
            InventoryItem<PieceDefinition> inventoryItem,
            Piece piece)
        {
            Type = type;
            InventoryItem = inventoryItem;
            Piece = piece;
        }



        // -------------------------------------
        // Static Factories
        // -------------------------------------

        public static BoardDropPayloadInfo Create(InventoryItem<PieceDefinition> item)
            => new(BoardDropPayloadType.InventoryItem, item, null);

        public static BoardDropPayloadInfo Create(Piece piece)
            => new(BoardDropPayloadType.Piece, null, piece);

        public static BoardDropPayloadInfo Unknown()
            => new(BoardDropPayloadType.Unknown, null, null);
    }
}
