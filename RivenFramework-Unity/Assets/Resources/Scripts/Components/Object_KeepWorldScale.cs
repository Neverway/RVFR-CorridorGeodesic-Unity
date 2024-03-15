//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_KeepWorldScale : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector3 initialScale;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    void Start()
    {
        // Record the initial scale of the sprite
        initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        // Get the parent's scale
        Vector3 parentScale = transform.parent.localScale;

        // Calculate the inverse scale factor
        Vector3 inverseScale = new Vector3(
            1f / parentScale.x,
            1f / parentScale.y,
            1f / parentScale.z
        );

        // Apply the inverse scale to the sprite
        transform.localScale = Vector3.Scale(initialScale, inverseScale);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
