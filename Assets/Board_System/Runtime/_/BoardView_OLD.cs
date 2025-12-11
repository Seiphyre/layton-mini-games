using BoardSystem;
using System.Collections.Generic;
using UnityEngine;

public class BoardView_OLD : UIElement
{
    [Header("Tile Appearance")]
    [SerializeField] private TileView_OLD tileViewPrefab;
    [SerializeField] private int tileSize = 64;
    [SerializeField] private Color tileColor = Color.white;
    [SerializeField] private Color holeColor = Color.gray;

    [Header("Wall Appearance")]
    [SerializeField] private WallView_OLD wallViewPrefab;
    [SerializeField] private float wallThickness = 8f;
    [SerializeField] private Color wallColor = Color.black;

    [Header("Wall Joint Appearance")]
    [SerializeField] private WallJointView wallJointPrefab;
    [SerializeField] private Color wallJointColor = Color.black;

    private BoardModel board;

    private TileView_OLD[,] tileViews;
    private readonly List<WallView_OLD> wallViews = new();
    private readonly List<WallJointView> wallJointViews = new();


    // ---------------------------------------------------------------------
    // Public API
    // ---------------------------------------------------------------------

    public void Create(BoardModel board)
    {
        this.board = board;

        Destroy();
        UpdateSizeFromBoard();
        CreateTiles();
        CreateWalls();
    }

    public void Refresh()
    {
        RefreshTiles();
        RefreshWalls();
    }

    private void Destroy()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        wallViews.Clear();
        wallJointViews.Clear();
        tileViews = null;
    }


    // ---------------------------------------------------------------------
    // SIZE & ORIGIN
    // ---------------------------------------------------------------------

    private void UpdateSizeFromBoard()
    {
        int widthPx = board.Width * tileSize;
        int heightPx = board.Height * tileSize;

        //SetSize(new Vector2(widthPx, heightPx));
        SetLayoutSize(new Vector2(widthPx, heightPx));
    }

    private Vector2 GetLocalWorldOrigin()
    {
        float gridWidth = board.Width * tileSize;
        float gridHeight = board.Height * tileSize;

        var rt = RectTransform;

        float x = -rt.pivot.x * gridWidth;
        float y = -rt.pivot.y * gridHeight;

        return new Vector2(x, y);
    }


    // ---------------------------------------------------------------------
    // TILES
    // ---------------------------------------------------------------------

    private void CreateTiles()
    {
        int w = board.Width;
        int h = board.Height;

        tileViews = new TileView_OLD[w, h];

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                Tile tile = board.GetTile(x, y);
                Vector2 tilePos = GetTilePosition(x, y);

                TileView_OLD tileView = Instantiate(tileViewPrefab, RectTransform);
                tileViews[x, y] = tileView;              

                tileView.Initialize(tilePos, tileSize, /*tile.IsHole ? holeColor :*/ tileColor);
            }
        }
    }

    private void RefreshTiles()
    {
        int w = board.Width;
        int h = board.Height;

        for (int row = 0; row < h; row++)
        {
            for (int col = 0; col < w; col++)
            {
                Tile tile = board.GetTile(col, row);
                tileViews[col, row].SetColor(/*tile.IsHole ? holeColor :*/ tileColor);
            }
        }
    }


    // ---------------------------------------------------------------------
    // WALLS
    // ---------------------------------------------------------------------

    private void CreateWalls()
    {
        //int w = board.Width;
        //int h = board.Height;

        //for (int y = 0; y < h; y++)
        //{
        //    for (int x = 0; x < w; x++)
        //    {
        //        Tile tile = board.GetTile(x, y);



        //        if (tile.Walls.North)
        //            CreateWall(x, y, TileEdge.North);

        //        if (tile.Walls.South)
        //            CreateWall(x, y, TileEdge.South);

        //        if (tile.Walls.East)
        //            CreateWall(x, y, TileEdge.East);

        //        if (tile.Walls.West)
        //            CreateWall(x, y, TileEdge.West);

        //        // Todo: remove duplicate joints

        //        if (board.IsWallJointVisible(x, y, TileVertex.NorthWest))
        //            CreateWallJoint(x, y, TileVertex.NorthWest);

        //        if (board.IsWallJointVisible(x, y, TileVertex.NorthEast))
        //            CreateWallJoint(x, y, TileVertex.NorthEast);

        //        if (board.IsWallJointVisible(x, y, TileVertex.SouthWest))
        //            CreateWallJoint(x, y, TileVertex.SouthWest);

        //        if (board.IsWallJointVisible(x, y, TileVertex.SouthEast))
        //            CreateWallJoint(x, y, TileVertex.SouthEast);
        //    }
        //}
    }

    private void CreateWall(int x, int y, EdgeAxis axis)
    {
        //Vector2 tilePos = GetTilePosition(x, y);
        //float halfTileSize = tileSize * 0.5f;
        //float wallLength = tileSize - wallThickness;

        //Vector2 size;
        //Vector2 position;

        //switch (edge)
        //{
        //    case TileEdge.North:
        //        size = new Vector2(wallLength, wallThickness);
        //        position = new Vector2(tilePos.x, tilePos.y + halfTileSize);
        //        break;

        //    case TileEdge.South:
        //        size = new Vector2(wallLength, wallThickness);
        //        position = new Vector2(tilePos.x, tilePos.y - halfTileSize);
        //        break;

        //    case TileEdge.West:
        //        size = new Vector2(wallThickness, wallLength);
        //        position = new Vector2(tilePos.x - halfTileSize, tilePos.y);
        //        break;

        //    case TileEdge.East:
        //        size = new Vector2(wallThickness, wallLength);
        //        position = new Vector2(tilePos.x + halfTileSize, tilePos.y);
        //        break;

        //    default: return;
        //}

        //WallView wallView = Instantiate(wallViewPrefab, RectTransform);
        //wallViews.Add(wallView);

        //wallView.Initialize(position, size, wallColor);
    }

    private void CreateWallJoint(int x, int y)
    {
        //Vector2 tilePos = GetTilePosition(x, y);
        //float halfTileSize = tileSize * 0.5f;
        //Vector2 size = new Vector2(wallThickness, wallThickness);

        //Vector2 position;

        //switch (vertex)
        //{
        //    case TileVertex.NorthWest:
        //        position = new Vector2(tilePos.x - halfTileSize, tilePos.y + halfTileSize);
        //        break;

        //    case TileVertex.NorthEast:
        //        position = new Vector2(tilePos.x + halfTileSize, tilePos.y + halfTileSize);
        //        break;

        //    case TileVertex.SouthWest:
        //        position = new Vector2(tilePos.x - halfTileSize, tilePos.y - halfTileSize);
        //        break;

        //    case TileVertex.SouthEast:
        //        position = new Vector2(tilePos.x + halfTileSize, tilePos.y - halfTileSize);
        //        break;

        //    default: return;
        //}

        //WallJointView wallJointView = Instantiate(wallJointPrefab, RectTransform);
        //wallJointViews.Add(wallJointView);

        //wallJointView.Initialize(position, size, wallJointColor);
    }

    private void RefreshWalls()
    {
        foreach (var v in wallViews) Destroy(v.gameObject);
        foreach (var j in wallJointViews) Destroy(j.gameObject);
        wallViews.Clear();
        wallJointViews.Clear();
        CreateWalls();
    }

    

    // ---------------------------------------------------------------------
    // Utils
    // ---------------------------------------------------------------------

    private Vector2 GetTilePosition(int x, int y)
    {
        Vector2 origin = GetLocalWorldOrigin();
        float halfTileSize = tileSize * 0.5f;

        float posX = origin.x + (x * tileSize) + halfTileSize;
        float posY = origin.y + (y * tileSize) + halfTileSize;

        return new Vector2(posX, posY);
    }
}
