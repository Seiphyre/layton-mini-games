using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Visual representation of a wall between two tiles.
/// Pure view component (no gameplay logic).
/// </summary>
public class WallView_OLD : UIElement
{
    [SerializeField] private Image image;

    /// <summary>
    /// center: center of the wall in local coordinates
    /// size: width/height of the wall in pixels
    /// color: color of the wall
    /// </summary>
    public void Initialize(Vector2 center, Vector2 size, Color color)
    {
        if (image == null)
            image = GetComponent<Image>();

        // Pivot center = easiest for wall placement
        RectTransform.pivot = new Vector2(0.5f, 0.5f);

        SetSize(size);
        SetLocalPosition(center);
        SetColor(color);
    }

    public void SetColor(Color c)
    {
        if (image == null)
            image = GetComponent<Image>();

        image.color = c;
    }
}
