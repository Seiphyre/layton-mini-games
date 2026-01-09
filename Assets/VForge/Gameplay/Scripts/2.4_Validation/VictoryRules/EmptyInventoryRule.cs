using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VForge.Gameplay
{
    public class EmptyInventoryRule : IVictoryRule
    {
        public VictoryValidationResult Evaluate(GameState context)
        {
            if (context.InventoryState.ItemCount != 0)
            {
                return VictoryValidationResult.Fail("Inventory not empty");
            }

            return VictoryValidationResult.Success();
        }
    }
}
