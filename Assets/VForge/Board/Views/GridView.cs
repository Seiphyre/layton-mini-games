using UnityEngine;
using UnityEngine.UI;

namespace VForge.Boards.Views
{
    public class GridView : UIElement
    {
        [SerializeField] private RawImage image;

        private void Awake()
        {
            if (image == null)
                image = GetComponent<RawImage>();
        }

        public void SetTexture(Texture2D tex)
        {
            if (image != null)
                image.texture = tex;
        }

        public void SetColor(Color c)
        {
            if (image != null)
                image.color = c;
        }
    }
}
