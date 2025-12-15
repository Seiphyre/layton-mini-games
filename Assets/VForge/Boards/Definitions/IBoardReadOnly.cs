using System.Collections.Generic;
using VForge.Boards.Runtime;

namespace VForge.Boards.Definitions
{
    public interface IBoardReadOnly
    {
        int Width { get; }
        int Height { get; }

        Tile GetTile(int x, int y);
        IEnumerable<Tile> GetAllTiles();

        Wall GetWall(int x, int y, EdgeAxis axis);
        Wall GetHorizontalWall(int x, int y);
        Wall GetVerticalWall(int x, int y);
        IEnumerable<Wall> GetAllWalls();

        BPiece GetPiece(int x, int y);
        IEnumerable<BPiece> GetAllPieces();
    }
}
