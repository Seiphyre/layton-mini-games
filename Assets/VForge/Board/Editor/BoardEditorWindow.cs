using UnityEditor;
using UnityEngine;
using VForge.Boards;
using UnityEditorInternal;
using VForge.Boards.Definitions;
using VForge.Boards.Runtime;

public class BoardEditorWindow : EditorWindow
{
    private const float EDGE_THICKNESS = 6f;
    private const int CELL_SIZE = 40;
    private const int PADDING = 10;
    private Color CELL_COLOR = new Color(0.15f, 0.15f, 0.15f);
    private Color TILE_COLOR = new Color(0.25f, 0.35f, 0.25f);
    private Color GRID_COLOR = Color.black;
    private Color WALL_COLOR = new Color(0.55f, 0.30f, 0.10f);
    private Color PIECE_COLOR = new Color(0.2f, 0.5f, 0.8f, 0.5f);

    private BoardData _data;
    private Board _runtime;

    private Vector2 _scroll;

    private enum EditMode { Tiles, Walls, Pieces }
    private EditMode _mode = EditMode.Tiles;

    //private TileType _tilePaintType = TileType.Default;
    //private WallType _wallPaintType = WallType.Default;
    private EdgeAxis _wallAxis = EdgeAxis.Horizontal;
    private string _pieceIdInput = "Piece_01";



    [MenuItem("Tools/Board Editor")]
    public static void Open()
    {
        var wnd = GetWindow<BoardEditorWindow>();
        wnd.titleContent = new GUIContent("Board Editor");
    }

    private void OnEnable()
    {
        if (_data != null)
            _runtime = new Board(_data);
    }

    private void OnGUI()
    {
        // 1. BoardData field (top)
        EditorGUI.BeginChangeCheck();

        var newData = (BoardData)EditorGUILayout.ObjectField("Board Data", _data, typeof(BoardData), false);
        
        if (EditorGUI.EndChangeCheck())
        {
            _data = newData;
            _runtime = _data != null ? new Board(_data) : null;
            Repaint();

        }

        if (_data == null)
        {
            EditorGUILayout.HelpBox("Select a BoardData asset to edit.", MessageType.Info);
            return;
        }

        // --------------------------------------------------------------------
        // BOARD SIZE EDITOR
        // --------------------------------------------------------------------
        EditorGUI.BeginChangeCheck();

        int newWidth = Mathf.Max(1, EditorGUILayout.IntField("Board Width", _data.Width));
        int newHeight = Mathf.Max(1, EditorGUILayout.IntField("Board Height", _data.Height));

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_data, "Resize Board");

            _data.Width = newWidth;
            _data.Height = newHeight;

            _runtime.Resize(newWidth, newHeight);

            Repaint();
        }

        if (_runtime == null)
            _runtime = new Board(_data);

        // 2. Toolbar placed BELOW the BoardData field
        DrawToolbar();

        // 3. Grid
        DrawGrid();

        // 4. Save button
        DrawSaveButton();
        DrawClearButton();
    }

    private void DrawToolbar()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Board Editing Tools", EditorStyles.boldLabel);
        EditorGUILayout.Space(3);

        // Uniform button style
        GUIStyle modeButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fixedHeight = 28,
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter
        };

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Toggle(_mode == EditMode.Tiles, "Tiles", modeButtonStyle))
            _mode = EditMode.Tiles;

        if (GUILayout.Toggle(_mode == EditMode.Walls, "Walls", modeButtonStyle))
            _mode = EditMode.Walls;

        //if (GUILayout.Toggle(_mode == EditMode.Pieces, "Pieces", modeButtonStyle))
        //    _mode = EditMode.Pieces;

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(6);

        // Mode-specific fields
        switch (_mode)
        {
            case EditMode.Tiles:
                //_tilePaintType = (TileType)EditorGUILayout.EnumPopup("Tile Type:", _tilePaintType);
                break;

            case EditMode.Walls:
                //_wallPaintType = (WallType)EditorGUILayout.EnumPopup("Wall Type:", _wallPaintType);
                //_wallAxis = (EdgeAxis)EditorGUILayout.EnumPopup("Axis:", _wallAxis);
                break;

            //case EditMode.Pieces:
            //    _pieceIdInput = EditorGUILayout.TextField("Piece ID:", _pieceIdInput);
            //    break;
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    private void DrawGrid()
    {
        int w = _runtime.Width;
        int h = _runtime.Height;

        int gridWidth = w * CELL_SIZE;
        int gridHeight = h * CELL_SIZE;

        // Reserve viewport rect
        Rect viewport = GUILayoutUtility.GetRect(
            0f, 10000f,
            gridHeight + PADDING * 2,
            gridHeight + PADDING * 2,
            GUILayout.ExpandWidth(true),
            GUILayout.ExpandHeight(false)
        );

        float contentWidth = gridWidth + PADDING * 2;
        float contentHeight = gridHeight + PADDING * 2;

        float extraWidth = viewport.width - contentWidth;
        float centerOffsetX = extraWidth > 0 ? extraWidth * 0.5f : 0f;

        _scroll = GUI.BeginScrollView(
            viewport,
            _scroll,
            new Rect(0, 0, contentWidth, contentHeight)
        );

        Handles.BeginGUI();

        float baseX = PADDING + centerOffsetX;
        float baseY = PADDING;

        // Draw cells
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                Rect cell = new Rect(
                    baseX + x * CELL_SIZE,
                    baseY + (h - 1 - y) * CELL_SIZE,
                    CELL_SIZE,
                    CELL_SIZE
                );

                DrawCellVisuals(x, y, cell);

                if (Event.current.type == EventType.MouseDown && cell.Contains(Event.current.mousePosition))
                {
                    HandleClick(x, y, Event.current.mousePosition, cell);
                    Event.current.Use();
                }
            }
        }

        Handles.EndGUI();
        GUI.EndScrollView();
    }

    private void DrawSaveButton()
    {
        EditorGUILayout.Space();

        if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            Undo.RecordObject(_data, "Board Save");
            _runtime.SaveTo(_data);
            EditorUtility.SetDirty(_data);
            AssetDatabase.SaveAssets();
        }

    }

    private void DrawClearButton()
    {
        EditorGUILayout.Space(6);

        // Surrounding box (Unity style for destructive actions)
        EditorGUILayout.BeginVertical("HelpBox");
        EditorGUILayout.LabelField("Danger Zone", EditorStyles.boldLabel);

        GUIStyle dangerButton = new GUIStyle(GUI.skin.button);
        dangerButton.normal.textColor = new Color(0.85f, 0.15f, 0.15f);
        dangerButton.fontStyle = FontStyle.Bold;
        dangerButton.fixedHeight = 28;

        GUI.backgroundColor = new Color(0.9f, 0.5f, 0.5f, 0.25f); // subtle red tint

        if (GUILayout.Button("Clear Entire Board", dangerButton))
        {
            if (EditorUtility.DisplayDialog(
                "Clear Board",
                "This will remove ALL tiles, walls and pieces.\n\nAre you sure?",
                "Clear", "Cancel"))
            {
                Undo.RecordObject(_data, "Board Clear");

                _data.Tiles.Clear();
                _data.Walls.Clear();
                _data.Pieces.Clear();

                _runtime = new Board(_data);

                EditorUtility.SetDirty(_data);
                Repaint();
            }
        }

        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndVertical();
    }


    // -----------------------------------------------
    // Drawing Logic
    // -----------------------------------------------

    private void DrawCellVisuals(int x, int y, Rect cell)
    {
        // Background grid
        EditorGUI.DrawRect(cell, CELL_COLOR);

        // Tile
        var tile = _runtime.GetTile(x, y);
        if (tile != null)
            EditorGUI.DrawRect(cell, TILE_COLOR);

        // Grid border
        Handles.color = GRID_COLOR;
        Handles.DrawLine(new Vector2(cell.x, cell.y), new Vector2(cell.x + CELL_SIZE, cell.y));
        Handles.DrawLine(new Vector2(cell.x, cell.y), new Vector2(cell.x, cell.y + CELL_SIZE));
        Handles.DrawLine(new Vector2(cell.x + CELL_SIZE, cell.y), new Vector2(cell.x + CELL_SIZE, cell.y + CELL_SIZE));
        Handles.DrawLine(new Vector2(cell.x, cell.y + CELL_SIZE), new Vector2(cell.x + CELL_SIZE, cell.y + CELL_SIZE));

        // Wall

        // Horizontal wall → bottom edge of cell
        var bottomWall = _runtime.GetHorizontalWall(x, y);
        if (bottomWall != null)
        {
            float midY = cell.yMax; // bottom edge
            Rect hRect = new Rect(
                cell.x,
                midY - EDGE_THICKNESS * 0.5f,
                cell.width,
                EDGE_THICKNESS
            );
            EditorGUI.DrawRect(hRect, WALL_COLOR);
        }

        if (y == _runtime.Height - 1)
        {
            var topWall = _runtime.GetHorizontalWall(x, y + 1);
            if (topWall != null)
            {
                float midY = cell.yMin; // top of cell
                Rect hRect = new Rect(
                    cell.x,
                    midY - EDGE_THICKNESS * 0.5f,
                    cell.width,
                    EDGE_THICKNESS
                );
                EditorGUI.DrawRect(hRect, WALL_COLOR);
            }
        }

        // Vertical wall → left edge of cell
        var leftWall = _runtime.GetVerticalWall(x, y);
        if (leftWall != null)
        {
            float midX = cell.xMin;
            Rect vRect = new Rect(
                midX - EDGE_THICKNESS * 0.5f,
                cell.y,
                EDGE_THICKNESS,
                cell.height
            );
            EditorGUI.DrawRect(vRect, WALL_COLOR);
        }

        if (x == _runtime.Width - 1)
        {
            var rightWall = _runtime.GetVerticalWall(x + 1, y);
            if (rightWall != null)
            {
                float midX = cell.xMax; // right of cell
                Rect vRect = new Rect(
                    midX - EDGE_THICKNESS * 0.5f,
                    cell.y,
                    EDGE_THICKNESS,
                    cell.height
                );
                EditorGUI.DrawRect(vRect, WALL_COLOR);
            }
        }

        // Piece
        var piece = _runtime.GetPiece(x, y);
        if (piece != null)
        {
            EditorGUI.DrawRect(cell, PIECE_COLOR);
            //GUI.Label(cell, piece.PieceId.Substring(0, Mathf.Min(3, piece.PieceId.Length)));
        }
    }

    // -----------------------------------------------
    // Click Handling
    // -----------------------------------------------

    private void HandleClick(int x, int y, Vector2 mousePos, Rect cell)
    {
        if (_mode == EditMode.Tiles)
        {
            ToggleTile(x, y);
            Repaint();
            return;
        }

        if (_mode == EditMode.Walls)
        {
            float left = cell.xMin;
            float right = cell.xMax;
            float top = cell.yMin;
            float bottom = cell.yMax;

            float mx = mousePos.x;
            float my = mousePos.y;

            // LEFT edge → vertical wall at (x, y)
            if (mx >= left && mx <= left + EDGE_THICKNESS)
            {
                ToggleWall(EdgeAxis.Vertical, x, y);
                Repaint();
                return;
            }

            // RIGHT edge → vertical wall at (x+1, y)
            if (mx >= right - EDGE_THICKNESS && mx <= right)
            {
                ToggleWall(EdgeAxis.Vertical, x + 1, y);
                Repaint();
                return;
            }

            // BOTTOM edge → horizontal wall at (x, y)
            if (my >= bottom - EDGE_THICKNESS && my <= bottom)
            {
                ToggleWall(EdgeAxis.Horizontal, x, y);
                Repaint();
                return;
            }

            // TOP edge → horizontal wall at (x, y+1)
            if (my >= top && my <= top + EDGE_THICKNESS)
            {

                ToggleWall(EdgeAxis.Horizontal, x, y + 1);
                Repaint();
                return;
            }
        }

        if (_mode == EditMode.Pieces)
        {
            TogglePiece(x, y);
            Repaint();
            return;
        }
    }

    // -----------------------------------------------
    // Runtime accessors
    // -----------------------------------------------

    private void ToggleTile(int x, int y)
    {
        var existing = _runtime.GetTile(x, y);
        if (existing != null)
            _runtime.TryRemoveTile(x, y);
        else
            _runtime.TryAddTile(x, y);
    }

    private void ToggleWall(EdgeAxis axis, int x, int y)
    {
        var existing = _runtime.GetWall(x, y, axis);
        if (existing != null)
            _runtime.TryRemoveWall(axis, x, y);
        else
            _runtime.TryAddWall(axis, x, y);
    }

    private void TogglePiece(int x, int y)
    {
        var existing = _runtime.GetPiece(x, y);
        if (existing != null)
            _runtime.TryRemovePiece(x, y);
        else
            _runtime.TryAddPiece(x, y);
    }
}
