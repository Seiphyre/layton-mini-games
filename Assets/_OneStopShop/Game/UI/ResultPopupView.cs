using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OneStopShop
{
    public class ResultPopupView : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] TMP_Text titleText;
        [SerializeField] TMP_Text messageText;

        [Header("Buttons")]
        [SerializeField] Button retryButton;
        [SerializeField] Button continueButton;
        [SerializeField] Button dismissButton;

        public event Action OnDismissClicked;
        public event Action OnRetryClicked;
        public event Action OnContinueClicked;



        // --

        private void OnEnable()
        {
            retryButton.onClick.AddListener(InvokeOnRetryClickedEvent);
            continueButton.onClick.AddListener(InvokeOnContinueClickedEvent);
            dismissButton.onClick.AddListener(InvokeOnDismissClickedEvent);
        }

        private void OnDisable()
        {
            retryButton.onClick.RemoveListener(InvokeOnRetryClickedEvent);
            continueButton.onClick.RemoveListener(InvokeOnContinueClickedEvent);
            dismissButton.onClick.RemoveListener(InvokeOnDismissClickedEvent);
        }

        // --

        public void Show(VictoryValidationResult result)
        {
            gameObject.SetActive(true);

            switch (result.IsValid)
            {
                case true:
                    titleText.text = "Success!";
                    messageText.text = "You solved the puzzle.";
                    continueButton.gameObject.SetActive(true);
                    break;

                case false:
                    titleText.text = "Oh oh...";
                    messageText.text = "It didn't work. Give it another try!";
                    continueButton.gameObject.SetActive(false);
                    break;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        // --

        private void InvokeOnDismissClickedEvent()
        {
            OnDismissClicked?.Invoke();
        }

        private void InvokeOnRetryClickedEvent()
        {
            OnRetryClicked?.Invoke();
        }

        private void InvokeOnContinueClickedEvent()
        {
            OnContinueClicked?.Invoke();
        }
    }
}
