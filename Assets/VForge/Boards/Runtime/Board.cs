using System.Collections.Generic;
using UnityEngine;
using VForge.Boards.Definitions;

namespace VForge.Boards.Runtime
{
    public class Board
    {
        // ================================
        // Public board dimensions
        // ================================

        public int Width { get; private set; }
        public int Height { get; private set; }



        // ================================
        // Internal storage
        // ================================

        private Tile[,] _tiles;
        private Wall[,] _horizontalWalls; // bottom edge of cell (x,y)
        private Wall[,] _verticalWalls;   // left edge of cell (x,y)



        // =====================================================
        // Constructor
        // =====================================================

        public Board(BoardDefinition data)
        {
            Width = data.Width;
            Height = data.Height;

            _tiles = new Tile[Width, Height];

            _horizontalWalls = new Wall[Width, Height + 1];
            _verticalWalls = new Wall[Width + 1, Height];

            // Load Tiles
            foreach (var td in data.Tiles)
                TryAddTile(td.X, td.Y);

            // Load Walls
            foreach (var w in data.Walls)
                TryAddWall(w.Axis, w.X, w.Y);
        }



        // =====================================================
        // Board Operations
        // =====================================================

        public void Resize(int newWidth, int newHeight)
        {
            // 1. Create new arrays
            Tile[,] newTiles = new Tile[newWidth, newHeight];
            Wall[,] newHoriz = new Wall[newWidth, newHeight + 1];
            Wall[,] newVert = new Wall[newWidth + 1, newHeight];

            // 2. Copy tiles
            for (int x = 0; x < Mathf.Min(Width, newWidth); x++)
            {
                for (int y = 0; y < Mathf.Min(Height, newHeight); y++)
                {
                    newTiles[x, y] = _tiles[x, y];
                }
            }

            // 3. Copy horizontal walls (Width × (Height+1))
            for (int x = 0; x < Mathf.Min(Width, newWidth); x++)
            {
                for (int y = 0; y < Mathf.Min(Height + 1, newHeight + 1); y++)
                {
                    newHoriz[x, y] = _horizontalWalls[x, y];
                }
            }

            // 4. Copy vertical walls ((Width+1) × Height)
            for (int x = 0; x < Mathf.Min(Width + 1, newWidth + 1); x++)
            {
                for (int y = 0; y < Mathf.Min(Height, newHeight); y++)
                {
                    newVert[x, y] = _verticalWalls[x, y];
                }
            }

            // 5. Replace internal data & update dimensions
            _tiles = newTiles;
            _horizontalWalls = newHoriz;
            _verticalWalls = newVert;

            Width = newWidth;
            Height = newHeight;
        }

        public bool IsInsideCell(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
        public bool IsInsideCell(Vector2Int cell) => IsInsideCell(cell.x, cell.y);

        private bool IsInsideHorizontalEdge(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y <= Height;
        }

        private bool IsInsideVerticalEdge(int x, int y)
        {
            return x >= 0 && x <= Width && y >= 0 && y < Height;
        }



        // =====================================================
        // Tiles Operations
        // =====================================================

        public Tile GetTile(int x, int y)
        {
            return IsInsideCell(x, y) ? _tiles[x, y] : null;
        }

        public IEnumerable<Tile> GetAllTiles()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var tile = _tiles[x, y];
                    if (tile != null)
                        yield return tile;
                }
            }
        }

        public bool HasTile(int x, int y)
        {
            return IsInsideCell(x, y) && _tiles[x, y] != null;
        }
        public bool HasTile(Vector2Int cell) => HasTile(cell.x, cell.y);

        public bool TryAddTile(int x, int y)
        {
            if (!IsInsideCell(x, y) || HasTile(x, y))
                return false;

            _tiles[x, y] = new Tile(x, y);
            return true;
        }

        public bool TryRemoveTile(int x, int y)
        {
            if (!IsInsideCell(x, y) || !HasTile(x, y))
                return false;

            _tiles[x, y] = null;
            return true;
        }



        // =====================================================
        // Walls Operations
        // =====================================================

        public Wall GetHorizontalWall(int x, int y)
        {
            return IsInsideHorizontalEdge(x, y) ? _horizontalWalls[x, y] : null;
        }

        public Wall GetVerticalWall(int x, int y)
        {
            return IsInsideVerticalEdge(x, y) ? _verticalWalls[x, y] : null;
        }

        public Wall GetWall(int x, int y, EdgeAxis axis)
        {
            return (axis == EdgeAxis.Horizontal)
                    ? GetHorizontalWall(x, y)
                    : GetVerticalWall(x, y);
        }

        public IEnumerable<Wall> GetAllWalls()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y <= Height; y++)
                    if (_horizontalWalls[x, y] != null)
                        yield return _horizontalWalls[x, y];
            }

            for (int x = 0; x <= Width; x++)
            {
                for (int y = 0; y < Height; y++)
                    if (_verticalWalls[x, y] != null)
                        yield return _verticalWalls[x, y];
            }
        }

        private bool HasHorizontalsWall(int x, int y)
        {
            return IsInsideHorizontalEdge(x, y) && _horizontalWalls[x, y] != null;
        }

        private bool HasVerticalsWall(int x, int y)
        {
            return IsInsideVerticalEdge(x, y) && _verticalWalls[x, y] != null;
        }

        public bool HasWall(int x, int y, EdgeAxis axis)
        {
            return (axis == EdgeAxis.Horizontal)
                    ? HasHorizontalsWall(x, y)
                    : HasVerticalsWall(x, y);
        }

        public bool TryAddWall(EdgeAxis axis, int x, int y)
        {
            if (axis == EdgeAxis.Horizontal)
            {
                if (!IsInsideHorizontalEdge(x, y) || _horizontalWalls[x, y] != null)
                    return false;

                _horizontalWalls[x, y] = new Wall(axis, x, y);
                return true;
            }
            else
            {
                if (!IsInsideVerticalEdge(x, y) || _verticalWalls[x, y] != null)
                    return false;

                _verticalWalls[x, y] = new Wall(axis, x, y);
                return true;
            }
        }

        public bool TryRemoveWall(EdgeAxis axis, int x, int y)
        {
            if (axis == EdgeAxis.Horizontal)
            {
                if (!IsInsideHorizontalEdge(x, y)) return false;
                _horizontalWalls[x, y] = null;
                return true;
            }
            else
            {
                if (!IsInsideVerticalEdge(x, y)) return false;
                _verticalWalls[x, y] = null;
                return true;
            }
        }



        // =====================================================
        //  SAVE RUNTIME MODEL BACK INTO BOARD DATA
        // =====================================================
        public void SaveTo(BoardDefinition data)
        {
            if (data == null)
                return;

            // Update basic dimensions
            data.Width = Width;
            data.Height = Height;

            // Clear existing lists
            data.Tiles.Clear();
            data.Walls.Clear();

            // Save tiles
            foreach (var tile in GetAllTiles())
                data.Tiles.Add(tile.ToSaveData());

            // Save walls
            foreach (var wall in GetAllWalls())
                data.Walls.Add(wall.ToSaveData());
        }

    }

}