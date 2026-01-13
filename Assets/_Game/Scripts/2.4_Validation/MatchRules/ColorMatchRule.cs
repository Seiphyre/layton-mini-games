using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace OneStopShop
{
    public class ColorMatchRule : IMatchRule
    {
        public bool IsMatch(Piece a, Piece b)
            => a.Definition.Style.Color == b.Definition.Style.Color;
    }
}
