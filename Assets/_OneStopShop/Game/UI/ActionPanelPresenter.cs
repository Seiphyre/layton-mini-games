using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
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
            view.OnExitClicked += OnExitClicked;

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
            view.OnExitClicked -= OnExitClicked;

            gameplay.InventoryStateChanged -= OnInventoryStateChanged;
            gameplay.GameStateChanged -= OnGameStateChanged;
        }



        // -------------------------------------
        // Update logic
        // -------------------------------------

        private void UpdateView()
        {
            view.SetFinishCheckEnabled(gameStarted && inventoryEmpty);
            view.SetResetEnabled(gameStarted);
            view.SetExitEnabled(gameStarted);
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
            gameplay.ResetLevel();
        }

        private void OnExitClicked()
        {
            gameplay.ExitGame();
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
