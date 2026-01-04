using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour
{
    [Header("Proxy")]
    [SerializeField] private RectTransform proxyRoot; // DragLayer under a Canvas
    [SerializeField] private bool raycastTargetsFromChildren = true;

    public bool IsDragging => _session != null;
    public DragSession Current => _session;

    public event Action<DragSession> DragStarted;
    public event Action<DragSession> DragUpdated;
    public event Action<DragSession> DragDropped;
    public event Action<DragSession, DragCancelReason> DragCancelled;
    public event Action<DragSession> DragEnded;

    private DragSession _session;
    private IDragProxyFactory _proxyFactory;

    private readonly List<RaycastResult> _raycastResults = new(32);

    private void Awake()
    {
        if (proxyRoot == null)
            Debug.LogWarning($"{nameof(DragController)}: proxyRoot not set. Proxy will be disabled.");
    }

    // --------------------
    // Public API
    // --------------------

    public bool TryBeginDrag(DragSource source, object payload, IDragProxyFactory proxyFactory = null)
    {
        if (_session != null) return false;

        _session = new DragSession(source, payload);
        _proxyFactory = proxyFactory;

        _session.ScreenPosition = Input.mousePosition;

        if (_proxyFactory != null && proxyRoot != null)
        {
            _session.Proxy = _proxyFactory.Create(payload, proxyRoot);
            _session.Proxy?.SetScreenPosition(_session.ScreenPosition);
        }

        _session.HoverTarget = RaycastDropTarget(_session.ScreenPosition);

        DragStarted?.Invoke(_session);
        return true;
    }

    public void CancelDrag(DragCancelReason reason = DragCancelReason.CancelledByUser)
    {
        if (_session == null) return;

        DragCancelled?.Invoke(_session, reason);
        EndDragInternal();
    }

    // --------------------
    // Update loop
    // --------------------

    private void Update()
    {
        if (_session == null)
            return;

        _session.ScreenPosition = Input.mousePosition;

        _session.Proxy?.SetScreenPosition(_session.ScreenPosition);
        DragUpdated?.Invoke(_session);

        // hover target (optional, for feedback)
        _session.HoverTarget = RaycastDropTarget(_session.ScreenPosition);

        // end conditions
        if (Input.GetMouseButtonUp(0))
        {
            ResolveDropOrCancel();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelDrag(DragCancelReason.CancelledByUser);
        }
    }

    private void ResolveDropOrCancel()
    {
        var target = _session.HoverTarget;

        if (target == null)
        {
            DragCancelled?.Invoke(_session, DragCancelReason.ReleasedNoTarget);
            EndDragInternal();
            return;
        }

        if (!target.CanAccept(_session.Payload))
        {
            DragCancelled?.Invoke(_session, DragCancelReason.TargetRejected);
            EndDragInternal();
            return;
        }

        target.Accept(_session.Payload);
        DragDropped?.Invoke(_session);
        EndDragInternal();
    }

    private void EndDragInternal()
    {
        var ended = _session;

        ended.Proxy?.Destroy();

        _session = null;
        _proxyFactory = null;

        DragEnded?.Invoke(ended);
    }

    // --------------------
    // Raycast resolution
    // --------------------

    private DropTarget RaycastDropTarget(Vector2 screenPos)
    {
        var es = EventSystem.current;
        if (es == null) return null;

        var ped = new PointerEventData(es) { position = screenPos };

        _raycastResults.Clear();
        es.RaycastAll(ped, _raycastResults);

        for (int i = 0; i < _raycastResults.Count; i++)
        {
            var go = _raycastResults[i].gameObject;
            if (go == null) continue;

            // Prefer parent lookup so any child graphic can be a hit.
            var target = raycastTargetsFromChildren
                ? go.GetComponentInParent<DropTarget>()
                : go.GetComponent<DropTarget>();

            if (target != null && target.isActiveAndEnabled)
                return target;
        }

        return null;
    }
}
