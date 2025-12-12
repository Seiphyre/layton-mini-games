using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a piece type (e.g. clothing, hat, shoes, etc...).
/// </summary>
[CreateAssetMenu(fileName = "Tag", menuName = "Data/Piece System/Tag")]
public class TagData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
}
