using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OneStopShop
{
    public class ActionPanelView : MonoBehaviour
    {
        [SerializeField] Button finishCheckButton;
        [SerializeField] Button resetButton;
        [SerializeField] Button exitButton;

        public event Action OnFinishCheckClicked;
        public event Action OnResetClicked;
        public event Action OnExitClicked;



        // --------------------------------------------------
        // Monobehaviour lifecycle
        // --------------------------------------------------

        private void OnEnable()
        {
            finishCheckButton.onClick.AddListener(InvokeOnFinishCheckClickedEvent);
            resetButton.onClick.AddListener(InvokeOnResetClickedEvent);
            exitButton.onClick.AddListener(InvokeOnExitClickedEvent);
        }

        private void OnDisable()
        {
            finishCheckButton.onClick.RemoveListener(InvokeOnFinishCheckClickedEvent);
            resetButton.onClick.RemoveListener(InvokeOnResetClickedEvent);
            exitButton.onClick.RemoveListener(InvokeOnExitClickedEvent);
        }



        // --------------------------------------------------
        // public API
        // --------------------------------------------------

        public void SetFinishCheckEnabled(bool enabled)
        {
            finishCheckButton.interactable = enabled;
        }

        public void SetResetEnabled(bool enabled)
        {
            resetButton.interactable = enabled;
        }

        public void SetExitEnabled(bool enabled)
        {
            exitButton.interactable = enabled;
        }



        // --------------------------------------------------
        // internal helpers
        // --------------------------------------------------

        private void InvokeOnFinishCheckClickedEvent()
        {
            OnFinishCheckClicked?.Invoke();
        }

        private void InvokeOnResetClickedEvent()
        {
            OnResetClicked?.Invoke();
        }

        private void InvokeOnExitClickedEvent()
        {
            OnExitClicked?.Invoke();
        }
    }
}
