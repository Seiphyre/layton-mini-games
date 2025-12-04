using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// - Visual representation(instantiates Tile views, maybe Wall visuals later).
/// - Talks to Board via a reference or a BoardController.
/// - Does not contenir la logique de validation ou de game rules.
///
/// </summary>

public class BoardView : MonoBehaviour
{
    [Header("Board"), Space]

    [SerializeField] private BoardData m_BoardData;

    [SerializeField] private Vector2 m_BoardPivot = new Vector2(0, 1);
    [SerializeField] private int m_RowDir = -1; // 1: bottom-to-top, -1: top-to-bottom
    [SerializeField] private int m_ColDir = 1; // 1: left-to-right, -1: right-to-left

    [Header("Grid"), Space]

    [SerializeField] private int m_GridLineThickness = 2;
    [SerializeField] private Color m_GridLineColor = Color.black;
    [SerializeField] private VisualTile m_GridTemplate;

    [SerializeField, HideInInspector] private GameObject m_GridLayer;

    public int GridColCount => m_BoardData.GridColCount;
    public int GridRowCount => m_BoardData.GridRowCount;
    public Vector2Int GridSize => new Vector2Int(GridColCount, GridRowCount);



    [Header("Tiles"), Space]

    [SerializeField] private int m_TileSize = 64;
    [SerializeField] private VisualTile m_TileTemplate;

    public int TileSize => m_TileSize;

    [SerializeField, HideInInspector] private GameObject m_TilesLayer;



    [Header("Walls"), Space]

    [SerializeField] private int m_WallThickness = 8;
    [SerializeField] private Color m_WallColor = Color.blue;
    [SerializeField] private VisualTile m_WallsTemplate;

    [SerializeField, HideInInspector] private GameObject m_WallsLayer;


    [Header("Pieces"), Space]

    [SerializeField] private VisualBoardElement m_PieceTemplate;
    [SerializeField] private VisualBoardElement m_DropPreviewTemplate;

    [SerializeField, HideInInspector] private GameObject m_PiecesLayer;



    // -- [ Dropzone ]

    private DropZone _dropZone;

    public DropZone DropZone
    {
        get
        {
            if (_dropZone == null)
                _dropZone = GetComponent<DropZone>();

            return _dropZone;
        }
    }

    public bool HasDropZone
    {
        get
        {
            return _dropZone != null;
        }
    }

    private VisualBoardElement _dropPreview;



    // -----------------------------------------------------

    private void OnEnable()
    {
        if (DropZone != null)
        {
            DropZone.onEnter.AddListener(DropZone_DraggableEnter);
            DropZone.onExit.AddListener(DropZone_DraggableExit);
            DropZone.onMove.AddListener(DropZone_DraggableMove);
            DropZone.onDropped.AddListener(DropZone_Dropped);
        }
    }

    public void OnDisable()
    {
        if (DropZone != null)
        {
            DropZone.onEnter.RemoveListener(DropZone_DraggableEnter);
            DropZone.onExit.RemoveListener(DropZone_DraggableExit);
            DropZone.onMove.RemoveListener(DropZone_DraggableMove);
            DropZone.onDropped.RemoveListener(DropZone_Dropped);
        }
    }


    // ------------------------------------------------------

    public GameObject CreateBoard()
    {
        GameObject grid = null;

        if (m_BoardData != null)
        {
            DestroyBoard();

            // --

            LayoutGroup layout = this.GetComponent<LayoutGroup>();

            if (layout != null)
            {
                int padding = Mathf.Max(m_WallThickness, m_GridLineThickness) / 2;

                layout.padding = new RectOffset(padding, padding, padding, padding);
            }

            // -- Create GameObject

            grid = new GameObject("Content", typeof(RectTransform), typeof(LayoutElement));

            // -- Init Components

            RectTransform rectTransform = grid.GetComponent<RectTransform>();

            rectTransform.pivot = m_BoardPivot;
            rectTransform.SetParent(this.transform);

            // --

            LayoutElement layoutElement = grid.GetComponent<LayoutElement>();

            int width = GridSize.x * TileSize;
            int height = GridSize.y * TileSize;

            layoutElement.ignoreLayout = false;
            layoutElement.minWidth = width;
            layoutElement.minHeight = height;
            layoutElement.preferredWidth = width;
            layoutElement.preferredHeight = height;
            layoutElement.flexibleWidth = 0;
            layoutElement.flexibleHeight = 0;

            // -- Create Children

            m_TilesLayer = CreateLayer("Tiles", m_TileTemplate, grid.transform);
            m_GridLayer = CreateLayer("Grid", m_GridTemplate, grid.transform);
            m_PiecesLayer = CreateLayer("Pieces", null, grid.transform);
            m_WallsLayer = CreateLayer("Walls", m_WallsTemplate, grid.transform);
        }

        return grid;
    }

    private GameObject CreateLayer(string layerName, VisualTile cellTemplate, Transform parent)
    {
        // -- Create GameObject

        GameObject layer = new GameObject(layerName, typeof(RectTransform));

        // -- Init Components

        if (parent != null)
            layer.transform.SetParent(parent);

        RectTransform rectTransform = layer.GetComponent<RectTransform>();

        rectTransform.pivot = m_BoardPivot;
        rectTransform.anchorMin = Vector2.zero;  // bottom-left
        rectTransform.anchorMax = Vector2.one;   // top-right
        rectTransform.offsetMin = Vector2.zero;  // no margin
        rectTransform.offsetMax = Vector2.zero;  // no margin

        if (cellTemplate != null)
        {
            VerticalLayoutGroup layout = layer.AddComponent<VerticalLayoutGroup>();

            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.reverseArrangement = (m_RowDir < 0) ? false : true;

            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childScaleWidth = false;
            layout.childScaleHeight = false;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
        }

        // -- Create Children

        if (cellTemplate != null)
        {
            for (int rowIndex = 0; rowIndex < m_BoardData.GridRowCount; rowIndex++)
            {
                GameObject row = CreateRow($"Row {rowIndex}", cellTemplate, layer.transform, rowIndex);
            }
        }

        return layer;
    }

    private GameObject CreateRow(string rowName, VisualTile cellTemplate, Transform parent, int rowIndex)
    {
        // -- Create Game Object

        GameObject row = new GameObject(rowName, typeof(RectTransform), typeof(HorizontalLayoutGroup));

        // -- Init Components

        if (parent != null)
            row.transform.SetParent(parent);

        RectTransform rectTransform = row.GetComponent<RectTransform>();

        rectTransform.anchorMin = Vector2.zero;  // bottom-left
        rectTransform.anchorMax = Vector2.one;   // top-right
        rectTransform.offsetMin = Vector2.zero;  // no margin
        rectTransform.offsetMax = Vector2.zero;  // no margin

        // --

        HorizontalLayoutGroup rowLayout = row.GetComponent<HorizontalLayoutGroup>();

        rowLayout.padding = new RectOffset(0, 0, 0, 0);
        rowLayout.childAlignment = TextAnchor.UpperLeft;
        rowLayout.reverseArrangement = false;

        rowLayout.childControlWidth = true;
        rowLayout.childControlHeight = true;
        rowLayout.childScaleWidth = false;
        rowLayout.childScaleHeight = false;
        rowLayout.childForceExpandWidth = false;
        rowLayout.childForceExpandHeight = false;

        // -- Create Children

        for (int colIndex = 0; colIndex < m_BoardData.GridColCount; colIndex++)
        {
            GameObject cell = CreateCell($"Tile {colIndex}:{rowIndex}",cellTemplate, row.transform, colIndex, rowIndex);
        }

        return row;
    }

    private GameObject CreateCell(string cellName, VisualTile cellTemplate, Transform parent, int colIndex, int rowIndex)
    {
        // -- Create GameObject

        VisualTile tile = Instantiate(cellTemplate, parent);

        // -- Init Components

        tile.name = cellName;

        // --

        LayoutElement tileLayoutElement = tile.GetComponent<LayoutElement>();

        if (tileLayoutElement == null)
            tileLayoutElement = tile.AddComponent<LayoutElement>();

        tileLayoutElement.ignoreLayout = false;
        tileLayoutElement.minWidth = TileSize;
        tileLayoutElement.minHeight = TileSize;
        tileLayoutElement.preferredWidth = TileSize;
        tileLayoutElement.preferredHeight = TileSize;
        tileLayoutElement.flexibleWidth = 0;
        tileLayoutElement.flexibleHeight = 0;

        // --

        //tile.Value = m_BoardData.GetTile(colIndex, rowIndex);

        tile.BorderColor = m_GridLineColor;
        tile.BorderThickness = m_GridLineThickness;
        tile.WallsColor = m_WallColor;
        tile.WallsThickness = m_WallThickness;

        tile.Refresh();

        // --

        return tile.gameObject;
    }

    // --

    public void DestroyBoard()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child == transform) continue;

            if (child != null)
                DestroyImmediate(child.gameObject);
        }

        //LayoutGroup oldLayout = GetComponent<LayoutGroup>();

        //if (oldLayout != null)
        //    DestroyImmediate(oldLayout);
    }



    // -------------------------------------------------------

    private void DropZone_Dropped(Draggable draggedGameobject)
    {
        if (draggedGameobject != null)
        {
            VisualElement visualElement = draggedGameobject.GetComponent<VisualElement>();

            if (visualElement != null)
            {
                //if (visualElement.Value != null && visualElement.Value is ShopItem shopItem)
                //{
                //    VisualBoardElement piece = Instantiate(m_PieceTemplate, m_PiecesLayer.transform);

                //    piece.Board = this;
                //    piece.Value = shopItem;

                //    MovePiece(piece, ScreenPointToBoardPosition(GetPositionAtPivot(_dropPreview.transform as RectTransform, m_BoardPivot)));

                //    //_pieces.Add(piece);
                //}
            }
        }
    }

    private void DropZone_DraggableMove(Draggable draggedGameobject)
    {

        if (draggedGameobject != null)
        {
            VisualElement visualElement = draggedGameobject.GetComponentInChildren<VisualBoardElement>();

            if (visualElement != null)
            {
                //if (visualElement.Value != null && visualElement.Value is ShopItem shopItem)
                //{
                //    MoveDropPreview(GetPositionAtPivot(visualElement.transform as RectTransform, m_BoardPivot));
                //}
            }
        }
    }

    private void DropZone_DraggableExit(Draggable draggedGameobject)
    {
        if (draggedGameobject != null)
        {
            VisualElement visualElement = draggedGameobject.GetComponentInChildren<VisualBoardElement>();

            if (visualElement != null)
            {
                //if (visualElement.Value != null)
                //{
                //    if (_dropPreview != null && _dropPreview.Value == visualElement.Value)
                //        DestroyDropPreview();
                //}
            }
        }
    }

    private void DropZone_DraggableEnter(Draggable draggedGameobject)
    {
        if (draggedGameobject != null)
        {
            VisualElement visualElement = draggedGameobject.GetComponentInChildren<VisualBoardElement>();

            if (visualElement != null)
            {
                //if (visualElement.Value != null && visualElement.Value is ShopItem shopItem)
                //{
                //    CreateDropPreview(shopItem);
                //    MoveDropPreview(GetPositionAtPivot(visualElement.transform as RectTransform, m_BoardPivot));
                //}
            }
        }
    }

    // --

    private void CreateDropPreview(PieceData piece)
    {
        _dropPreview = Instantiate(m_DropPreviewTemplate, m_PiecesLayer.transform);

        _dropPreview.Board = this;
        //_dropPreview.Value = piece;
        _dropPreview.Size = new Vector2Int(piece.Shape.Width, piece.Shape.Height);

        var canvasGroup = _dropPreview.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
    }

    private void MoveDropPreview(Vector2 position)
    {
        if (_dropPreview != null)
        {
            MovePiece(_dropPreview, ScreenPointToBoardPosition(position));
        }
    }

    private void DestroyDropPreview()
    {
        Destroy(_dropPreview.gameObject);
        _dropPreview = null;
    }

    // --

    private Vector2Int ScreenPointToBoardPosition(Vector2 screenPoint)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_PiecesLayer.transform as RectTransform,
            screenPoint,
            null,
            out Vector2 boardPosition);

        boardPosition = boardPosition / m_TileSize;

        return new Vector2Int(Mathf.RoundToInt(boardPosition.x) * m_ColDir, Mathf.RoundToInt(boardPosition.y) * m_RowDir);
    }

    public void MovePiece(VisualBoardElement piece, Vector2Int gridPosition)
    {
        //gridPosition.Clamp(Vector2Int.zero, GridSize - piece.Size);

        piece.RectTransform.anchorMin = m_BoardPivot;
        piece.RectTransform.anchorMax = m_BoardPivot;
        piece.RectTransform.pivot = m_BoardPivot;

        piece.transform.localPosition = (Vector2)gridPosition * m_TileSize * new Vector2(m_ColDir, m_RowDir);

        piece.Position = gridPosition;

        piece.gameObject.SetActive(CanPlacePiece(piece));
    }

    public VisualBoardElement CreatePiece(PieceData shopItem)
    {
        VisualBoardElement piece = Instantiate(m_PieceTemplate, m_PiecesLayer.transform);

        piece.Board = this;
        //piece.Value = shopItem;
        piece.Size = new Vector2Int(shopItem.Shape.Width, shopItem.Shape.Height);

        return piece;
    }

    public bool CanPlacePiece(VisualBoardElement piece)
    {
        return !CollideWithEmptyTile(piece)
            && !CollideWithWall(piece)
            && !CollideWithOtherPieces(piece)
            && !IsOutOfBound(piece);
    }

    private bool IsOutOfBound(VisualBoardElement piece)
    {
        return piece.Position.x < 0
            || piece.Position.y < 0
            || piece.Position.x + piece.Size.x > m_BoardData.GridColCount
            || piece.Position.y + piece.Size.y > m_BoardData.GridRowCount;
    }

    private bool CollideWithEmptyTile(VisualBoardElement piece)
    {
        for (int y = piece.Position.y; y < piece.Position.y + piece.Size.y; y++)
        {
            for (int x = piece.Position.x; x < piece.Position.x + piece.Size.x; x++)
            {
                Tile tile = m_BoardData.GetTile(x, y);

                if (tile != null && tile.IsEmpty)
                    return true;
            }
        }

        return false;
    }

    private bool CollideWithWall(VisualBoardElement piece)
    {
        Vector2Int min = piece.Position;
        Vector2Int max = min + piece.Size - Vector2Int.one;

        for (int y = min.y; y <= max.y; y++)
        {
            for (int x = min.x; x <= max.x; x++)
            {
                Tile tile = m_BoardData.GetTile(x, y);

                if (tile != null)
                {
                    if (tile.North && y != min.y) // Collision with north wall
                        return true;

                    if (tile.South && y != max.y) // Collision with south wall
                        return true;

                    if (tile.West && x != min.x) // Collision with west wall
                        return true;

                    if (tile.East && x != max.x) // Collision with east wall
                        return true;
                }
            }
        }

        return false;
    }

    private bool CollideWithOtherPieces(VisualBoardElement piece)
    {
        return false;
    }

    private bool CollideWith(VisualBoardElement piece, VisualBoardElement other)
    {
        for (int y = piece.Position.y; y < piece.Position.y + piece.Size.y; y++)
        {
            for (int x = piece.Position.x; x < piece.Position.x + piece.Size.x; x++)
            {
                Tile tile = m_BoardData.GetTile(x, y);

                if (tile != null && tile.IsEmpty)
                    return true;
            }
        }

        return false;
    }

    public static Vector3 GetPositionAtPivot(RectTransform rt, Vector2 pivot)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        return new Vector3(
            Mathf.Lerp(bottomLeft.x, topRight.x, pivot.x),
            Mathf.Lerp(bottomLeft.y, topRight.y, pivot.y),
            rt.position.z
        );
    }
}
