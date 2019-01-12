using UnityEngine;

public class SelectionScript : MonoBehaviour
{
    /* Inspector */

    /* Life Cycle */

    private void OnMouseDown()
    {
        GetComponentInParent<GridScript>().SetSelectedTile( gameObject );
    }

    /* Public */

    /* Private */
}
