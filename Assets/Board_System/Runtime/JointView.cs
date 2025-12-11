using UnityEngine;
using UnityEngine.UI;

namespace BoardSystem
{
    public class JointView : UIElement
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