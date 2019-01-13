using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridScript : MonoBehaviour
{
    /* Inspector */

    #region PREFAB REFERENCES

    [Space]
    [Header( "Prefab References" )]

    public GameObject topLeftEdgePrefab;
    public GameObject topMiddleEdgePrefab;
    public GameObject topRightEdgePrefab;
    public GameObject bottomLeftEdgePrefab;
    public GameObject bottomMiddleEdgePrefab;
    public GameObject bottomRightEdgePrefab;
    public GameObject middleRightEdgePrefab;
    public GameObject middleLeftEdgePrefab;

    public GameObject[] tilePrefabs;

    #endregion

    #region ANIMATION PARAMETERS

    [Space]
    [Header( "Tile Movement" )]

    public float TileMovementSpeed;
    public Ease MovementEase;

    [Space]
    [Header( "Shake Animation" )]

    public float ShakeDuration;
    public float ShakeIntensity;
    public int ShakeVibratto;
    public float ShakeRandomness;

    [Space]
    [Header( "Victory Animation" )]

    public Ease GrowToFitEase;
    public float GrowthDuration;

    #endregion

    /* Life Cycle */

    private void Awake()
    {
        #region EDGE CREATION

        CreateAt( topLeftEdgePrefab, 0, SIZE - 1 );
        CreateAt( topRightEdgePrefab, SIZE - 1, SIZE - 1 );
        CreateAt( bottomLeftEdgePrefab, 0, 0 );
        CreateAt( bottomRightEdgePrefab, SIZE - 1, 0 );

        for ( int x = 1; x < SIZE - 1; x++ )
        {
            CreateAt( bottomMiddleEdgePrefab, x, 0 );
            CreateAt( topMiddleEdgePrefab, x, SIZE - 1 );
        }

        for ( int y = 1; y < SIZE - 1; y++ )
        {
            CreateAt( middleLeftEdgePrefab, 0, y );
            CreateAt( middleRightEdgePrefab, SIZE - 1, y );
        }

        #endregion

        #region TILE CREATION

        int center = SIZE / 2;

        Coordinate tile1Coordinates = new Coordinate( center - 2, center );
        Coordinate tile2Coordinates = new Coordinate( center - 1, center );
        Coordinate tile3Coordinates = new Coordinate( center + 0, center );
        Coordinate tile4Coordinates = new Coordinate( center + 1, center );

        CreateAt( tilePrefabs[ 0 ], tile1Coordinates ); // orange
        CreateAt( tilePrefabs[ 1 ], tile2Coordinates ); // pink
        CreateAt( tilePrefabs[ 2 ], tile3Coordinates ); // yellow
        CreateAt( tilePrefabs[ 3 ], tile4Coordinates ); // blue

        tiles_ = new List<GameObject>()
        {
            grid_[tile1Coordinates.x, tile1Coordinates.y],
            grid_[tile2Coordinates.x, tile2Coordinates.y],
            grid_[tile3Coordinates.x, tile3Coordinates.y],
            grid_[tile4Coordinates.x, tile4Coordinates.y],
        };

        #endregion
    }

    private void Update()
    {
        #region INPUT

        if ( Input.GetKeyDown( KeyCode.W ) )
        {
            Move( Direction.UP );
        }
        else if ( Input.GetKeyDown( KeyCode.UpArrow ) )
        {
            Move( Direction.UP );
        }
        else if ( Input.GetKeyDown( KeyCode.A ) )
        {
            Move( Direction.LEFT );
        }
        else if ( Input.GetKeyDown( KeyCode.LeftArrow ) )
        {
            Move( Direction.LEFT );
        }
        else if ( Input.GetKeyDown( KeyCode.S ) )
        {
            Move( Direction.DOWN );
        }
        else if ( Input.GetKeyDown( KeyCode.DownArrow ) )
        {
            Move( Direction.DOWN );
        }
        else if ( Input.GetKeyDown( KeyCode.D ) )
        {
            Move( Direction.RIGHT );
        }
        else if ( Input.GetKeyDown( KeyCode.RightArrow ) )
        {
            Move( Direction.RIGHT );
        }
        else if ( Input.GetKeyDown( KeyCode.Space ) )
        {
            CycleActiveTile();
        }
        else if ( Input.GetKeyDown( KeyCode.Return ) )
        {
            CycleActiveTile();
        }
        else if ( Input.GetKeyDown( KeyCode.Tab ) )
        {
            CycleActiveTile();
        }
        else if ( Input.GetKeyDown( KeyCode.R ) )
        {
            Restart();
        }

        #endregion
    }

    /* Public */

    public void SetActiveTile( GameObject tile )
    {
        activeTileIndex_ = tiles_.IndexOf( tile );
        Shake( tile );
    }

    /* Private */

    private const int SIZE = 10; // 8 x 8
    private const int VICTORY_SIZE = 2;

    private GameObject[,] grid_ = new GameObject[ SIZE, SIZE ];

    private List<GameObject> tiles_ = null;
    private int activeTileIndex_ = 0;

    private bool isMoving = false;

    private Vector3 BottomLeftCorner => transform.localPosition - SIZE / 2 * Vector3.one;

    private GameObject activeTile_ => tiles_[ activeTileIndex_ ];
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
                        return new Coordinate( x, y );
                    }
                }
            }

            return new Coordinate();
        }
    }

    private void CycleActiveTile()
    {
        SetActiveTile( tiles_[ ( activeTileIndex_ + 1 ) % tiles_.Count ] );
    }

    private bool IsTile( GameObject obj )
    {
        foreach ( GameObject tile in tiles_ )
        {
            if ( tile.Equals( obj ) )
            {
                return true;
            }
        }

        return false;
    }

    private bool IsMended()
    {
        for ( int bottomLeftX = 0; bottomLeftX < SIZE - VICTORY_SIZE; bottomLeftX++ )
        {
            for ( int bottomLeftY = 0; bottomLeftY < SIZE - VICTORY_SIZE; bottomLeftY++ )
            {
                int count = 0;

                for ( int x = bottomLeftX; x < SIZE && x < bottomLeftX + VICTORY_SIZE; x++ )
                {
                    for ( int y = bottomLeftY; y < SIZE && y < bottomLeftY + VICTORY_SIZE; y++ )
                    {
                        GameObject tile = grid_[ x, y ];
                        if ( tile != null && IsTile( tile ) )
                        {
                            count++;
                        }
                    }
                }

                if ( count == VICTORY_SIZE * VICTORY_SIZE )
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void Restart()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

    private void Shake( GameObject tile, TweenCallback onComplete = null )
    {
        tile.transform.localScale = Vector3.one;
        tile.transform.DOShakeScale( ShakeDuration, ShakeIntensity, ShakeVibratto, ShakeRandomness ).OnComplete( () => {
            tile.transform.localScale = Vector3.one;
            onComplete?.Invoke();
        } );
    }

    private void CheckVictory()
    {
        if ( IsMended() )
        {
            GameObject container = new GameObject( "container" );

            Vector3 p = Vector3.zero;
            foreach ( GameObject tile in tiles_ )
            {
                p += tile.transform.localPosition;
            }
            p = p / tiles_.Count;
            container.transform.position = p;

            foreach ( GameObject tile in tiles_ )
            {
                tile.transform.SetParent( container.transform );
            }

            container.transform.DOScale( 0, 1 ).SetEase( GrowToFitEase ).OnComplete( () => {
                container.transform.position = Vector3.zero;
                container.transform.DOScale( 4, GrowthDuration ).SetEase( GrowToFitEase );
            } );
        }
    }

    private void CreateAt( GameObject prefab, int x, int y )
    {
        GameObject instance = Instantiate( prefab, transform );
        instance.transform.localPosition = BottomLeftCorner + Vector3.one / 2 + new Vector3( x, y, 0 );
        grid_[ x, y ] = instance;
    }

    private void CreateAt( GameObject prefab, Coordinate coordinate )
    {
        CreateAt( prefab, coordinate.x, coordinate.y );
    }

    private Coordinate Destination( Direction direction )
    {
        Coordinate dest = ActiveTileCoordinate;
        switch ( direction )
        {
            case Direction.UP:
                for ( int y = dest.y + 1; y < SIZE; y++ )
                {
                    if ( grid_[ dest.x, y ] != null )
                    {
                        break;
                    }

                    dest.y = y;
                }

                break;

            case Direction.DOWN:
                for ( int y = dest.y - 1; y > 0; y-- )
                {
                    if ( grid_[ dest.x, y ] != null )
                    {
                        break;
                    }

                    dest.y = y;
                }
                break;

            case Direction.RIGHT:
                for ( int x = dest.x + 1; x < SIZE; x++ )
                {
                    if ( grid_[ x, dest.y ] != null )
                    {
                        break;
                    }

                    dest.x = x;
                }
                break;

            case Direction.LEFT:
                for ( int x = dest.x - 1; x > 0; x-- )
                {
                    if ( grid_[ x, dest.y ] != null )
                    {
                        break;
                    }

                    dest.x = x;
                }
                break;
        }

        return dest;
    }

    private void Move( Direction direction )
    {
        if ( isMoving || IsMended() )
        {
            return;
        }
        isMoving = true;

        Coordinate oldCoordinate = ActiveTileCoordinate;
        Coordinate newCoordinate = Destination( direction );
        if ( !oldCoordinate.Equals( newCoordinate ) )
        {
            Vector3 dest = BottomLeftCorner + Vector3.one / 2 + new Vector3( newCoordinate.x, newCoordinate.y, 0 );
            GameObject movingTile = activeTile_;

            float distanceToMove = Vector2.Distance( dest, movingTile.transform.position );
            float time = distanceToMove / TileMovementSpeed;

            movingTile.transform.DOMove( dest, time ).SetEase( MovementEase ).OnComplete( () => {
                foreach ( GameObject tile in tiles_ )
                {
                    float distance = Vector2.Distance( tile.transform.localPosition, movingTile.transform.localPosition );
                    if ( distance < 1.8f && distance > 0.2f )
                    {
                        Shake( tile );
                    }
                }

                Shake( movingTile, () => {
                    isMoving = false;
                    grid_[ newCoordinate.x, newCoordinate.y ] = movingTile;
                    grid_[ oldCoordinate.x, oldCoordinate.y ] = null;
                    CheckVictory();
                } );
            } );
        }
        else
        {
            isMoving = false;
        }
    }
}
