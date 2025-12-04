using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ListData<T> : ScriptableObject
{
    [field: SerializeField] public List<T> Items { get; private set; } = new();
}
