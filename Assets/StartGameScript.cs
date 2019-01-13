using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.W ) || Input.GetKeyDown( KeyCode.UpArrow )
            || Input.GetKeyDown( KeyCode.A ) || Input.GetKeyDown( KeyCode.LeftArrow )
            || Input.GetKeyDown( KeyCode.S ) || Input.GetKeyDown( KeyCode.DownArrow )
            || Input.GetKeyDown( KeyCode.D ) || Input.GetKeyDown( KeyCode.RightArrow )
            || Input.GetKeyDown( KeyCode.Space ) || Input.GetKeyDown( KeyCode.Return )
            || Input.GetKeyDown( KeyCode.Tab ) || Input.GetMouseButtonDown( 0 ) )
        {

            SceneManager.LoadScene( "Main" );
        }
    }
}
