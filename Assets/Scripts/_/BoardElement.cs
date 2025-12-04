using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardElement
{
    [field: SerializeField] public PieceData Item { get; set; }
    [field: SerializeField] public int X { get; set; }
    [field: SerializeField] public int Y { get; set; }
}
