using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValidationUtils
{
    public static bool IsOfType<TType>(object value)
    {
        return value is TType;
    }
}
