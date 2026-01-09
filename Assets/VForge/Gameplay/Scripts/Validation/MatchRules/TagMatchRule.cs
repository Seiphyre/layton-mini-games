using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace VForge.Gameplay
{
    public class TagMatchRule : IMatchRule
    {
        public bool IsMatch(Piece a, Piece b)
            => a.Definition.Tag == b.Definition.Tag;
    }
}
