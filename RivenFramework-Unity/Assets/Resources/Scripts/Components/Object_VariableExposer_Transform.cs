//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Exposes variables to control a GameObject's Transform properties.
// Notes: This script allows manipulation of scale and position offsets.
//
//=============================================================================

using UnityEngine;

public class Object_VariableExposer_Transform : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public float scaleX=1, scaleY=1, positionOffsetX, positionOffsetY;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector2 positionOrigin;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        positionOrigin = new Vector2(transform.position.x-positionOffsetX, transform.position.y-positionOffsetY);
    }

    private void Update()
    {
        transform.localScale = new Vector3(scaleX, scaleY, 1);
        transform.position = new Vector2(positionOrigin.x+positionOffsetX, positionOrigin.y+positionOffsetY);
    }

    
    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
