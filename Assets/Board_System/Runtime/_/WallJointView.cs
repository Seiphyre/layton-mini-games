using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Visual element placed at the intersection of two or more walls.
/// Pure view: no gameplay logic.
/// </summary>
public class WallJointView : UIElement
{
    [SerializeField] private Image image;

    /// <summary>
    /// center: local-space center of the joint
    /// size: width/height (usually wallThickness x wallThickness)
    /// color: same as wall color or stylized
    /// </summary>
    public void Initialize(Vector2 center, Vector2 size, Color color)
    {
        if (image == null)
            image = GetComponent<Image>();

        // Center pivot makes positioning trivial
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
