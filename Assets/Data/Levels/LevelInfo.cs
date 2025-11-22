using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Level")]
public class LevelInfo : ScriptableObject
{
    [field: SerializeField] public List<ShopItem> Items { get; private set; }
    [field: SerializeField] public List<BoardElement> Purchases { get; private set; }
    [field: SerializeField] public BoardData BoardData { get; private set; }
}
