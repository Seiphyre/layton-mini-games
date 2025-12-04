using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// - Defines a logical color (not just a Unity.Color).
/// - Useful for matching rules (ex: red, blue, green…).
///
/// </summary>
[CreateAssetMenu(fileName = "Color", menuName = "Data/Piece System/Color")]
public class ColorData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: Space]

    [field: SerializeField] public Color Color { get; private set; } = new Color(1, 1, 1, 1);
}
