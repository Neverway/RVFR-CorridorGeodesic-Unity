//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rift_Audio : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=

    private bool bPlane = false;
    private Transform playerTransform;
    private CorGeo_ActorData playerActorData;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        Alt_Item_Geodesic_Utility_GeoGun.onStateChanged.AddListener (OnStateChanged);
        playerTransform = FindAnyObjectByType<Player>().transform;
        playerActorData = playerTransform.gameObject.GetComponent<CorGeo_ActorData>();
    }

    private void Update()
    {
        if (Alt_Item_Geodesic_Utility_GeoGun.currentState == RiftState.None)
        {
            return;
        }
        if (bPlane)
        {
            AlignBPlane ();
            return;
        }
        AlignAPlane ();
    }

    private void OnDestroy ()
    {
        Alt_Item_Geodesic_Utility_GeoGun.onStateChanged.RemoveListener (OnStateChanged);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private void OnStateChanged ()
    {
        if (Alt_Item_Geodesic_Utility_GeoGun.currentState != RiftState.None && Alt_Item_Geodesic_Utility_GeoGun.previousState == RiftState.None)
        {
            OnRiftCreated ();
        }

        //todo: Put cases here for when rift changes state (i.e. collapsing, expanding, etc.)
        switch (Alt_Item_Geodesic_Utility_GeoGun.currentState)
        {
            case RiftState.None:
                OnRiftRemoved ();
                break;
        }
    }

    private void AlignAPlane ()
    {
        transform.position = Alt_Item_Geodesic_Utility_GeoGun.planeA.ClosestPointOnPlane (playerTransform.position);
    }

    private void AlignBPlane ()
    {
        transform.position = Alt_Item_Geodesic_Utility_GeoGun.planeB.ClosestPointOnPlane (playerTransform.position);
    }

    private void OnRiftRemoved ()
    {
        //Put code here for when rift is cleared.
    }

    private void OnRiftCreated ()
    {
        //Put code here for when rift first starts moving
    }

    //=-----------------=
    // External Functions
    //=-----------------=

    public void OnSetup (bool _bPlane)
    {
        bPlane = _bPlane;
    }
}