using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.Boards.Views;
using VForge.Inventories;

namespace VForge.Gameplay.UI
{
    /// <summary>
    /// UI adapter that converts drag/drop events into board cell placement.
    /// Lives entirely in UI space.
    /// </summary>
    public sealed class BoardDropAdapter : MonoBehaviour
    {
        [SerializeField] private DropZone dropZone;
        [SerializeField] private BoardView boardView;

        private PiecePlacementController _controller;

        public void Initialize(PiecePlacementController controller)
        {
            _controller = controller;

            dropZone.onEnter.AddListener(OnEnter);
            dropZone.onMove.AddListener(OnMove);
            dropZone.onExit.AddListener(OnExit);
            dropZone.onDropped.AddListener(OnDropped);
        }

        private void OnDestroy()
        {
            if (dropZone == null) return;

            dropZone.onEnter.RemoveListener(OnEnter);
            dropZone.onMove.RemoveListener(OnMove);
            dropZone.onExit.RemoveListener(OnExit);
            dropZone.onDropped.RemoveListener(OnDropped);
        }

        // --------------------------------------------------
        // DropZone callbacks
        // --------------------------------------------------

        private void OnEnter(Draggable draggable)
        {
            if (!TryGetItem(draggable, out var item))
                return;

            _controller.BeginPlacement(item);
        }

        private void OnMove(Draggable draggable)
        {
            if (!_controller.HasActivePlacement)
                return;

            if (!TryGetCell(draggable, out var cell))
                return;

            _controller.CanPlaceAt(cell);
        }

        private void OnExit(Draggable draggable)
        {
            _controller.CancelPlacement();
        }

        private void OnDropped(Draggable draggable)
        {
            if (!TryGetItem(draggable, out var item))
                return;

            if (!TryGetCell(draggable, out var cell))
                return;

            _controller.CommitPlacement(cell);
        }

        // --------------------------------------------------
        // Helpers
        // --------------------------------------------------

        private static bool TryGetItem(
            Draggable draggable,
            out InventoryItem<PieceDefinition> item)
        {
            item = draggable.Payload as InventoryItem<PieceDefinition>;
            return item != null;
        }

        private bool TryGetCell(Draggable draggable, out Vector2Int cell)
        {
            cell = default;

            if (boardView == null)
                return false;

            Vector2 screenPos = Input.mousePosition;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    boardView.GetLayer(BoardViewLayer.Pieces),
                    screenPos,
                    boardView.Canvas?.worldCamera,
                    out var local))
                return false;

            return boardView.TryLocalPositionToCellPosition(local, out cell);
        }
    }
}
