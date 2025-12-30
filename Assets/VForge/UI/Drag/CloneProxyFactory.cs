using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "VForge/Drag/Proxy Factories/Clone DragSource", fileName = "CloneProxyFactory")]
public sealed class CloneProxyFactory : DragProxyFactory
{
    [Header("Proxy Configuration")]
    [SerializeField] private bool disableRaycasts = true;
    [SerializeField] private float proxyAlpha = 0.9f;



    // -------------------------------------------------
    // IProxyFactory Interface API
    // -------------------------------------------------

    public override IDragProxy Create(object payload, Transform proxyRoot)
    {
        GameObject sourceGO = payload switch
        {
            GameObject go => go,
            Component component => component.gameObject,
            _ => null
        };

        if (sourceGO == null)
        {
            Debug.LogError("CloneDragFactory expects payload to be a gameobject or component.");
            return null;
        }

        var sourceRect = sourceGO.GetComponent<RectTransform>();

        if (sourceRect == null)
        {
            Debug.LogError("DragSource must have a RectTransform.");
            return null;
        }

        // 1. Clone
        var proxyGO = Instantiate(sourceGO, proxyRoot);
        proxyGO.name = $"{sourceGO.name} (Drag Proxy)";

        var proxyRect = proxyGO.GetComponent<RectTransform>();
        proxyRect.sizeDelta = sourceRect.rect.size;

        // 2. Disable interaction
        DisableInteraction(proxyGO);

        // 3. Set Visual
        ApplyVisualTweaks(proxyGO);

        return new CloneProxy(proxyGO);
    }



    // -------------------------------------------------
    // Internal Helpers
    // -------------------------------------------------

    private void DisableInteraction(GameObject proxyGO)
    {
        // Disable DragSource itself
        foreach (var dragSource in proxyGO.GetComponentsInChildren<DragSource>())
            dragSource.enabled = false;

        // Disable raycast blocking (UI)
        if (disableRaycasts)
        {
            foreach (var graphic in proxyGO.GetComponentsInChildren<Graphic>())
                graphic.raycastTarget = false;
        }
    }

    private void ApplyVisualTweaks(GameObject proxyGO)
    {
        if (proxyAlpha >= 0f && proxyAlpha < 1f)
        {
            var canvasGroup = proxyGO.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = proxyGO.AddComponent<CanvasGroup>();

            canvasGroup.alpha = proxyAlpha;
        }
    }
}