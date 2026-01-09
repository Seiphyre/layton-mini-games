using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace VForge.Gameplay
{
    public class OrMatchRule : IMatchRule
    {
        private readonly IMatchRule[] _rules;

        public OrMatchRule(params IMatchRule[] rules)
        {
            _rules = rules;
        }

        public bool IsMatch(Piece a, Piece b)
        {
            foreach (var rule in _rules)
                if (rule.IsMatch(a, b))
                    return true;

            return false;
        }
    }
}
