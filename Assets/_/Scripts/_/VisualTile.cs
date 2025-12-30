using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualTile : VisualElement
{
    [Header("Background"), Space]

    [SerializeField] private bool m_ShowBackground;
    [SerializeField] private Image m_BackgroundImg;

    private GameObject Background => m_BackgroundImg.gameObject;



    [Header("Borders"), Space]

    //[SerializeField] private bool m_ShowBorders = true;
    [SerializeField] private int m_BorderThickness = 2;
    [SerializeField] private Color m_BorderColor = Color.black;

    [Space]

    [SerializeField] private GameObject m_Borders;

    [Space]

    [SerializeField] private Image m_NorthBorderImg;
    [SerializeField] private Image m_SouthBorderImg;
    [SerializeField] private Image m_WestBorderImg;
    [SerializeField] private Image m_EastBorderImg;

    private RectTransform NorthBorder => m_NorthBorderImg.gameObject.transform as RectTransform;
    private RectTransform SouthBorder => m_SouthBorderImg.gameObject.transform as RectTransform;
    private RectTransform WestBorder => m_WestBorderImg.gameObject.transform as RectTransform;
    private RectTransform EastBorder => m_EastBorderImg.gameObject.transform as RectTransform;

    public int BorderThickness { get { return m_BorderThickness; } set { m_BorderThickness = value; } }
    public Color BorderColor { get { return m_BorderColor; } set { m_BorderColor = value; } }


    [Header("Walls"), Space]

    [SerializeField] private bool m_ShowWalls;
    [SerializeField] private int m_WallsThickness = 8;
    [SerializeField] private Color m_WallsColor = Color.blue;

    [Space]

    [SerializeField] private GameObject m_Walls;

    [Space]

    [SerializeField] private Image m_NorthWallImg;
    [SerializeField] private Image m_SouthWallImg;
    [SerializeField] private Image m_WestWallImg;
    [SerializeField] private Image m_EastWallImg;

    private RectTransform NorthWall => m_NorthWallImg.gameObject.transform as RectTransform;
    private RectTransform SouthWall => m_SouthWallImg.gameObject.transform as RectTransform;
    private RectTransform WestWall => m_WestWallImg.gameObject.transform as RectTransform;
    private RectTransform EastWall => m_EastWallImg.gameObject.transform as RectTransform;

    public int WallsThickness { get { return m_WallsThickness; } set { m_WallsThickness = value; } }
    public Color WallsColor { get { return m_WallsColor; } set { m_WallsColor = value; } }



    public override void Refresh()
    {
        base.Refresh();

        // -- 

        //if (Value != null && Value is Tile tile)
        //{
        //    if (Background != null) Background.SetActive(m_ShowBackground);

        //    if (m_BackgroundImg != null)
        //    {
        //        m_BackgroundImg.enabled = (!tile.IsEmpty);
        //        m_BackgroundImg.raycastTarget = (!tile.IsEmpty);
        //    }

        //    // --

        //    if (m_Borders != null)
        //        m_Borders.SetActive(m_ShowBorders);



        //    if (NorthBorder != null)
        //    {
        //        NorthBorder.pivot = new Vector2(0.5f, 0.5f);
        //        NorthBorder.anchorMin = new Vector2(0, 1);
        //        NorthBorder.anchorMax = new Vector2(1, 1);
        //        NorthBorder.offsetMin = new Vector2(-m_BorderThickness / 2, -m_BorderThickness / 2);
        //        NorthBorder.offsetMax = new Vector2(m_BorderThickness / 2, m_BorderThickness / 2);
        //    }
        //    if (m_NorthBorderImg != null)
        //    {
        //        m_NorthBorderImg.color = m_BorderColor;
        //    }



        //    if (SouthBorder != null)
        //    {
        //        SouthBorder.anchorMin = new Vector2(0, 0);
        //        SouthBorder.anchorMax = new Vector2(1, 0);
        //        SouthBorder.offsetMin = new Vector2(-m_BorderThickness / 2, -m_BorderThickness / 2);
        //        SouthBorder.offsetMax = new Vector2(m_BorderThickness / 2, m_BorderThickness / 2);
        //    }
        //    if (m_SouthBorderImg != null)
        //    {
        //        m_SouthBorderImg.color = m_BorderColor;
        //    }



        //    if (WestBorder != null)
        //    {
        //        WestBorder.anchorMin = new Vector2(0, 0);
        //        WestBorder.anchorMax = new Vector2(0, 1);
        //        WestBorder.offsetMin = new Vector2(-m_BorderThickness / 2, -m_BorderThickness / 2);
        //        WestBorder.offsetMax = new Vector2(m_BorderThickness / 2, m_BorderThickness / 2);
        //    }
        //    if (m_WestBorderImg != null)
        //    {
        //        m_WestBorderImg.color = m_BorderColor;
        //    }



        //    if (EastBorder != null)
        //    {
        //        EastBorder.anchorMin = new Vector2(1, 0);
        //        EastBorder.anchorMax = new Vector2(1, 1);
        //        EastBorder.offsetMin = new Vector2(-m_BorderThickness / 2, -m_BorderThickness / 2);
        //        EastBorder.offsetMax = new Vector2(m_BorderThickness / 2, m_BorderThickness / 2);
        //    }
        //    if (m_EastBorderImg != null)
        //    {
        //        m_EastBorderImg.color = m_BorderColor;
        //    }

        //    // --

        //    if (m_Walls != null) 
        //        m_Walls.SetActive(m_ShowWalls);

        //    if (NorthWall != null)
        //    {
        //        NorthWall.gameObject.SetActive(tile.North);

        //        NorthWall.pivot = new Vector2(0.5f, 0.5f);
        //        NorthWall.anchorMin = new Vector2(0, 1);
        //        NorthWall.anchorMax = new Vector2(1, 1);
        //        NorthWall.offsetMin = new Vector2(-m_WallsThickness / 2, -m_WallsThickness / 2);
        //        NorthWall.offsetMax = new Vector2(m_WallsThickness / 2, m_WallsThickness / 2);
        //    }
        //    if (m_NorthWallImg != null)
        //    {
        //        m_NorthWallImg.color = m_WallsColor;
        //    }



        //    if (SouthWall != null)
        //    {
        //        SouthWall.gameObject.SetActive(tile.South);

        //        SouthWall.anchorMin = new Vector2(0, 0);
        //        SouthWall.anchorMax = new Vector2(1, 0);
        //        NorthWall.offsetMin = new Vector2(-m_WallsThickness / 2, -m_WallsThickness / 2);
        //        NorthWall.offsetMax = new Vector2(m_WallsThickness / 2, m_WallsThickness / 2);
        //    }
        //    if (m_SouthWallImg != null)
        //    {
        //        m_SouthWallImg.color = m_WallsColor;
        //    }



        //    if (WestWall != null)
        //    {
        //        WestWall.gameObject.SetActive(tile.West);

        //        WestWall.anchorMin = new Vector2(0, 0);
        //        WestWall.anchorMax = new Vector2(0, 1);
        //        NorthWall.offsetMin = new Vector2(-m_WallsThickness / 2, -m_WallsThickness / 2);
        //        NorthWall.offsetMax = new Vector2(m_WallsThickness / 2, m_WallsThickness / 2);
        //    }
        //    if (m_WestWallImg != null)
        //    {
        //        m_WestWallImg.color = m_WallsColor;
        //    }



        //    if (EastWall != null)
        //    {
        //        EastWall.gameObject.SetActive(tile.East);

        //        EastWall.anchorMin = new Vector2(1, 0);
        //        EastWall.anchorMax = new Vector2(1, 1);
        //        NorthWall.offsetMin = new Vector2(-m_WallsThickness / 2, -m_WallsThickness / 2);
        //        NorthWall.offsetMax = new Vector2(m_WallsThickness / 2, m_WallsThickness / 2);
        //    }
        //    if (m_EastWallImg != null)
        //    {
        //        m_EastWallImg.color = m_WallsColor;
        //    }
        //}
    }
}
