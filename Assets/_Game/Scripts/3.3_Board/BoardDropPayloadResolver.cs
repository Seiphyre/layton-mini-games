using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.Inventories;

namespace OneStopShop
{
    public sealed class BoardDropPayloadResolver
    {
        public bool TryResolve(object payload, out BoardDropPayloadInfo boardPayload)
        {
            switch (payload)
            {
                case InventoryItem<PieceDefinition> item:
                    boardPayload = BoardDropPayloadInfo.Create(item);
                    return true;

                case Piece piece:
                    boardPayload = BoardDropPayloadInfo.Create(piece);
                    return true;

                default:
                    boardPayload = BoardDropPayloadInfo.Unknown();
                    return false;
            }
        }
    }

}
