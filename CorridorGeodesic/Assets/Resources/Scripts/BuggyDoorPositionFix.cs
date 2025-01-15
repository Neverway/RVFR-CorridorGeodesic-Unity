//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Physics doors seem to become offset from their root when sliced,
// this forces them to always be at the same position offset from their parent
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuggyDoorPositionFix : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private Vector3 positionOffsetFromParent;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
    }

    private void Update()
    {
        if (transform.localPosition != positionOffsetFromParent)
        {
            transform.localPosition = positionOffsetFromParent;
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
