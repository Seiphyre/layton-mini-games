using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OneStopShop
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button Button;
        [SerializeField] private TMP_Text ButtonText;

        public LevelDefinition LevelData { get; private set; }

        public event Action<LevelButton> OnCick;

        private void OnEnable()
        {
            Button.onClick.AddListener(InvokeOnClickEvent);
        }

        private void OnDisable()
        {
            Button.onClick.RemoveListener(InvokeOnClickEvent);
        }

        public void Initialize(LevelDefinition levelData)
        {
            LevelData = levelData;

            ButtonText.text = levelData.Name;
        }

        private void InvokeOnClickEvent()
        {
            OnCick?.Invoke(this);
        }
    }
}
