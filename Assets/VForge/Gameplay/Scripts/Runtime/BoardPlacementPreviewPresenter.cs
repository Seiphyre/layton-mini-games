using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VForge.BoardPieces.Views;
using VForge.Boards.Views;
using VForge.Gameplay;

public sealed class BoardPlacementPreviewPresenter
{
    private readonly BoardDragAdapter boardAdapter;
    private readonly PiecePlacementController placement;
    private readonly PieceBoardView pieceBoardView;
    private readonly DragController dragController;



    // -----------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------

    public BoardPlacementPreviewPresenter(
        BoardDragAdapter boardAdapter,
        PiecePlacementController placement,
        DragController dragController,
        PieceBoardView boardView)
    {
        this.boardAdapter = boardAdapter ?? throw new ArgumentNullException(nameof(boardAdapter));
        this.placement = placement ?? throw new ArgumentNullException(nameof(placement));
        this.pieceBoardView = boardView ?? throw new ArgumentNullException(nameof(boardView));
        this.dragController = dragController ?? throw new ArgumentNullException(nameof(dragController));

        boardAdapter.EnterBoard += ShowPreviewAndHideProxy;
        boardAdapter.HoverCell += (cellPosition) => MovePreviewAndSetValidity(cellPosition);
        boardAdapter.ExitBoard += HidePreviewAndShowProxy;

        dragController.DragStarted += (dragSession) => CreatePreview();
        dragController.DragEnded += (dragSession) => DestroyPreview();
    }



    // -----------------------------------------------------------------
    // Preview Lifecycle
    // -----------------------------------------------------------------

    private void CreatePreview()
    {
        // Create Preview
        if (!placement.HasActivePlacement)
            return;

        pieceBoardView.CreatePreview(placement.ActiveDefinition);
        pieceBoardView.HidePreview();
    }

    private void MovePreviewAndSetValidity(Vector2Int cellPosition)
    {
        if (!placement.HasActivePlacement)
            return;

        // Move Preview
        pieceBoardView.SetPreviewPosition(cellPosition);

        // Set Preview Validity
        var result = pieceBoardView.PieceBoard.CanPlace(placement.ActiveDefinition, cellPosition);
        pieceBoardView.SetPreviewValidity(result.Success);
    }

    private void DestroyPreview()
    {
        // Destroy Preview
        pieceBoardView.DestroyPreview();
    }

    private void ShowPreviewAndHideProxy()
    {
        // Hide Proxy
        if (dragController.Current != null && dragController.Current.HasProxy)
            dragController.Current.Proxy.Hide();

        // Show Preview
        pieceBoardView.ShowPreview();
    }

    private void HidePreviewAndShowProxy()
    {
        // Show Proxy
        if (dragController.Current != null && dragController.Current.HasProxy)
            dragController.Current.Proxy.Show();

        // Hide Preview
        pieceBoardView.HidePreview();
    }
}
