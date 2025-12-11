using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Visual representation of a single tile in the grid.
/// Pure view element: no logic, no gameplay.
/// </summary>
public class TileView_OLD : UIElement
{
    [SerializeField] private Image image;

    /// <summary>
    /// Called by BoardView after instantiation.
    /// center: local-space position of the center of the tile
    /// tileSize: pixel size of the tile (square)
    /// color: color of the tile (hole or normal)
    /// </summary>
    public void Initialize(Vector2 center, int tileSize, Color color)
    {
        if (image == null)
            image = GetComponent<Image>();

        // Ensure pivot = center for easy placement
        RectTransform.pivot = new Vector2(0.5f, 0.5f);

        SetSize(new Vector2(tileSize, tileSize));
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
