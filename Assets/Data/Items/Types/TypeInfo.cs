using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemType")]
public class TypeInfo : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
}
