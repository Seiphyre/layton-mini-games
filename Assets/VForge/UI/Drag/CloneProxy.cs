using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneProxy : IDragProxy
{
    public GameObject GameObject { get; }
    //public float Alpha { get; set; } = 1.0f;



    // -----------------------------------------------
    // Constructor
    // -----------------------------------------------

    public CloneProxy(GameObject go)
    {
        GameObject = go;
    }


    // -----------------------------------------------
    // IProxy Interface API
    // -----------------------------------------------

    public void SetScreenPosition(Vector2 screenPosition)
    {
        GameObject.transform.position = screenPosition;
    }

    public void Destroy()
    {
        Object.Destroy(GameObject);
    }

    public void Show()
    {
        GameObject.SetActive(true);
        //var canvasGroup = GameObject.GetComponent<CanvasGroup>();
        //if (canvasGroup == null)
        //{
        //    canvasGroup.alpha = Alpha;
        //}
    }

    public void Hide()
    {
        GameObject.SetActive(false);
        //var canvasGroup = GameObject.GetComponent<CanvasGroup>();
        //if (canvasGroup == null)
        //{
        //    canvasGroup.alpha = 0;
        //}
    }
}
