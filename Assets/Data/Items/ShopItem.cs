using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class ShopItem : ScriptableObject
{
    [field: SerializeField] public ShapeInfo Shape { get; private set; }
    [field: SerializeField] public ColorInfo Color { get; private set; }
    [field: SerializeField] public TypeInfo Type { get; private set; }
}
