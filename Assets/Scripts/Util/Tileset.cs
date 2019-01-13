using System.Collections.Generic;

class Tileset
{

    public Tileset()
    {
        Dictionary<char, Tile> tiles = new Dictionary<char, Tile>
        {
            { 'A', new Tile() },
            { 'B', new Tile() },
            { 'C', new Tile() },
            { 'D', new Tile() }
        };
        Tiles = tiles;
    }

    public Dictionary<char, Tile> Tiles { get; set; }

    public Coordinate get( char letter )
    {
        return Tiles[ letter ].Coordinates[ 0 ];
    }
}
