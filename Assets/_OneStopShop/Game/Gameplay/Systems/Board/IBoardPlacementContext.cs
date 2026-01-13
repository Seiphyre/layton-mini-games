using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
{
    public interface IBoardPlacementContext
    {
        BoardPlacementInfo CurrentPlacement { get; }
        public bool IsPlacing { get; }



        BoardPlacementOperationResult ValidatePlacementAt(Vector2Int cellPosition);
    }
}
