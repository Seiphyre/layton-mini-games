using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VForge.Boards.Views
{
    public class WallView : UIElement
    {
        [SerializeField] private Image _image;

        private void Awake()
        {
            if (_image == null)
                _image = GetComponent<Image>();
        }

        public void SetColor(Color c)
        {
            if (_image != null)
                _image.color = c;
        }
    }
}
