using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using VForge.Boards.Definitions;

namespace VForge.Boards.Views
{
    public class BoardView : UIElement, IBoardViewContext
    {
        [Header("Data")]
        [SerializeField] private BoardData boardData;

        [Header("Board Settings")]
        [SerializeField] private float cellSize = 40f;
        [SerializeField] private Color backgroundColor = new Color(0.5f, 0.5f, 0.5f);

        [Header("Tiles Settings")]
        [SerializeField] private TileView tilePrefab;

        [SerializeField] private int tilesLayerOrder = 0;
        [SerializeField] private Color tileColor = new Color(0.25f, 0.35f, 0.25f);

        [Header("Walls Settings")]
        [SerializeField] private WallView wallHorizontalPrefab;
        [SerializeField] private WallView wallVerticalPrefab;
        [SerializeField] private JointView jointPrefab;

        [SerializeField] private int wallsHLayerOrder = 3;
        [SerializeField] private int wallsVLayerOrder = 4;
        [SerializeField] private int jointsLayerOrder = 5;

        [SerializeField] private float wallThickness = 8f;
        [SerializeField] private Color wallColor = new Color(0.55f, 0.30f, 0.10f);
        [SerializeField] private Color jointColor = new Color(0.55f, 0.30f, 0.10f);

        [Header("Grid Settings")]
        [SerializeField] private GridView gridPrefab;
        [SerializeField] private int gridLayerOrder = 1;
        [SerializeField] private float gridThickness = 1f;
        [SerializeField] private Color gridColor = new Color(0, 0, 0, 0.75f);

        [Header("Pieces Settings")]
        [SerializeField] private int pieceLayerOrder = 2;

        // --- Layers ---
        private readonly Dictionary<BoardViewLayer, RectTransform> layers = new();

        private RectTransform layerBackground;
        private RectTransform layerTiles;
        private RectTransform layerGrid;
        private RectTransform layerPieces;
        private RectTransform layerWallsH;
        private RectTransform layerWallsV;
        private RectTransform layerJoints;

        // --- Instantiated views ---
        private readonly List<TileView> tileViews = new();
        private readonly List<WallView> wallHViews = new();
        private readonly List<WallView> wallVViews = new();
        private readonly List<JointView> jointViews = new();
        private GridView gridView;



        // ============================================================
        // Public Variables
        // ============================================================

        public int GridWidth => boardData.Width;
        public int GridHeight => boardData.Height;


        public Vector2 BoardSizePx => new Vector2(GridWidth * CellSizePx, GridHeight * CellSizePx);
        public Vector2 OuterBoardSizePx => BoardSizePx + (Vector2.one * FrameThickness);
        public float CellSizePx => cellSize;
        private float FrameThickness => Mathf.Max(gridThickness, wallThickness);



        public RectTransform PiecesLayer => layerPieces;

        public BoardData BoardData
        {
            get => boardData;
            set
            {
                if (boardData == value)
                    return;

                boardData = value;
                Rebuild();
            }
        }

        /// <summary>
        /// Converts a board cell coordinate to local UI position
        /// without centering (cell bottom-left corner).
        /// </summary>
        public Vector2 CellPositionToLocalPosition(Vector2Int cellPosition)
        {
            return new Vector2(
                cellPosition.x * cellSize,
                cellPosition.y * cellSize
            );
        }



        // ============================================================
        // Lifecycle
        // ============================================================

        public void Rebuild()
        {
            if (boardData == null)
                return;

            ClearHierarchy();

            CreateLayers();
            ApplyLayerOrder();
            ResizeBoard();
            //ApplyOriginShift();
            ApplyLayerOrder();

            CreateBackground();
            CreateTiles();
            CreateGrid();
            CreateWalls();
            CreateJoints();

            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }

        // ============================================================
        // Grid Management
        // ============================================================

        private void ResizeBoard()
        {
            SetLayoutSize(OuterBoardSizePx);
        }


        // ============================================================
        // Hierarchy Management
        // ============================================================

        private void ClearHierarchy()
        {
            tileViews.Clear();
            wallHViews.Clear();
            wallVViews.Clear();
            jointViews.Clear();
            gridView = null;

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (Application.isPlaying)
                    Destroy(child.gameObject);
                else
                    DestroyImmediate(child.gameObject);
            }
        }

        private void CreateLayers()
        {
            layerBackground = CreateLayer("Background");
            layerTiles = CreateLayer("Tiles");
            layerGrid = CreateLayer("Grid");
            layerPieces = CreateLayer("Pieces");
            layerWallsH = CreateLayer("Walls-H");
            layerWallsV = CreateLayer("Walls-V");
            layerJoints = CreateLayer("Joints");

            layers.Clear();
            layers[BoardViewLayer.Background] = layerBackground;
            layers[BoardViewLayer.Tiles] = layerTiles;
            layers[BoardViewLayer.Grid] = layerGrid;
            layers[BoardViewLayer.Pieces] = layerPieces;
            layers[BoardViewLayer.WallsHorizontal] = layerWallsH;
            layers[BoardViewLayer.WallsVertical] = layerWallsV;
            layers[BoardViewLayer.Joints] = layerJoints;
        }

        private RectTransform CreateLayer(string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(transform, false);

            Vector2 origin = OuterBoardSizePx * -RectTransform.pivot; // Position of "outer" origin
            origin += Vector2.one * FrameThickness * 0.5f; // position of origin

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.zero;
            rt.pivot = Vector2.zero;
            rt.localPosition = origin;
            rt.sizeDelta = BoardSizePx;

            return rt;
        }

        private void ApplyLayerOrder()
        {
            if (layerTiles == null || layerWallsH == null || layerWallsV == null)
                return;

            // Build an ordered list
            List<(int order, RectTransform layer)> ordered = new()
            {
                (-1, layerBackground), 
                (tilesLayerOrder, layerTiles),
                (gridLayerOrder, layerGrid),
                (pieceLayerOrder, layerPieces),
                (wallsHLayerOrder, layerWallsH),
                (wallsVLayerOrder, layerWallsV),
                (jointsLayerOrder, layerJoints),
            };

            // Sort by order value
            ordered.Sort((a, b) => a.order.CompareTo(b.order));

            // Apply sibling indices in sorted order
            for (int i = 0; i < ordered.Count; i++)
                ordered[i].layer.SetSiblingIndex(i);
        }

        public bool AttachToLayer(BoardViewLayer layer, RectTransform t)
        {
            if (t == null)
                return false;

            if (!layers.TryGetValue(layer, out var rt) || rt == null)
            {
                Debug.LogError($"BoardView layer '{layer}' not available. Did you call Rebuild()?");
                return false;
            }

            t.SetParent(rt, false);

            t.anchorMin = Vector2.zero;
            t.anchorMax = Vector2.zero;
            t.pivot = Vector2.zero;
            t.localPosition = Vector2.zero;

            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;

            return true;
        }

        // ============================================================
        // Background Instantiation
        // ============================================================

        private void CreateBackground()
        {
            var backgroundImage = layerBackground.gameObject.AddComponent<Image>();
            backgroundImage.color = backgroundColor;

            //layerBackground.anchorMin = Vector2.zero;
            //layerBackground.anchorMax = Vector2.one;
            //layerBackground.offsetMin = Vector2.zero;
            //layerBackground.offsetMax = Vector2.zero;
        }



        // ============================================================
        // Tile Instantiation
        // ============================================================

        private void CreateTiles()
        {
            foreach (var td in boardData.Tiles)
            {
                var tv = Instantiate(tilePrefab, layerTiles);
                tv.name = $"Tile ({td.X},{td.Y})";

                Vector2 pos = new Vector2(
                    td.X * cellSize + cellSize * 0.5f,
                    td.Y * cellSize + cellSize * 0.5f
                );

                tv.SetLocalPosition(pos);
                tv.SetSize(new Vector2(cellSize, cellSize));
                tv.SetColor(tileColor);

                tileViews.Add(tv);
            }
        }



        // ============================================================
        // Wall Instantiation
        // ============================================================

        private void CreateWalls()
        {
            foreach (var w in boardData.Walls)
            {
                if (w.Axis == EdgeAxis.Horizontal)
                    CreateHorizontalWall(w);
                else
                    CreateVerticalWall(w);
            }
        }

        private void CreateHorizontalWall(WallData w)
        {
            var hv = Instantiate(wallHorizontalPrefab, layerWallsH);
            hv.name = $"Wall H ({w.X},{w.Y})";

            Vector2 pos = new Vector2(
                w.X * cellSize + cellSize * 0.5f,
                w.Y * cellSize
            );

            hv.SetLocalPosition(pos);
            hv.SetSize(new Vector2(cellSize - wallThickness, wallThickness));
            hv.SetColor(wallColor);

            wallHViews.Add(hv);
        }

        private void CreateVerticalWall(WallData w)
        {
            var vv = Instantiate(wallVerticalPrefab, layerWallsV);
            vv.name = $"Wall V ({w.X},{w.Y})";

            Vector2 pos = new Vector2(
                w.X * cellSize,
                w.Y * cellSize + cellSize * 0.5f
            );

            vv.SetLocalPosition(pos);
            vv.SetSize(new Vector2(wallThickness, cellSize - wallThickness));
            vv.SetColor(wallColor);

            wallVViews.Add(vv);
        }



        // ============================================================
        // Joint Instantiation
        // ============================================================
        private void CreateJoints()
        {
            if (jointPrefab == null)
                return;

            int w = boardData.Width;
            int h = boardData.Height;

            for (int jx = 0; jx <= w; jx++)
            {
                for (int jy = 0; jy <= h; jy++)
                {
                    if (!IsJointActive(jx, jy))
                        continue;

                    var jv = Instantiate(jointPrefab, layerJoints);
                    jv.name = $"Joint ({jx},{jy})";

                    Vector2 pos = new Vector2(jx * cellSize, jy * cellSize);

                    jv.SetLocalPosition(pos);
                    jv.SetSize(new Vector2(wallThickness, wallThickness));
                    jv.SetColor(jointColor);

                    jointViews.Add(jv);
                }
            }
        }

        private bool IsJointActive(int jx, int jy)
        {
            // Option B — show if ANY wall touches

            if (HasHorizontalWallAt(jx, jy)) return true;
            if (HasHorizontalWallAt(jx - 1, jy)) return true;

            if (HasVerticalWallAt(jx, jy)) return true;
            if (HasVerticalWallAt(jx, jy - 1)) return true;

            return false;
        }

        private bool HasHorizontalWallAt(int x, int y)
        {
            if (x < 0 || x >= boardData.Width) return false;
            if (y < 0 || y > boardData.Height) return false;

            foreach (var w in boardData.Walls)
                if (w.Axis == EdgeAxis.Horizontal && w.X == x && w.Y == y)
                    return true;

            return false;
        }

        private bool HasVerticalWallAt(int x, int y)
        {
            if (x < 0 || x > boardData.Width) return false;
            if (y < 0 || y >= boardData.Height) return false;

            foreach (var w in boardData.Walls)
                if (w.Axis == EdgeAxis.Vertical && w.X == x && w.Y == y)
                    return true;

            return false;
        }



        // ============================================================
        // Grid Instantiation
        // ============================================================

        private void CreateGrid()
        {
            if (gridPrefab == null)
                return;

            gridView = Instantiate(gridPrefab, layerGrid);
            gridView.name = "Grid";

            gridView.RectTransform.anchorMax = Vector2.zero;
            gridView.RectTransform.anchorMin = Vector2.zero;
            gridView.RectTransform.pivot = Vector2.zero;

            gridView.SetSize(BoardSizePx + (Vector2.one * gridThickness));
            gridView.SetLocalPosition(Vector2.one * -(gridThickness * 0.5f));

            // Generate procedural grid texture
            Texture2D tex = GenerateGridTexture(GridWidth, GridHeight, CellSizePx, gridThickness);
            gridView.SetTexture(tex);
            gridView.SetColor(gridColor);
        }

        private Texture2D GenerateGridTexture(int widthCells, int heightCells, float cellSize, float thickness)
        {
            int texWidth = Mathf.RoundToInt((widthCells * cellSize) + thickness);
            int texHeight = Mathf.RoundToInt((heightCells * cellSize) + thickness);

            Texture2D tex = new Texture2D(texWidth, texHeight, TextureFormat.ARGB32, false);
            tex.filterMode = FilterMode.Point;

            Color32 clear = new Color(0, 0, 0, 0);
            Color32 line = Color.white;

            for (int y = 0; y < texHeight; y++)
            {
                for (int x = 0; x < texWidth; x++)
                {
                    bool isVerticalLine = (x % cellSize < thickness);
                    bool isHorizontalLine = (y % cellSize < thickness);

                    bool isRightEdge = (x >= texWidth - thickness);
                    bool isTopEdge = (y >= texHeight - thickness);

                    bool isLine = isVerticalLine || isHorizontalLine || isRightEdge || isTopEdge;

                    tex.SetPixel(x, y, isLine ? line : clear);
                }
            }

            tex.Apply();
            return tex;
        }

        // ============================================================
        // Editor
        // ============================================================

#if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool dirty;
        public bool IsDirty => dirty;
        public void ClearDirty() => dirty = false;

        private void OnValidate()
        {
            if (!Application.isPlaying)
                dirty = true;
        }
#endif
    }
}
