using System.Collections.Generic;
using UnityEngine;

namespace BoardSystem
{
    public class BoardModel : IBoard
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
        private readonly Dictionary<(int x, int y), Piece> _pieces = new();



        // =====================================================
        // Constructor (load runtime from BoardData)
        // =====================================================
        public BoardModel(BoardData data)
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

            // Load Pieces
            foreach (var pd in data.Pieces)
                TryAddPiece(pd.X, pd.Y);
        }



        // =====================================================
        // Basic helper methods
        // =====================================================
        private bool IsInsideCell(int x, int y)
            => x >= 0 && x < Width && y >= 0 && y < Height;

        private bool IsInsideHorizontalEdge(int x, int y)
            => x >= 0 && x < Width && y >= 0 && y <= Height;

        private bool IsInsideVerticalEdge(int x, int y)
            => x >= 0 && x <= Width && y >= 0 && y < Height;

        // ---- Tile helpers ----
        private bool HasTile(int x, int y)
            => _tiles[x, y] != null;


        // ---- Wall helpers ----
        private bool HasHorizontalsWall(int x, int y)
            => _horizontalWalls[x, y] != null;

        private bool HasVerticalsWall(int x, int y)
            => _verticalWalls[x, y] != null;

        public bool HasWall(int x, int y, EdgeAxis axis)
            => (axis == EdgeAxis.Horizontal)
                ? HasHorizontalsWall(x, y)
                : HasVerticalsWall(x, y);


        // ---- Piece helpers ----
        private bool HasPiece(int x, int y)
            => _pieces.ContainsKey((x, y));

        private bool IsValidAndFree(int x, int y)
            => IsInsideCell(x, y) && !HasPiece(x, y);

        // --

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

            // 5. Remove pieces outside new bounds
            var keys = new List<(int, int)>(_pieces.Keys);
            foreach (var k in keys)
                if (k.Item1 >= newWidth || k.Item2 >= newHeight)
                    _pieces.Remove(k);

            // 6. Replace internal data & update dimensions
            _tiles = newTiles;
            _horizontalWalls = newHoriz;
            _verticalWalls = newVert;

            Width = newWidth;
            Height = newHeight;
        }


        // =====================================================
        // Read-only queries (IBoardReadOnly)
        // =====================================================

        public Tile GetTile(int x, int y)
            => IsInsideCell(x, y) ? _tiles[x, y] : null;

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

        public Wall GetHorizontalWall(int x, int y)
            => IsInsideHorizontalEdge(x, y) ? _horizontalWalls[x, y] : null;

        public Wall GetVerticalWall(int x, int y)
            => IsInsideVerticalEdge(x, y) ? _verticalWalls[x, y] : null;

        public Wall GetWall(int x, int y, EdgeAxis axis)
            => (axis == EdgeAxis.Horizontal)
                ? GetHorizontalWall(x, y)
                : GetVerticalWall(x, y);

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

        public Piece GetPiece(int x, int y)
            => _pieces.TryGetValue((x, y), out var p) ? p : null;

        public IEnumerable<Piece> GetAllPieces()
        {
            foreach (var entry in _pieces)
                yield return entry.Value;
        }



        // =====================================================
        // EDITOR OPERATIONS — Tiles (IBoard)
        // =====================================================

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
        // EDITOR OPERATIONS — Walls (IBoard)
        // =====================================================

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
        // GAMEPLAY OPERATIONS — Pieces (IBoardRuntime)
        // =====================================================

        public bool TryAddPiece(int x, int y)
        {
            if (!IsValidAndFree(x, y))
                return false;

            _pieces[(x, y)] = new Piece(x, y);
            return true;
        }

        public bool TryMovePiece(Piece piece, int newX, int newY)
        {
            if (piece == null || !IsValidAndFree(newX, newY))
                return false;

            // Remove old cell
            _pieces.Remove((piece.X, piece.Y));

            // Update
            piece.X = newX;
            piece.Y = newY;

            // Insert in new cell
            _pieces[(newX, newY)] = piece;

            return true;
        }

        public bool TryRemovePiece(int x, int y)
        {
            return _pieces.Remove((x, y));
        }



        // =====================================================
        //  SAVE RUNTIME MODEL BACK INTO BOARD DATA
        // =====================================================
        public void SaveTo(BoardData data)
        {
            if (data == null)
                return;

            // Update basic dimensions
            data.Width = Width;
            data.Height = Height;

            // Clear existing lists
            data.Tiles.Clear();
            data.Walls.Clear();
            data.Pieces.Clear();

            // Save tiles
            foreach (var tile in GetAllTiles())
                data.Tiles.Add(tile.ToSaveData());

            // Save walls
            foreach (var wall in GetAllWalls())
                data.Walls.Add(wall.ToSaveData());

            // Save pieces
            foreach (var piece in GetAllPieces())
                data.Pieces.Add(piece.ToSaveData());
        }

    }

}