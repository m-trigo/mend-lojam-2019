using System.Collections.Generic;
using System.Linq;

class FileParser
{
    public static Tileset Parse( string[] lines, int gridSize )
    {
        Tileset tileset = new Tileset();

        for ( int rowIndex = 0; rowIndex < lines.Length; ++rowIndex )
        {
            string currentLine = lines[ rowIndex ];
            for ( int colIndex = 0; colIndex < currentLine.Length; ++colIndex )
            {
                char currentChar = currentLine[ colIndex ];
                if ( char.IsLetter( currentChar ) )
                {
                    Coordinate coordinate = new Coordinate( colIndex, rowIndex );
                    tileset.Tiles[ currentChar ].Coordinates.Add( coordinate );
                }
            }
        }

        // copy of the dict keys - foreach won't ley you modify the dict otherwise
        List<char> pieceLetters = tileset.Tiles.Keys.ToList();

        // adjust the orientation to match the one of the game grid (bottom -> top, not top -> bottom)
        foreach ( char pieceLetter in pieceLetters )
        {
            Tile piece = tileset.Tiles[ pieceLetter ];
            for ( int tileIndex = 0; tileIndex < piece.Coordinates.Count; ++tileIndex )
            {
                Coordinate tileCoordinate = piece.Coordinates[ tileIndex ];
                tileCoordinate.y = gridSize - 1 - tileCoordinate.y;

                piece.Coordinates[ tileIndex ] = tileCoordinate;
            }

            tileset.Tiles[ pieceLetter ] = piece;
        }

        return tileset;
    }
}