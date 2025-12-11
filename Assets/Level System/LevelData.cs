using BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Level", menuName = "Data/Level System/Level")]
public class LevelData : ScriptableObject
{
    [field: SerializeField] public List<PieceDefinition> Items { get; private set; }
    [field: SerializeField] public List<BoardElement> Purchases { get; private set; }
    [field: SerializeField] public BoardData BoardData { get; private set; }
}
