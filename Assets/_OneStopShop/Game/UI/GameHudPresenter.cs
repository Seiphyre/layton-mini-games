using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
{
    public class GameHudPresenter
    {
        readonly GameHudView view;
        readonly GameplayController gameplay;

        readonly ActionPanelPresenter actionPanelPresenter;


        
        // ---------------------------------------------------
        // Contructor
        // ---------------------------------------------------

        public GameHudPresenter(
            GameHudView view,
            GameplayController gameplay)
        {
            this.view = view;
            this.gameplay = gameplay;

            actionPanelPresenter = new ActionPanelPresenter(
                view.ActionPanel,
                gameplay
            );
        }

        public void Initialize()
        {
            actionPanelPresenter.Initialize();

            view.SetTitle(gameplay.LevelTitle);

            view.OverlayRoot.ResultPopupView.OnRetryClicked += OnRetryClicked;
            view.OverlayRoot.ResultPopupView.OnContinueClicked += OnContinueClicked;
            view.OverlayRoot.ResultPopupView.OnDismissClicked += OnDismissClicked;

            gameplay.BoardValidated += OnBoardValidated;
        }

        public void Dispose()
        {
            gameplay.BoardValidated -= OnBoardValidated;

            view.OverlayRoot.ResultPopupView.OnRetryClicked -= OnRetryClicked;
            view.OverlayRoot.ResultPopupView.OnContinueClicked -= OnContinueClicked;
            view.OverlayRoot.ResultPopupView.OnDismissClicked -= OnDismissClicked;

            actionPanelPresenter.Dispose();
        }



        // ---------------------------------------------------
        // Contructor
        // ---------------------------------------------------

        private void OnBoardValidated(VictoryValidationResult result)
        {
            view.OverlayRoot.ShowResult(result);
        }

        private void OnRetryClicked()
        {
            view.OverlayRoot.HideAll();
            gameplay.ResetLevel();
        }

        private void OnContinueClicked()
        {
            view.OverlayRoot.HideAll();

            if (gameplay.HasNextLevel())
                gameplay.NextLevel();
            else
                gameplay.ExitGame();
        }

        private void OnDismissClicked()
        {
            view.OverlayRoot.HideAll();
            gameplay.ExitGame();
        }
    }
}
