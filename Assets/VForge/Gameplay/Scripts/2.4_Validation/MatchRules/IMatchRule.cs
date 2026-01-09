using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace VForge.Gameplay
{
    public interface IMatchRule
    {
        bool IsMatch(Piece a, Piece b);
    }
}
