using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public sealed class DragSession
{
    public object Payload { get; }
    public Vector2 ScreenPosition { get; internal set; }
    public DragSource Source { get; internal set; }
    public DropTarget HoverTarget { get; internal set; }
    public bool HasProxy => Proxy != null;
    public IDragProxy Proxy { get; internal set; }

    internal DragSession(DragSource source, object payload)
    {
        Source = source;
        Payload = payload;
    }
}
