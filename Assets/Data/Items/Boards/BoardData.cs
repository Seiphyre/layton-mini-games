using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardData")]
public class BoardData : ScriptableObject
{
    [Range(2, 10)] public int GridColCount = 5;
    [Range(2, 10)] public int GridRowCount = 5;

    [SerializeField] private Tile[] _tiles;
    public Tile[] Tiles
    {
        get
        {
            if (_tiles == null || _tiles.Length != GridCellCount)
            {
                Tile[] newTiles = new Tile[GridCellCount];

                for (int i = 0; i < GridCellCount; i++)
                    newTiles[i] = (_tiles != null && i < _tiles.Length) ? _tiles[i] : new Tile();


                _tiles = newTiles;
            }

            return _tiles;
        }

        set { _tiles = value; }
    }

    public int GridCellCount => GridColCount * GridRowCount;



    // -------------------------------------------------------------

    public int Id(int x, int y)
    {
        if (x < 0 || x >= GridColCount)
            return -1;

        if (y < 0 || y >= GridRowCount)
            return -1;

        return (y * GridColCount + x);
    }

    // --

    public Tile GetTile(int x, int y) => Tiles.ElementAtOrDefault(Id(x, y));

    // --

    public void SetTile(int x, int y, Tile tile) => Tiles[Id(x, y)] = tile;

}
