using UnityEngine;

public class GridScript : MonoBehaviour
{
    /* Inspector */

    [SerializeField]
    private GameObject edgePrefab = null;

    [SerializeField]
    private GameObject testTilePrefab = null;

    /* Life Cycle */

    private void Awake()
    {
        grid_ = new GameObject[ SIZE, SIZE ];

        for ( int x = 0; x < SIZE; x++ )
        {
            CreateAt( edgePrefab, x, 0 );
            CreateAt( edgePrefab, x, SIZE - 1 );
        }

        for ( int y = 1; y < SIZE - 1; y++ )
        {
            CreateAt( edgePrefab, 0, y );
            CreateAt( edgePrefab, SIZE - 1, y );
        }

        CreateAt( testTilePrefab, SIZE / 2, SIZE / 2 );
        testTileCoordinate = new Coordinate()
        {
            x = SIZE / 2,
            y = SIZE / 2
        };
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.W ) )
        {
            move( Direction.UP );
        }
        else if ( Input.GetKeyDown( KeyCode.A ) )
        {
            move( Direction.LEFT );
        }
        else if ( Input.GetKeyDown( KeyCode.S ) )
        {
            move( Direction.DOWN );
        }
        else if ( Input.GetKeyDown( KeyCode.D ) )
        {
            move( Direction.RIGHT );
        }
    }

    /* Public */

    public const int SIZE = 10;

    /* Private */

    private enum Direction { UP, DOWN, LEFT, RIGHT };

    private struct Coordinate
    {
        public int x;
        public int y;

        public override string ToString()
        {
            return "[" + x + "," + y + "]";
        }

        public bool Equals( Coordinate coordinate )
        {
            return coordinate.x == x && coordinate.y == y;
        }
    }

    private Coordinate testTileCoordinate;

    private GameObject[,] grid_;
    private GameObject testTile => grid_[ testTileCoordinate.x, testTileCoordinate.y ];

    private Vector3 bottomLeftCorner => transform.localPosition - SIZE / 2 * Vector3.one;

    private void PlaceAt( GameObject gameObject, Coordinate coordinate )
    {
        gameObject.transform.localPosition = bottomLeftCorner + Vector3.one / 2 + new Vector3( coordinate.x, coordinate.y, 0 );
        grid_[ coordinate.x, coordinate.y ] = gameObject;
    }

    private void CreateAt( GameObject prefab, int x, int y )
    {
        CreateAt( prefab, new Coordinate() { x = x, y = y } );
    }

    private void CreateAt( GameObject prefab, Coordinate coordinate )
    {
        GameObject instance = Instantiate( prefab, transform );
        PlaceAt( instance, coordinate );
        instance.name = coordinate.ToString();
    }

    private Coordinate destination( Direction direction )
    {
        Coordinate d = testTileCoordinate;
        switch ( direction )
        {
            case Direction.UP:
                for ( int y = d.y; y < SIZE; y++ )
                {
                    if ( grid_[ d.x, y ] == null )
                    {
                        d.y = y;
                    }
                }
                break;

            case Direction.DOWN:
                for ( int y = d.y; y > 0; y-- )
                {
                    if ( grid_[ d.x, y ] == null )
                    {
                        d.y = y;
                    }
                }
                break;

            case Direction.RIGHT:
                for ( int x = d.x; x < SIZE; x++ )
                {
                    if ( grid_[ x, d.y ] == null )
                    {
                        d.x = x;
                    }
                }
                break;

            case Direction.LEFT:
                for ( int x = d.x; x > 0; x-- )
                {
                    if ( grid_[ x, d.y ] == null )
                    {
                        d.x = x;
                    }
                }
                break;
        }

        return d;
    }

    private void move(Direction direction)
    {
        Coordinate d = destination( direction );
        PlaceAt( testTile, d );

        if ( !testTileCoordinate.Equals(d) )
        {
            grid_[ testTileCoordinate.x, testTileCoordinate.y ] = null;
            testTileCoordinate = d;
        }
    }
}
