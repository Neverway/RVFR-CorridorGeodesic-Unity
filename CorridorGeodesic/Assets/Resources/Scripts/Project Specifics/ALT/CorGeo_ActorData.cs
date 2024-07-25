//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorGeo_ActorData : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [HideInInspector] public Vector3 homePosition;
    [HideInInspector] public Vector3 homeScale;
    [HideInInspector] public Transform homeParent;
    [HideInInspector] public bool nullSpace = false;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        homePosition = transform.position;
        homeScale = transform.localScale;
        homeParent = transform.parent;
        ALTItem_Geodesic_Utility_GeoFolder.CorGeo_ActorDatas.Add(this);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    public void GoHome ()
    {
        transform.SetParent(homeParent);
        transform.localScale = homeScale;
        if (nullSpace)
        {
            transform.position = homePosition;
            return;
        }
        if (ALTItem_Geodesic_Utility_GeoFolder.plane1.GetDistanceToPoint (transform.position) > 0)
        {
            transform.position += ALTItem_Geodesic_Utility_GeoFolder.deployedRift.transform.forward * ALTItem_Geodesic_Utility_GeoFolder.riftWidth;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
