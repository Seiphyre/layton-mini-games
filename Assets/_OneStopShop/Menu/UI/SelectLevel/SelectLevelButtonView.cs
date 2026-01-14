using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OneStopShop
{
    public sealed class SelectLevelButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text label;

        public void Bind(string text, Action onClick = null)
        {
            Unbind();

            label.text = text;
            button.onClick.AddListener(() => onClick?.Invoke());
        }

        public void Unbind()
        {
            label.text = string.Empty;
            button.onClick.RemoveAllListeners();
        }
    }
}
