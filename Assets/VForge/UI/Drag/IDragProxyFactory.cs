using UnityEngine;

public interface IDragProxyFactory
{
    IDragProxy Create(object payload, Transform proxyRoot);
}