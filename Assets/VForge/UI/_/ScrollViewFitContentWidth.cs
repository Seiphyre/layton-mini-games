using UnityEngine.UI;
using UnityEngine;
using System.Collections;

[ExecuteAlways]
[RequireComponent(typeof(LayoutElement))]
public class ScrollViewFitContentWidth : MonoBehaviour
{
    [SerializeField] RectTransform content;

    LayoutElement layout;

    void Awake()
    {
        Ensure();
        UpdateWidth();
    }

    IEnumerator Start()
    {
        yield return null;
        Ensure();
        UpdateWidth();
    }

    void OnEnable()
    {
        Ensure();
        UpdateWidth();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        // Called when values change in Inspector
        Ensure();
        UpdateWidth();
    }
#endif

    void Ensure()
    {
        if (layout == null)
            layout = GetComponent<LayoutElement>();
    }

    void UpdateWidth()
    {
        if (content == null || layout == null)
            return;

        // Force content layout update
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        // Apply preferred width
        layout.preferredWidth = content.rect.width;

#if UNITY_EDITOR
        // Ensure Scene view refresh
        if (!Application.isPlaying)
            UnityEditor.EditorUtility.SetDirty(layout);
#endif
    }
}