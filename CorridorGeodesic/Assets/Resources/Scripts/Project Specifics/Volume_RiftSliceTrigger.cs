//===================== (Neverway 2024) Written by Errynei ===================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Volume_RiftSliceTrigger : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public enum RiftIntersection { AnyPartInsideRift, FullyEncapsulatedOnly, PartiallyEncapsulatedOnly }
    public RiftIntersection riftIntersectionType;

    //=-----------------=
    // Private Variables
    //=-----------------=
    private CorGeo_ActorData[] corners;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public bool IsInsideRift()
    {
        CorGeo_ActorData.Space currentSpace = corners[0].space;
        //if (currentSpace.)

        switch (riftIntersectionType)
        {
            case RiftIntersection.AnyPartInsideRift:
                {
                    
                    foreach (CorGeo_ActorData c in corners)
                    {
                        if (c.space != currentSpace)
                            return true;
                    }
                    return currentSpace == CorGeo_ActorData.Space.Null;
                }
                break;
            case RiftIntersection.FullyEncapsulatedOnly:
                {
                    foreach (CorGeo_ActorData c in corners)
                    {

                    }
                }
                break;
            case RiftIntersection.PartiallyEncapsulatedOnly:
                {
                    foreach (CorGeo_ActorData c in corners)
                    {

                    }
                }
                break;
        }
        return false;
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private new void Awake()
    {
        base.Awake();
        corners = GetComponentsInChildren<CorGeo_ActorData>();
    }
    private void Update()
    {
        if (Alt_Item_Geodesic_Utility_GeoGun.deployedRift)
        {
            

            
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
