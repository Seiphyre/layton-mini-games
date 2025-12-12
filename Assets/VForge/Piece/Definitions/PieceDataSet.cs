using System.Collections.Generic;
using UnityEngine;

namespace VForge.BoardPieces.Definitions
{
    [CreateAssetMenu(menuName = "Piece/Data Set")]
    public class PieceDataSet : ScriptableObject
    {
        public List<PieceData> Pieces = new();
    }
}
