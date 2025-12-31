using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Runtime;
using VForge.Gameplay;
using VForge.Inventories;

[CreateAssetMenu(menuName = "VForge/Drag/Proxy Factories/Piece DragSource", fileName = "PieceProxyFactory")]
public class PieceDragProxyFactory : DragProxyFactory
{
    [SerializeField] private PieceDragView dragViewPrefab;




    public override IDragProxy Create(object payload, Transform proxyRoot)
    {
        PieceDefinition pieceDefinition = payload switch
        {
            InventoryItem<PieceDefinition> item => item.Data,
            PieceDefinition definition => definition,
            Piece piece => piece.Definition,
            _ => null
        };

        if (pieceDefinition == null)
        {
            Debug.LogError("PieceDragProxyFactory expects payload to be a InventoryItem<PieceDefinition>, PieceDefinition or Piece.");
            return null;
        }

        if (dragViewPrefab == null)
        {
            Debug.LogError("PieceDragProxyFactory doesn't have prefab.");
            return null;
        }

        // 1. Clone
        var dragView = Instantiate(dragViewPrefab, proxyRoot);
        dragView.name = $"{pieceDefinition.Id} (Drag Proxy)";

        dragView.Initialize(pieceDefinition);

        DisableInteraction(dragView.gameObject);
        ApplyVisualTweaks(dragView.gameObject);

        return new DefaultProxy(dragView.gameObject);
    }

    private void DisableInteraction(GameObject proxyGO)
    {
        // Disable DragSource itself
        foreach (var dragSource in proxyGO.GetComponentsInChildren<DragSource>())
            dragSource.enabled = false;

        // Disable raycast blocking (UI)
        foreach (var graphic in proxyGO.GetComponentsInChildren<Graphic>())
            graphic.raycastTarget = false;
    }

    private void ApplyVisualTweaks(GameObject proxyGO)
    {
        var canvasGroup = proxyGO.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = proxyGO.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0.9f;
    }
}
