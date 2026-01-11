using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VForge.Gameplay
{
    public class ActionPanelView : MonoBehaviour
    {
        [SerializeField] Button finishCheckButton;
        [SerializeField] Button resetButton;

        public event Action OnFinishCheckClicked;
        public event Action OnResetClicked;



        // --------------------------------------------------
        // Monobehaviour lifecycle
        // --------------------------------------------------

        private void OnEnable()
        {
            finishCheckButton.onClick.AddListener(InvokeOnFinishCheckClickedEvent);
            resetButton.onClick.AddListener(InvokeOnResetClickedEvent);
        }

        private void OnDisable()
        {
            finishCheckButton.onClick.RemoveListener(InvokeOnFinishCheckClickedEvent);
            resetButton.onClick.RemoveListener(InvokeOnResetClickedEvent);
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
    }
}
