using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Tile
{
    public bool IsEmpty = true;

    public TileType Type = TileType.Wood;

    public bool North = false;
    public bool South = false;
    public bool East = false;
    public bool West = false;


    public Tile() { }

    public Tile(TileType type)
    {
        IsEmpty = false;
        Type = type;
    }

    //public bool IsEmpty => !Type.HasValue;

    public bool GetWall(WallDirection dir)
    {
        switch (dir)
        {
            case WallDirection.North: return North;
            case WallDirection.South: return South;
            case WallDirection.East: return East;
            case WallDirection.West: return West;

            default: return false;
        }
    }

    public void SetWall(WallDirection dir, bool value)
    {
        switch (dir)
        {
            case WallDirection.North: North = value; break;
            case WallDirection.South: South = value; break;
            case WallDirection.East: East = value; break;
            case WallDirection.West: West = value; break;
        }
    }
}
