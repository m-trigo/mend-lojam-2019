using UnityEngine;

public class GridScript : MonoBehaviour
{
    /* Inspector */

    [SerializeField]
    private GameObject edgePrefab = null;

    [SerializeField]
    private GameObject[] tiles = null;

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

        CreateAt( tiles[0], SIZE / 2, SIZE / 2 );
        CreateAt( tiles[1], SIZE / 2 - 1, SIZE / 2 );
        CreateAt( tiles[2], SIZE / 2 - 2, SIZE / 2 );
        CreateAt( tiles[3], SIZE / 2 + 1, SIZE / 2 );

        activeTile_ = grid_[ SIZE / 2, SIZE / 2 ];
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

    public void SetActiveTile( GameObject gameObject )
    {
        activeTile_ = gameObject;
    }

    /* Private */

    private GameObject activeTile_ = null;
    private Coordinate ActiveTileCoordinate
    {
        get
        {
            for ( int x = 0; x < SIZE; x++ )
            {
                for ( int y = 0; y < SIZE; y++ )
                {
                    if ( activeTile_.Equals( grid_[ x, y ] ) )
                    {
                        return new Coordinate() { x = x, y = y };
                    }
                }
            }

            return new Coordinate();
        }
    }

    private GameObject[,] grid_ = null;

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
        Coordinate d = ActiveTileCoordinate;
        switch ( direction )
        {
            case Direction.UP:
                for ( int y = d.y + 1; y < SIZE; y++ )
                {
                    if ( grid_[ d.x, y ] != null )
                    {
                        break;
                    }

                    d.y = y;
                }

                break;

            case Direction.DOWN:
                for ( int y = d.y - 1; y > 0; y-- )
                {
                    if ( grid_[ d.x, y ] != null )
                    {
                        break;
                    }
                    d.y = y;
                }
                break;

            case Direction.RIGHT:
                for ( int x = d.x + 1; x < SIZE; x++ )
                {
                    if ( grid_[ x, d.y ] != null )
                    {
                        break;
                    }

                    d.x = x;
                }
                break;

            case Direction.LEFT:
                for ( int x = d.x - 1; x > 0; x-- )
                {
                    if ( grid_[ x, d.y ] != null )
                    {
                        break;
                    }
                    d.x = x;
                }
                break;
        }

        return d;
    }

    private void move( Direction direction )
    {
        Coordinate oldCoordinate = ActiveTileCoordinate;
        Coordinate newCoordinate = destination( direction );
        if ( !oldCoordinate.Equals( newCoordinate ) )
        {
            PlaceAt( activeTile_, newCoordinate );
            grid_[ oldCoordinate.x, oldCoordinate.y ] = null;
        }
    }
}
