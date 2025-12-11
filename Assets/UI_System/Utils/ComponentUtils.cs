using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ComponentUtils
{
    /// <summary>
    /// Assigns the component reference if it is null.
    /// Logs a warning if auto-assigned, error if not found.
    /// </summary>
    public static void AssignIfNull<TOwner, TComp>(TOwner owner, ref TComp field)
        where TOwner : Component
        where TComp : Component
    {
        if (field != null)
            return;

        field = owner.GetComponent<TComp>();

        if (field == null)
            Debug.LogWarning($"{typeof(TOwner).Name} expected a {typeof(TComp).Name} but none was assigned or found.", owner);
    }

    /// <summary>
    /// Returns the existing component of type T if found, otherwise adds it to the GameObject.
    /// </summary>
    public static void GetOrAddComponent<TOwner, TComp>(TOwner owner, ref TComp comp) 
        where TOwner : Component
        where TComp : Component
    {
        comp = owner.GetComponent<TComp>();
        if (comp == null)
            comp = owner.gameObject.AddComponent<TComp>();
    }

    public static TComp GetOrAddComponent<TComp>(GameObject owner)
        where TComp : Component
    {
        TComp comp = owner.GetComponent<TComp>();
        if (comp == null)
            comp = owner.AddComponent<TComp>();

        return comp;
    }
}
