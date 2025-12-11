using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the walls surrounding a tile.
/// Pure data container: no logic.
/// </summary>
[Serializable]
public class TileWalls
{
    public bool North;
    public bool South;
    public bool East;
    public bool West;

    /// <summary>
    /// True if any wall exists on this tile.
    /// </summary>
    public bool HasAny => North || South || East || West;

    /// <summary>
    /// Get a wall state by direction enum.
    /// </summary>
    public bool Get(TileEdge tileEdge)
    {
        return tileEdge switch
        {
            TileEdge.North => North,
            TileEdge.South => South,
            TileEdge.East => East,
            TileEdge.West => West,
            _ => false
        };
    }

    /// <summary>
    /// Set a wall state by direction enum.
    /// </summary>
    public void Set(TileEdge tileEdge, bool value)
    {
        switch (tileEdge)
        {
            case TileEdge.North: North = value; break;
            case TileEdge.South: South = value; break;
            case TileEdge.East: East = value; break;
            case TileEdge.West: West = value; break;
        }
    }



    // --------------------------------
    // Constructors
    // --------------------------------

    public TileWalls() { }

    public TileWalls(TileWalls other)
    {
        North = other.North;
        South = other.South;
        East = other.East;
        West = other.West;
    }



    // --------------------------------
    // Utils
    // --------------------------------

    public TileWalls Clone() => new TileWalls(this);
}
