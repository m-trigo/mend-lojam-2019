using System;
using System.Collections.Generic;

class Tileset
{

    public Tileset()
    {
        Dictionary<char, Tile> tiles = new Dictionary<char, Tile>();
        tiles.Add('A', new Tile());
        tiles.Add('B', new Tile());
        tiles.Add('C', new Tile());
        tiles.Add('D', new Tile());
        Tiles = tiles;
    }

    public Dictionary<char, Tile> Tiles { get; set; }
}
