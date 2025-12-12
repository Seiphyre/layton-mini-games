using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectUtils
{
    public static string FindName(object obj)
    {
        if (obj == null)
            return "Empty";

        if (obj is ScriptableObject scriptableObject)
            return scriptableObject.name;

        if (obj is GameObject gameObject)
            return gameObject.name;

        return obj.ToString();
    }
}
