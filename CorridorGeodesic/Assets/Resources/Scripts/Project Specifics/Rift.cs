//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rift : MonoBehaviour
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
    public GameObject portalA, portalB;
    public float distanceToMarker;
    

    //=-----------------=
    // Mono Functions
    //=-----------------=
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void DivergePortals(float distance, float speed)
    {
        portalA.transform.localPosition = Vector3.Lerp(portalA.transform.localPosition, -Vector3.forward*distanceToMarker, Time.deltaTime);
        portalB.transform.localPosition = Vector3.Lerp(portalB.transform.localPosition, Vector3.forward*distanceToMarker, Time.deltaTime);
    }
}
