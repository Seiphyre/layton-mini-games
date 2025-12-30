using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class DragProxyFactory : ScriptableObject, IDragProxyFactory
{
    public abstract IDragProxy Create(object payload, Transform proxyRoot);
}