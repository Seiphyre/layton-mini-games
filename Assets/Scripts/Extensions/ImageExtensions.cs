using UnityEngine;
using UnityEngine.UI;

public static class ImageExtensions
{
    public static bool CollideWith(this Image a, Image b, bool pixelPerfect = false)
    {
        RectTransform rtA = a.rectTransform;
        RectTransform rtB = b.rectTransform;

        // 1. Get world-space rects
        Rect rectA = GetWorldRect(rtA);
        Rect rectB = GetWorldRect(rtB);

        // 2. Compute intersection
        Rect overlap = RectIntersection(rectA, rectB);
        if (overlap.width <= 0 || overlap.height <= 0)
            return false;

        if (!pixelPerfect)
            return true;

        // 3. Get textures
        Texture2D texA = a.sprite.texture;
        Texture2D texB = b.sprite.texture;

        // Needed values
        Vector2 pivotA = a.sprite.pivot;
        Vector2 pivotB = b.sprite.pivot;

        Vector2 sizeA = a.sprite.rect.size;
        Vector2 sizeB = b.sprite.rect.size;

        // 4. For each pixel inside the overlap rectangle
        int steps = 4; // sampling resolution → can increase for more precision
        for (float x = overlap.xMin; x < overlap.xMax; x += steps)
        {
            for (float y = overlap.yMin; y < overlap.yMax; y += steps)
            {
                // Convert world pos → local pos of A
                Vector2 localA = WorldToLocalPixel(rtA, x, y, sizeA, pivotA);
                Vector2 localB = WorldToLocalPixel(rtB, x, y, sizeB, pivotB);

                if (!IsInside(localA, sizeA) || !IsInside(localB, sizeB))
                    continue;

                // Sample alpha of both textures
                Color ca = texA.GetPixel((int)localA.x, (int)localA.y);
                Color cb = texB.GetPixel((int)localB.x, (int)localB.y);

                // If both pixels are visible → collision
                if (ca.a > 0.01f && cb.a > 0.01f)
                    return true;
            }
        }

        return false;
    }

    // ---------- Helpers ----------
    private static Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return new Rect(
            corners[0].x, corners[0].y,
            corners[2].x - corners[0].x,
            corners[2].y - corners[0].y
        );
    }

    private static Rect RectIntersection(Rect a, Rect b)
    {
        float x1 = Mathf.Max(a.xMin, b.xMin);
        float y1 = Mathf.Max(a.yMin, b.yMin);
        float x2 = Mathf.Min(a.xMax, b.xMax);
        float y2 = Mathf.Min(a.yMax, b.yMax);

        if (x2 >= x1 && y2 >= y1)
            return new Rect(x1, y1, x2 - x1, y2 - y1);

        return Rect.zero;
    }

    private static Vector2 WorldToLocalPixel(RectTransform rt, float wx, float wy, Vector2 spriteSize, Vector2 pivot)
    {
        Vector2 world = new Vector2(wx, wy);
        Vector2 local = rt.InverseTransformPoint(world);

        // Convert RectTransform local → sprite pixel coords
        float x = (local.x + rt.rect.width * 0.5f) * (spriteSize.x / rt.rect.width);
        float y = (local.y + rt.rect.height * 0.5f) * (spriteSize.y / rt.rect.height);

        return new Vector2(x, y);
    }

    private static bool IsInside(Vector2 p, Vector2 size)
    {
        return (p.x >= 0 && p.x < size.x && p.y >= 0 && p.y < size.y);
    }
}
