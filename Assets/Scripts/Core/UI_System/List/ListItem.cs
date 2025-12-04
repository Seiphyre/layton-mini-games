using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 
/// - Visual representation of one item in the list.
/// - Holds reference to underlying data (e.g. PieceData).
///
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class ListItem<T>
{
    public T Data;

    public GameObject GameObject;

    public Draggable Draggable;
    public UnityAction OnItemBeginDrag;
    public UnityAction<DropZone> OnItemEndDrag;
}
