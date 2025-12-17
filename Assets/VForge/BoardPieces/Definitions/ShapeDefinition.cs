using System.Collections.Generic;
using UnityEngine;

namespace VForge.BoardPieces.Definitions
{
    [CreateAssetMenu(menuName = "Piece/Shape")]
    public class ShapeDefinition : ScriptableObject
    {
        public List<Vector2Int> Cells = new();
        public int MaxShapeSize = 5;
    }
}
