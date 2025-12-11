using System.Collections.Generic;

namespace BoardSystem
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

        Piece GetPiece(int x, int y);
        IEnumerable<Piece> GetAllPieces();
    }
}
