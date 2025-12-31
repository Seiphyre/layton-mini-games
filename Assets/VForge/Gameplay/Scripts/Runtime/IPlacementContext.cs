using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VForge.Gameplay
{
    public interface IPlacementContext
    {
        PlacementInfo CurrentPlacement { get; }
        public bool IsPlacing { get; }



        PlacementOperationResult ValidatePlacementAt(Vector2Int cellPosition);
    }
}
