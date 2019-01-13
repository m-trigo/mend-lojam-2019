using System;
using System.Collections.Generic;

class Tile
{
    public Tile()
    {
        Coordinates = new List<Coordinate>();
    }

    public List<Coordinate> Coordinates { get; set; }
}
