using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// - Stores the shape pattern
///   - e.g.list of local offsets: (0, 0), (1, 0), (0, 1)...
/// - Rotations may be handled here or externally
///
/// </summary>
[CreateAssetMenu(fileName = "Shape", menuName = "Data/Piece System/Shape")]
public class ShapeData : ScriptableObject
{
    public static int MaxWidth = 3;
    public static int MaxHeight = 3;

    [field: SerializeField] public string Name { get; private set; }

    [field: Space]

    [field: SerializeField] public Sprite NormalSprite1 { get; private set; }
    [field: SerializeField] public Sprite NormalSprite2 { get; private set; }
    [field: SerializeField] public Sprite SimplifiedSprite { get; private set; }
    [field: SerializeField] public Sprite InteractionSprite { get; private set; }

    [field: Space]

    [field: SerializeField] public int Width { get; private set; } = 1;
    [field: SerializeField] public int Height { get; private set; } = 1;
}
