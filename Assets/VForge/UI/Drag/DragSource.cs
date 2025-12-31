using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class DragSource : MonoBehaviour, IBeginDragHandler, IDragHandler
{

    [Header("Drag Configuration"), Space]
    [SerializeField] private bool createProxy = true;
    [SerializeField] private DragProxyFactory proxyFactory;

    [Space]

    [Header("Dependencies"), Space]
    [SerializeField] private DragController dragSystem;

    // --

    public object Payload { get; set; }
    public bool CreateProxy
    {
        get { return createProxy; }
        set { createProxy = value; }
    }
    public DragProxyFactory ProxyFactory
    {
        get { return proxyFactory; }
        set { proxyFactory = value; }
    }



    // ---------------------------------------------------------
    // Initialization
    // ---------------------------------------------------------

    public void Initialize(DragController system)
    {
        dragSystem = system ?? throw new ArgumentNullException(nameof(system));
    }

    public bool IsInitialized
    {
        get
        {
            return (dragSystem != null);
        }
    }



    // ---------------------------------------------------------
    // Drag API
    // ---------------------------------------------------------

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsInitialized)
            return;

        if (Payload == null)
            Payload = this;

        dragSystem.TryBeginDrag(this, Payload, createProxy ? proxyFactory : null);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Needed for IBeginDragHandler to work ...
    }
}
