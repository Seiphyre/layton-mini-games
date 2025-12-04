using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// - Builds an in-memory representation of the board from BoardData.
/// - Knows which cells exist / are holes.
/// - Knows if there is a wall between two tiles.
/// - Exposes methods:
///   - IsTileActive(x, y)
///   - HasWallBetween(a, b)
///   - GetNeighbors(x, y)(respecting walls + holes)
///   
/// </summary>
/// 
public class Board
{

}
