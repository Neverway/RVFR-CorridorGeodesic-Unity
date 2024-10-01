//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume_PlatformMagnet : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private new void OnTriggerEnter(Collider _other)
    {
        base.OnTriggerEnter(_other); // Call the base class method
        if (_other.CompareTag("Pawn"))
        {
            if (_other.GetComponent<Pawn>())
            {
                _other.transform.SetParent(gameObject.transform);
                // TODO THIS IS A CORGEO FUNC, REMOVE ME LATER!!
                _other.GetComponent<CorGeo_ActorData>().isParentedIgnoreOffsets = true;
            }
        }
        if (_other.CompareTag("PhysProp"))
        {
            _other.transform.SetParent(gameObject.transform);
            // TODO THIS IS A CORGEO FUNC, REMOVE ME LATER!!
            _other.GetComponent<CorGeo_ActorData>().isParentedIgnoreOffsets = true;
        }
    }

    private new void OnTriggerExit(Collider _other)
    {
        base.OnTriggerExit(_other); // Call the base class method
        if (_other.CompareTag("Pawn"))
        {
            if (_other.GetComponent<Pawn>())
            {
                _other.transform.SetParent(null);
                // TODO THIS IS A CORGEO FUNC, REMOVE ME LATER!!
                _other.GetComponent<CorGeo_ActorData>().isParentedIgnoreOffsets = false;
            }
        }
        if (_other.CompareTag("PhysProp"))
        {
            _other.transform.SetParent(null);
            // TODO THIS IS A CORGEO FUNC, REMOVE ME LATER!!
            _other.GetComponent<CorGeo_ActorData>().isParentedIgnoreOffsets = false;
        }
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
