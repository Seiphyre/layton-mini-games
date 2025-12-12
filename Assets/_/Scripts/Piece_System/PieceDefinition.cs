using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// - Stores:
///   - reference to ShapeInfo,
///   - reference to ColorInfo,
///   - reference to TypeInfo,
///   - maybe a sprite or model for UI.
///
/// </summary>
[CreateAssetMenu(fileName = "Piece", menuName = "Data/Piece System/Piece")]
public class PieceDefinition : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: Space]

    [field: SerializeField] public TagData Type { get; private set; }
    [field: SerializeField] public ColorData Color { get; private set; }
    [field: SerializeField] public ShapeData Shape { get; private set; }
}
