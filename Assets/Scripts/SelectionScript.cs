using DG.Tweening;
using UnityEngine;

public class SelectionScript : MonoBehaviour
{
    /* Inspector */

    /* Life Cycle */

    private void OnMouseDown()
    {
        GetComponentInParent<GridScript>().SetActiveTile( gameObject );
        dragStart = Camera.main.ScreenToWorldPoint( Input.mousePosition );
    }

    private void OnMouseUp()
    {
        alreadyMoved = false;
        Vector2 mouse = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        Vector2 dragVector = mouse - ( Vector2 ) dragStart;
        if ( dragVector.magnitude < MIN_DRAG_DISTANCE && dragVector.magnitude > 0.1f )
        {
            if ( Mathf.Abs( dragVector.x ) > Mathf.Abs( dragVector.y ) )
            {
                float x = transform.position.x;
                int dx = 1;
                if ( dragVector.x < 0 )
                {
                    dx = -1;
                }

                transform.DOMoveX( x + dx, 0.1f ).SetEase( Ease.OutFlash ).OnComplete( () =>
                {
                    transform.DOMoveX( x, 0.3f ).SetEase( Ease.OutBack );
                } );

            }
            else
            {
                float y = transform.position.y;
                int dy = 1;
                if ( dragVector.y < 0 )
                {
                    dy = -1;
                }

                transform.DOMoveY( y + dy, 0.1f ).SetEase( Ease.OutFlash ).OnComplete( () =>
                {
                    transform.DOMoveY( y, 0.3f ).SetEase( Ease.OutBack );
                } );
            }
        }
    }

    private void OnMouseDrag()
    {
        if ( alreadyMoved )
        {
            return;
        }

        Vector2 mouse = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        Vector2 dragVector = mouse - ( Vector2 ) dragStart;
        if ( dragVector.magnitude > MIN_DRAG_DISTANCE )
        {
            alreadyMoved = true;
            GridScript grid = GetComponentInParent<GridScript>();

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

    private const float MIN_DRAG_DISTANCE = 0.75f;

    private Vector3 dragStart;
    private bool alreadyMoved = false;
}
