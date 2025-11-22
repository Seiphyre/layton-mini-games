using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color")]
public class ColorInfo : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: Space]

    [field: SerializeField] public Color Color { get; private set; } = new Color(1, 1, 1, 1);
}
