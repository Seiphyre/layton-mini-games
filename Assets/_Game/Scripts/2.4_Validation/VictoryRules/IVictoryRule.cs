using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
{
    public interface IVictoryRule
    {
        VictoryValidationResult Evaluate(GameState context);
    }
}
