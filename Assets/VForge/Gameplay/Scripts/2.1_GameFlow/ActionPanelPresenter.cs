using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VForge.Gameplay
{
    public class ActionPanelPresenter
    {
        readonly ActionPanelView view;
        readonly GameplayController gameplay;

        private bool inventoryEmpty = false;
        private bool gameStarted = false;



        // -------------------------------------
        // Init logic
        // -------------------------------------

        public ActionPanelPresenter(
            ActionPanelView view,
            GameplayController gameplay)
        {
            this.view = view;
            this.gameplay = gameplay;
        }

        public void Initialize()
        {
            view.OnFinishCheckClicked += OnFinishCheckClicked;
            view.OnResetClicked += OnResetClicked;

            gameplay.InventoryStateChanged += OnInventoryStateChanged;
            gameplay.GameStateChanged += OnGameStateChanged;

            inventoryEmpty = gameplay.InventoryState.InventoryEmpty;
            gameStarted = gameplay.GameState.Started;

            UpdateView();
        }

        public void Dispose()
        {
            view.OnFinishCheckClicked -= OnFinishCheckClicked;
            view.OnResetClicked -= OnResetClicked;

            gameplay.InventoryStateChanged -= OnInventoryStateChanged;
            gameplay.GameStateChanged -= OnGameStateChanged;
        }



        // -------------------------------------
        // Update logic
        // -------------------------------------

        private void UpdateView()
        {
            view.SetFinishCheckEnabled(gameStarted && inventoryEmpty);
            view.SetResetEnabled(/*gameStarted*/false);
        }



        // -------------------------------------
        // Click events
        // -------------------------------------

        private void OnFinishCheckClicked()
        {
            gameplay.EndGame();
            gameplay.ValidateBoard();
        }

        private void OnResetClicked()
        {
            gameplay.ResetGame();
        }



        // -------------------------------------
        // State change events
        // -------------------------------------

        private void OnInventoryStateChanged(InventoryState state)
        {
            inventoryEmpty = state.InventoryEmpty;
            UpdateView();
        }

        private void OnGameStateChanged(GameState state)
        {
            gameStarted = state.Started;
            UpdateView();
        }
    }
}
