using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
{
    public class VictoryValidator
    {
        private readonly List<IVictoryRule> _rules = new();

        public VictoryValidator(IEnumerable<IVictoryRule> rules)
        {
            _rules.AddRange(rules);
        }

        public VictoryValidationResult Validate(GameState gameState)
        {
            foreach (var rule in _rules)
            {
                var result = rule.Evaluate(gameState);
                if (!result.IsValid)
                    return result;
            }

            return VictoryValidationResult.Success();
        }
    }
}
