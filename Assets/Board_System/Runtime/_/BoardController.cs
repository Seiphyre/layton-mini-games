using BoardSystem;
using UnityEngine;

/// <summary>
/// Central orchestrator for the board system.
///
/// Responsibilities:
///   - Loads BoardData
///   - Creates and stores the runtime Board
///   - Connects Board to the BoardView
///   - Receives requests from gameplay systems (drag/drop, validation)
///   - Exposes a clean API for placing/removing pieces
///
/// Contains NO rendering logic and NO editor logic.
/// </summary>
public class BoardController : MonoBehaviour
{
    [Header("Board Data")]
    [SerializeField] private BoardData boardData;

    [Header("View")]
    [SerializeField] private BoardView_OLD boardView;

    /// <summary>
    /// The logical board used at runtime.
    /// </summary>
    public BoardModel Board { get; private set; }


    // --------------------------
    // INITIALIZATION
    // --------------------------

    private void Awake()
    {
        if (boardData == null)
        {
            Debug.LogError("[BoardController] Missing BoardData!");
            return;
        }

        // Create runtime board model
        Board = new BoardModel(boardData);

        // Initialize visual representation
        if (boardView != null)
            boardView.Create(Board);
    }


    // --------------------------
    // PUBLIC API FOR GAMEPLAY
    // --------------------------

    /// <summary>
    /// Requests a visual refresh of the board.
    /// </summary>
    public void RefreshView()
    {
        if (boardView != null)
            boardView.Refresh();
    }
}
