using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FixedGridLayoutGroup : LayoutGroup
{
    [Header("Grid")]
    [Min(1)] public int Rows = 2;
    [Min(1)] public int Columns = 5;
    public Vector2 Spacing = new Vector2(16f, 16f);

    // ─────────────────────────────────────────────
    // Layout Input Phase
    // ─────────────────────────────────────────────

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        SetLayoutInputForAxis(0f, 0f, 0f, 0);
    }

    public override void CalculateLayoutInputVertical()
    {
        SetLayoutInputForAxis(0f, 0f, 0f, 1);
    }

    // ─────────────────────────────────────────────
    // Layout Execution Phase
    // ─────────────────────────────────────────────

    public override void SetLayoutHorizontal() => LayoutChildren();
    public override void SetLayoutVertical() => LayoutChildren();

    private void LayoutChildren()
    {
        if (Columns <= 0 || Rows <= 0)
            return;

        int childCount = rectChildren.Count;
        if (childCount == 0)
            return;

        // Cell size is derived from the configured grid (preferred rows/columns)
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        float availableWidth = width - padding.horizontal;
        float availableHeight = height - padding.vertical;

        float totalSpacingX = Spacing.x * (Columns - 1);
        float totalSpacingY = Spacing.y * (Rows - 1);

        float cellWidth = (availableWidth - totalSpacingX) / Columns;
        float cellHeight = (availableHeight - totalSpacingY) / Rows;

        // Actual usage
        int usedRows = Mathf.CeilToInt(childCount / (float)Columns);
        int usedColsLastRow = Mathf.Min(Columns, childCount - (usedRows - 1) * Columns);

        // Grid footprint used for GRID-level alignment
        float usedContentWidth =
            (usedRows == 1 ? usedColsLastRow : Columns) * cellWidth +
            (usedRows == 1 ? usedColsLastRow - 1 : Columns - 1) * Spacing.x;

        float usedContentHeight =
            usedRows * cellHeight +
            (usedRows - 1) * Spacing.y;

        Vector2 alignmentOffset = GetAlignmentOffset(
            availableWidth,
            availableHeight,
            usedContentWidth,
            usedContentHeight
        );

        // Layout children
        for (int i = 0; i < childCount; i++)
        {
            int row = i / Columns;
            int col = i % Columns;

            // Per-row alignment for partial last row (only if multiple rows)
            float rowOffsetX = 0f;

            bool isLastRow = (row == usedRows - 1);
            if (usedRows > 1 && isLastRow && usedColsLastRow < Columns)
            {
                float missingWidth =
                    (Columns - usedColsLastRow) * cellWidth +
                    (Columns - usedColsLastRow) * Spacing.x;

                switch (childAlignment)
                {
                    case TextAnchor.UpperCenter:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.LowerCenter:
                        rowOffsetX = missingWidth * 0.5f;
                        break;

                    case TextAnchor.UpperRight:
                    case TextAnchor.MiddleRight:
                    case TextAnchor.LowerRight:
                        rowOffsetX = missingWidth;
                        break;
                }
            }

            float x =
                padding.left +
                alignmentOffset.x +
                rowOffsetX +
                col * (cellWidth + Spacing.x);

            float y =
                padding.top +
                alignmentOffset.y +
                row * (cellHeight + Spacing.y);

            SetChildAlongAxis(rectChildren[i], 0, x, cellWidth);
            SetChildAlongAxis(rectChildren[i], 1, y, cellHeight);
        }
    }

    private Vector2 GetAlignmentOffset(
        float availableWidth,
        float availableHeight,
        float contentWidth,
        float contentHeight)
    {
        float offsetX = 0f;
        float offsetY = 0f;

        // Horizontal
        switch (childAlignment)
        {
            case TextAnchor.UpperCenter:
            case TextAnchor.MiddleCenter:
            case TextAnchor.LowerCenter:
                offsetX = (availableWidth - contentWidth) * 0.5f;
                break;

            case TextAnchor.UpperRight:
            case TextAnchor.MiddleRight:
            case TextAnchor.LowerRight:
                offsetX = availableWidth - contentWidth;
                break;
        }

        // Vertical
        switch (childAlignment)
        {
            case TextAnchor.MiddleLeft:
            case TextAnchor.MiddleCenter:
            case TextAnchor.MiddleRight:
                offsetY = (availableHeight - contentHeight) * 0.5f;
                break;

            case TextAnchor.LowerLeft:
            case TextAnchor.LowerCenter:
            case TextAnchor.LowerRight:
                offsetY = availableHeight - contentHeight;
                break;
        }

        return new Vector2(offsetX, offsetY);
    }
}