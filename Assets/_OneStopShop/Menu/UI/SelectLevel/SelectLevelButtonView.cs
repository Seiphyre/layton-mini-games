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
        [SerializeField] private TMP_Text LevelName;
        [SerializeField] private TMP_Text LevelNumber;
        [SerializeField] private Image Thumbnail;

        public void Bind(int levelNumber, string levelName, Sprite thumbnail, Action onClick = null)
        {
            Unbind();

            if (LevelNumber) LevelNumber.text = levelNumber.ToString();
            if (LevelName) LevelName.text = levelName;
            if (Thumbnail) Thumbnail.sprite = thumbnail;

            button.onClick.AddListener(() => onClick?.Invoke());
        }

        public void Unbind()
        {
            if (LevelNumber) LevelNumber.text = "1";
            if (LevelName) LevelName.text = "level";
            if (Thumbnail) Thumbnail.sprite = null;

            button.onClick.RemoveAllListeners();
        }
    }
}
