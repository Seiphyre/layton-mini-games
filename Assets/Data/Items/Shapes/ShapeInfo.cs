using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shape")]
public class ShapeInfo : ScriptableObject
{
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
