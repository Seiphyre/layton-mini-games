using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.Boards.Views;

namespace VForge.BoardPieces.Views
{
    public class PieceBoardView : UIElement
    {
        [SerializeField] private RectTransform boardRoot;
        [SerializeField] private PieceView piecePrefab;
        [SerializeField] private PieceView pieceLockedPrefab;

        private IBoardViewContext viewContext;
        private PieceBoard board;
        private PieceView previewView;
        private readonly Dictionary<int, PieceView> pieceViews = new();

        public event Action<PieceView> OnPieceViewCreated;
        public event Action<PieceView> OnPieceViewDestroyed;

        public PieceBoard PieceBoard { get { return board; } }
        public IReadOnlyList<PieceView> PieceViews => pieceViews.Values.ToList();



        public void Initialize(PieceBoard board, IBoardViewContext viewContext)
        {
            if (this.board != null)
            {
                Debug.LogWarning("PieceBoardView already initialized.");
                return;
            }

            this.board = board;
            this.viewContext = viewContext;

            board.OnPiecePlaced += OnPlaced;
            board.OnPieceMoved += OnMoved;
            board.OnPieceRemoved += OnRemoved;

            Rebuild();
        }

        private void OnDestroy()
        {
            if (board == null)
                return;

            board.OnPiecePlaced -= OnPlaced;
            board.OnPieceMoved -= OnMoved;
            board.OnPieceRemoved -= OnRemoved;
        }

        private void Rebuild()
        {
            foreach (var v in pieceViews.Values)
                Destroy(v.gameObject);
            pieceViews.Clear();

            ResizeView();

            foreach (var p in board.PlacedPieces)
                CreatePieceView(p);

            HidePreview();
        }

        private void ResizeView()
        {
            SetSize(viewContext.BoardSizePx);
        }

        private void OnPlaced(Piece p)
        {
            CreatePieceView(p);
        }

        private void OnMoved(Piece p)
        {
            RefreshPieceView(p);
        }

        private void OnRemoved(Piece p)
        {
            DestroyPieceView(p);
        }

        private void CreatePieceView(Piece piece)
        {
            var v = Instantiate(piece.IsLocked ? pieceLockedPrefab : piecePrefab, boardRoot);

            v.name = $"Piece {piece.Id} ({piece.CellPosition.x},{piece.CellPosition.y})";

            v.RectTransform.anchorMin = Vector2.zero;
            v.RectTransform.anchorMax = Vector2.zero;
            v.RectTransform.pivot = Vector2.zero;

            v.Initialize(piece, viewContext);
            v.SetLocalPosition(viewContext.CellPositionToLocalPosition(piece.CellPosition));

            pieceViews[piece.Id] = v;

            OnPieceViewCreated?.Invoke(v);
        }

        private void DestroyPieceView(Piece piece)
        {
            if (pieceViews.TryGetValue(piece.Id, out var pieceView))
            {
                OnPieceViewDestroyed?.Invoke(pieceView);

                Destroy(pieceView.gameObject);
                pieceViews.Remove(piece.Id);
            }
        }

        private void RefreshPieceView(Piece piece)
        {
            if (pieceViews.TryGetValue(piece.Id, out var pieceView))
            {
                pieceView.SetLocalPosition(viewContext.CellPositionToLocalPosition(piece.CellPosition));
                pieceView.RefreshView();
            }
        }

        // --------------------------------------------------
        // Preview (ghost) API — Phase 3.4
        // --------------------------------------------------

        public void CreatePreview(PieceDefinition definition)
        {
            if (previewView == null)
            {
                previewView = Instantiate(piecePrefab, boardRoot);
                previewView.name = "Preview Piece";

                previewView.RectTransform.anchorMin = Vector2.zero;
                previewView.RectTransform.anchorMax = Vector2.zero;
                previewView.RectTransform.pivot = Vector2.zero;

                previewView.Initialize(new Piece(definition, Vector2Int.zero, locked: false), viewContext);
                previewView.SetPreviewMode(true);
            }
        }

        public void DestroyPreview()
        {
            if (previewView != null)
            {
                Destroy(previewView.gameObject);

                previewView = null;
            }
        }

        public void ShowPreview()
        {
            if (previewView != null)
                previewView.gameObject.SetActive(true);
        }

        public void HidePreview()
        {
            if (previewView != null)
                previewView.gameObject.SetActive(false);
        }

        public void SetPreviewPosition(Vector2Int cell)
        {
            if (previewView == null)
                return;

            //previewView.SetCellPosition(cell);
            previewView.SetLocalPosition(viewContext.CellPositionToLocalPosition(cell));
        }

        public void SetPreviewValidity(bool isValid)
        {
            if (previewView == null)
                return;

            previewView.SetValidity(isValid);
        }

    }
}
