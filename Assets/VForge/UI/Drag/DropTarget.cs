using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DropTarget : MonoBehaviour
{
    private readonly List<Func<object, bool>> _rules = new();

    public event Action<object> Dropped;
    public event Action<object> Rejected;


    // -------------------------------------------------------
    // Validation API
    // -------------------------------------------------------

    public void AddRule(Func<object, bool> rule)
    {
        if (rule != null) _rules.Add(rule);
    }

    public void ClearRules() => _rules.Clear();



    // -------------------------------------------------------
    // Drop Behaviour API
    // -------------------------------------------------------

    public bool CanAccept(object payload)
    {
        for (int i = 0; i < _rules.Count; i++)
            if (!_rules[i](payload))
                return false;

        return true;
    }

    public void Accept(object payload)
    {
        if (!CanAccept(payload))
        {
            Rejected?.Invoke(payload);
            return;
        }

        Dropped?.Invoke(payload);
    }
}
