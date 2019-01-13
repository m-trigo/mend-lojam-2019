using UnityEngine;

public class SelectionScript : MonoBehaviour
{
    /* Inspector */

    /* Life Cycle */

    private void OnMouseDown()
    {
        GetComponentInParent<GridScript>().SetActiveTile( gameObject );
    }

    private void OnMouseUp()
    {
        alreadyMoved = false;
    }

    private void OnMouseDrag()
    {
        if ( alreadyMoved )
        {
            return;
        }

        Vector2 mouse = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        float dragDistance = Vector2.Distance( mouse, gameObject.transform.position );
        if ( dragDistance > MIN_DRAG_DISTANCE )
        {
            alreadyMoved = true;
            GridScript grid = GetComponentInParent<GridScript>();

            Vector2 dragVector = mouse - ( Vector2 ) gameObject.transform.position;
            if ( Mathf.Abs( dragVector.x ) > Mathf.Abs( dragVector.y ) )
            {
                if ( dragVector.x > 0 )
                {
                    grid.Move( Direction.RIGHT );
                }
                else
                {
                    grid.Move( Direction.LEFT );
                }
            }
            else
            {
                if ( dragVector.y > 0 )
                {
                    grid.Move( Direction.UP );
                }
                else
                {
                    grid.Move( Direction.DOWN );
                }
            }
        }
    }

    /* Public */

    /* Private */

    private const float MIN_DRAG_DISTANCE = 1f;

    private bool alreadyMoved = false;
}
