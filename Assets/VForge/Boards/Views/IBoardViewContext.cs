using UnityEngine;

namespace VForge.Boards.Views
{
    public interface IBoardViewContext
    {
        float CellSizePx { get; }
        Vector2 BoardSizePx { get; }
        Vector2 CellPositionToLocalPosition(Vector2Int cell);
    }
}
