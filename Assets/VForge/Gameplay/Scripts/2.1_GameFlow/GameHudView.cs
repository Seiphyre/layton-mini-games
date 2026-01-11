using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VForge.Boards.Views;

namespace VForge.Gameplay
{
    public class GameHudView : MonoBehaviour
    {
        [Header("Board")]
        [SerializeField] BoardView boardView;

        [Header("HUD")]
        [SerializeField] TMP_Text titleText;
        [SerializeField] PieceInventoryView inventoryView;
        [SerializeField] ActionPanelView actionPanelView;

        [Header("Overlays")]
        [SerializeField] OverlayRootView overlayRootView;

        // ----- Getters only -----

        public BoardView BoardView => boardView;
        public PieceInventoryView InventoryView => inventoryView;
        public ActionPanelView ActionPanel => actionPanelView;
        public OverlayRootView OverlayRoot => overlayRootView;

        // --

        public void SetTitle(string title)
        {
            titleText.text = title;
        }
    }
}
