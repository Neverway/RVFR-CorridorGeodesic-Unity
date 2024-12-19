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
using Neverway.Framework.LogicSystem;

public class Volume_RiftSliceTrigger : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public enum RiftIntersection { AnyPartInsideRift, NoPartInsideRift, FullyEncapsulatedOnly, PartiallyEncapsulatedOnly, RiftActiveAtAll }
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

        if (currentSpace == CorGeo_ActorData.Space.None)
            return false;

        switch (riftIntersectionType)
        {
            case RiftIntersection.RiftActiveAtAll:
                {
                    return true;
                }
            case RiftIntersection.AnyPartInsideRift:
                {
                    foreach (CorGeo_ActorData c in corners)
                    {
                        if (c.space != currentSpace)
                            return true;
                    }
                    return currentSpace == CorGeo_ActorData.Space.Null;
                }
            case RiftIntersection.NoPartInsideRift:
                {
                    foreach (CorGeo_ActorData c in corners)
                    {
                        if (c.space != currentSpace || c.space == CorGeo_ActorData.Space.Null)
                            return false;
                    }
                    return true;
                }
            case RiftIntersection.FullyEncapsulatedOnly:
                {
                    foreach (CorGeo_ActorData c in corners)
                    {
                        if (c.space != CorGeo_ActorData.Space.Null)
                            return false;
                    }
                    return true;
                }
            case RiftIntersection.PartiallyEncapsulatedOnly:
                {
                    bool inSpaceA = false;
                    bool inSpaceB = false;
                    bool inSpaceNull = false;
                    foreach (CorGeo_ActorData c in corners)
                    {
                        inSpaceA = inSpaceA || c.space == CorGeo_ActorData.Space.A;
                        inSpaceB = inSpaceB || c.space == CorGeo_ActorData.Space.B;
                        inSpaceNull = inSpaceNull || c.space == CorGeo_ActorData.Space.Null;
                    }
                    return inSpaceNull && (inSpaceA ^ inSpaceB);
                }
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
        isPowered = IsInsideRift();
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
