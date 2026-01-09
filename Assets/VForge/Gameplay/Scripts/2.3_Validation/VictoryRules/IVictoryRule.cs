using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VForge.Gameplay
{
    public interface IVictoryRule
    {
        VictoryValidationResult Evaluate(GameState context);
    }
}
