using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoardData))]
public class BoardDataEditor : Editor
{
    private static GUIStyle _tileButtonStyle;
    private static GUIStyle TileButtonStyle
    {
        get
        {
            if (_tileButtonStyle == null)
            {
                _tileButtonStyle = new GUIStyle(GUI.skin.button);
                //_tileButtonStyle.normal.background = Texture2D.whiteTexture;
                //_tileButtonStyle.active.background = Texture2D.whiteTexture;
                //_tileButtonStyle.hover.background = Texture2D.whiteTexture;
                //_tileButtonStyle.border = new RectOffset(0,0,0,0);
                //_tileButtonStyle.margin = new RectOffset(0,0,0,0);
                //_tileButtonStyle.padding = new RectOffset(0,0,0,0);
            }

            return _tileButtonStyle;
        }
    }

    private static GUIStyle _emptyTileStyle = null;
    private static GUIStyle EmptyTileStyle
    {
        get
        {
            if (_emptyTileStyle == null)
            {
                _emptyTileStyle = new GUIStyle(GUI.skin.button);

                Texture2D normalTex = MakeColorTex(Color.red);
                Texture2D hoverTex = MakeColorTex(Color.red);
                Texture2D activeTex = MakeColorTex(Color.red);

                _emptyTileStyle.normal.background = normalTex;
                _emptyTileStyle.active.background = normalTex;
                _emptyTileStyle.hover.background = hoverTex;
                _emptyTileStyle.focused.background = activeTex;

                _emptyTileStyle.onNormal.background = normalTex;
                _emptyTileStyle.onActive.background = normalTex;
                _emptyTileStyle.onHover.background = hoverTex;
                _emptyTileStyle.onFocused.background = activeTex;

                _emptyTileStyle.border = new RectOffset(0, 0, 0, 0);
                _emptyTileStyle.padding = new RectOffset(0, 0, 0, 0);

                _emptyTileStyle.contentOffset = Vector2.zero;
            }

            return _emptyTileStyle;
        }
    }

    private static GUIStyle _tileStyle;
    private static GUIStyle TileStyle
    {
        get
        {
            if (_tileStyle == null)
            {
                _tileStyle = new GUIStyle(GUI.skin.button);

                Texture2D normalTex = MakeColorTex(Color.green);
                Texture2D hoverTex = MakeColorTex(Color.green);
                Texture2D activeTex = MakeColorTex(Color.green);

                _tileStyle.normal.background = normalTex;
                _tileStyle.active.background = normalTex;
                _tileStyle.hover.background = hoverTex;
                _tileStyle.focused.background = activeTex;

                _tileStyle.onNormal.background = normalTex;
                _tileStyle.onActive.background = normalTex;
                _tileStyle.onHover.background = hoverTex;
                _tileStyle.onFocused.background = activeTex;

                _tileStyle.border = new RectOffset(0, 0, 0, 0);
                _tileStyle.padding = new RectOffset(0, 0, 0, 0);

                _tileStyle.contentOffset = Vector2.zero;
            }

            return _tileStyle;

        }
    }

    private TileType paintType = TileType.Wood;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BoardData board = (BoardData)target;

        // --

        //EditorGUILayout.LabelField("Width", EditorStyles.boldLabel);
        ////board.Size = EditorGUILayout.Vector2IntField("Board Size", board.Size);
        //board.BoardGridSize.x = EditorGUILayout.IntSlider(board.BoardGridSize.x, 2, 10);

        //EditorGUILayout.LabelField("Height", EditorStyles.boldLabel);
        ////board.Size = EditorGUILayout.Vector2IntField("Board Size", board.Size);
        //board.BoardGridSize.y = EditorGUILayout.IntSlider(board.BoardGridSize.y, 2, 10);

        //EditorGUILayout.Space(10);

        //// --

        //paintType = (TileType)EditorGUILayout.EnumPopup("Brush", paintType);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tiles Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        int colCount = board.GridColCount;
        int rowCount = board.GridRowCount;
        int tileSize = 16;
        int wallSize = 8;
        int gapSize = 2;

        for (int y = 0; y < rowCount; y++)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));

            for (int x = 0; x < colCount; x++)
            {

                EditorGUILayout.BeginVertical(
                    GUILayout.Width(wallSize + gapSize + tileSize), 
                    GUILayout.Height(wallSize + gapSize + tileSize + ((y == rowCount - 1) ? gapSize + wallSize : 0)), 
                    GUILayout.ExpandWidth(false), 
                    GUILayout.ExpandHeight(false));


                // -- TOP

                EditorGUILayout.BeginHorizontal(
                    GUILayout.Width(wallSize + gapSize + tileSize + ((x == colCount - 1) ? gapSize + wallSize : 0)), 
                    GUILayout.Height(wallSize), 
                    GUILayout.ExpandWidth(false),
                    GUILayout.ExpandHeight(false));

                DrawCornerGUI(board, x, y, wallSize, wallSize);

                GUILayout.Space(gapSize);

                DrawWallGUI(board, x, y, tileSize, wallSize, WallDirection.North);

                if (x == colCount - 1)
                {
                    GUILayout.Space(gapSize);

                    DrawCornerGUI(board, x, y, wallSize, wallSize);
                }

                EditorGUILayout.EndHorizontal();

                // --

                GUILayout.Space(gapSize);

                // -- Middle

                GUILayout.BeginHorizontal(
                    GUILayout.Width(wallSize + gapSize + tileSize + ((x == colCount - 1) ? gapSize + wallSize : 0)),
                    GUILayout.Height(tileSize), 
                    GUILayout.ExpandWidth(false), 
                    GUILayout.ExpandHeight(false));

                DrawWallGUI(board, x, y, wallSize, tileSize, WallDirection.West);

                GUILayout.Space(gapSize);

                DrawTileGUI(board, x, y, tileSize, tileSize);

                if (x == colCount - 1)
                {
                    GUILayout.Space(gapSize);

                    DrawWallGUI(board, x, y, wallSize, tileSize, WallDirection.East);
                }

                GUILayout.EndHorizontal();

                // -- Bottom

                if (y == rowCount - 1)
                {
                    GUILayout.Space(gapSize);

                    EditorGUILayout.BeginHorizontal(
                        GUILayout.Width(wallSize + gapSize + tileSize + ((x == colCount - 1) ? gapSize + wallSize : 0)),
                        GUILayout.Height(wallSize),
                        GUILayout.ExpandWidth(false),
                        GUILayout.ExpandHeight(false));

                    DrawCornerGUI(board, x, y, wallSize, wallSize);

                    GUILayout.Space(gapSize);

                    DrawWallGUI(board, x, y, tileSize, wallSize, WallDirection.South);

                    if (x == colCount - 1)
                    {
                        GUILayout.Space(gapSize);

                        DrawCornerGUI(board, x, y, wallSize, wallSize);
                    }

                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(gapSize);
                }

                // --

                EditorGUILayout.EndVertical();
                GUILayout.Space(gapSize);
            }

            GUILayout.EndHorizontal();

            if (y != rowCount)
                GUILayout.Space(gapSize);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(board);
        }
    }

    private void DrawTileGUI(BoardData board, int x, int y, int width, int height)
    {
        Tile tile = board.GetTile(x, y);
        Tile topTile = board.GetTile(x, y - 1);
        Tile leftTile = board.GetTile(x - 1, y);

        Rect tileRect = GUILayoutUtility.GetRect(width, height, GUILayout.Width(width), GUILayout.Height(height), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));

        if (tile.IsEmpty)
        {
            EditorGUI.DrawRect(tileRect, EmptyTileColor);

            if (GUI.Button(tileRect, GUIContent.none, GUIStyle.none))
            {
                Tile newTile = new Tile(TileType.Wood);

                if (topTile != null && !topTile.IsEmpty)
                {
                    if (topTile.South)
                    {
                        topTile.South = false;
                        newTile.North = true;
                    }
                }

                if (leftTile != null && !leftTile.IsEmpty)
                {
                    if (leftTile.East)
                    {
                        leftTile.East = false;
                        newTile.West = true;
                    }
                }

                board.SetTile(x, y, newTile);
            }
        }
        else
        {
            EditorGUI.DrawRect(tileRect, NormalTileColor);

            if (GUI.Button(tileRect, GUIContent.none, GUIStyle.none))
            {
                Tile newTile = new Tile();

                if (topTile != null && !topTile.IsEmpty)
                {
                    if (tile.North)
                    {
                        topTile.South = true;
                    }
                }

                if (leftTile != null && !leftTile.IsEmpty)
                {
                    if (tile.West)
                    {
                        leftTile.East = true;
                    }
                }

                board.SetTile(x, y, newTile);
            }
        }
    }

    private void DrawWallGUI(BoardData board, int x, int y, int width, int height, WallDirection dir)
    {
        Tile tile = board.GetTile(x, y);
        Tile topTile = board.GetTile(x, y - 1);
        Tile leftTile = board.GetTile(x - 1, y);

        if (tile != null && tile.IsEmpty && dir == WallDirection.North && topTile != null && !topTile.IsEmpty)
        {
            dir = WallDirection.South;
            tile = topTile;
        }

        if (tile != null && tile.IsEmpty && dir == WallDirection.West && leftTile != null && !leftTile.IsEmpty)
        {
            dir = WallDirection.East;
            tile = leftTile;
        }

        if (!tile.IsEmpty)
        {
            bool wall = tile.GetWall(dir);

            // --

            Rect tileRect = GUILayoutUtility.GetRect(width, height, GUILayout.Width(width), GUILayout.Height(height), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));

            EditorGUI.DrawRect(tileRect, wall ? NormalWallColor : EmptyWallColor);

            if (GUI.Button(tileRect, GUIContent.none, GUIStyle.none))
            {
                tile.SetWall(dir, !wall);
            }
        }
        else
        {
            GUILayout.Space(width);
        }
    }

    private void DrawCornerGUI(BoardData board, int x, int y, int width, int height)
    {
        GUILayout.Space(width);

        //TileData tile = board.GetTile(x, y);

        //if (!tile.IsEmpty)
        //{
        //    Vector2Int boardSize = board.Size;


        //    // -- Draw North Wall

        //    Rect tileRect = GUILayoutUtility.GetRect(tileSize, tileSize, GUILayout.Width(tileSize), GUILayout.Height(32));
        //}
    }

    private static Texture2D MakeColorTex(Color color)
    {
        var tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, color);
        tex.Apply();

        return tex;
    }

    private Color EmptyWallColor => EmptyTileColor;
    private Color EmptyTileColor => new Color32(255 / 2, 255 / 2, 255 / 2, 255 / 8);
    private Color NormalWallColor => new Color32(0, 0, 0, 255);
    private Color NormalTileColor => new Color32(255, 255, 255, 255);

    //void DrawTileGUI(BoardData data, int x, int y, int tileSize)
    //{
    //    TileType tile = data.GetTile(x, y);
    //    WallData wall = data.GetWall(x, y);

    //    Rect rect = GUILayoutUtility.GetRect(tileSize, tileSize, GUILayout.Width(tileSize), GUILayout.Width(tileSize), GUILayout.ExpandWidth(false));
    //    Color oldColor = GUI.color;
    //    GUI.color = GetColor(tile);

    //    // --

    //    //TileButtonStyle.normal.background = Texture2D.whiteTexture;
    //    //EditorGUI.DrawRect(rect, GetColor(tile));

    //    if (GUI.Button(rect, GUIContent.none, TileButtonStyle))
    //    {
    //        Undo.RecordObject(data, "Paint Tile");
    //        data.SetTile(x, y, paintType);
    //    }

    //    GUI.color = oldColor;

    //    // --

    //    float w = tileSize * 0.3f;
    //    float h = w;

    //    GUIStyle btn = new GUIStyle(GUI.skin.button);
    //    btn.fontSize = 8;

    //    // --

    //    //Rect north = new Rect(rect.x + tileSize/2 - w/2, rect.y + 2, w, h);
    //    //if (GUI.Button(north, wall.North ? "N" : "", btn))
    //    //{
    //    //    Undo.RecordObject(data, "Toggle Wall");
    //    //    data.ToggleWall(x, y, WallDirection.North);
    //    //}

    //    //Rect south = new Rect(rect.x + tileSize / 2 - w / 2, rect.y + tileSize - h - 2, w, h);
    //    //if (GUI.Button(south, wall.South ? "-" : "", btn))
    //    //{
    //    //    Undo.RecordObject(data, "Toggle Wall");
    //    //    data.ToggleWall(x, y, WallDirection.South);
    //    //}

    //    //Rect west = new Rect(rect.x + 2, rect.y + tileSize/2 - h/2, w, h);
    //    //if (GUI.Button(west, wall.West ? "|" : "", btn))
    //    //{
    //    //    Undo.RecordObject(data, "Toggle Wall");
    //    //    data.ToggleWall(x, y, WallDirection.West);
    //    //}

    //    //Rect east = new Rect(rect.x + tileSize - w - 2, rect.y + tileSize / 2 - h / 2, w, h);
    //    //if (GUI.Button(east, wall.East ? "|" : "", btn))
    //    //{
    //    //    Undo.RecordObject(data, "Toggle Wall");
    //    //    data.ToggleWall(x, y, WallDirection.East);
    //    //}
    //}

    //private void DrawWall(BoardData data, int x, int y, int tileSize, bool isHorizontal)
    //{
    //    int w = isHorizontal ? tileSize : tileSize / 4;
    //    int h = isHorizontal ? tileSize / 4 : tileSize;

    //    Rect rect = GUILayoutUtility.GetRect(w, h, GUILayout.Width(w), GUILayout.Height(h), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));

    //    GUI.Button(rect, GUIContent.none);

    //    //EditorGUI.DrawRect(rect, Color.green);
    //}

    //private void DrawInterserction(BoardData data, int x, int y, int tileSize)
    //{
    //    int w = tileSize / 4;
    //    int h = tileSize / 4;

    //    Rect rect = GUILayoutUtility.GetRect(w, h, GUILayout.Width(w), GUILayout.Height(h), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));

    //    EditorGUI.DrawRect(rect, new Color32(0,0,0,0));
    //}

    //private Color GetColor(TileType type)
    //{
    //    switch (type)
    //    {
    //        case TileType.Empty: return new Color(0.2f, 0.2f, 0.2f);
    //        case TileType.Wood: return new Color(1f, 1f, 1f);
    //    }

    //    return Color.magenta;
    //}
}
