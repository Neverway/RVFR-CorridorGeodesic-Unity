using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePhysicsVolume : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string tagToFreeze = "PhysProp";
    [LogicComponentHandle] public LogicComponent doFreezing;

    //=-----------------=
    // Private Variables
    //=-----------------=

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=

    public void LateUpdate()
    {
        if (doFreezing == null)
        {
            Debug.LogError(nameof(FreezePhysicsVolume) + " needs an input");
            return;
        }
        isPowered = doFreezing.isPowered;

        bool riftNotMoving =
            Alt_Item_Geodesic_Utility_GeoGun.currentState != RiftState.Collapsing &&
            Alt_Item_Geodesic_Utility_GeoGun.currentState != RiftState.Expanding;

        foreach(GameObject prop in propsInTrigger)
        {
            Rigidbody rb = prop.GetComponent<Rigidbody>();
            if (rb != null && rb.velocity.magnitude < 0.025f)
            {
                rb.isKinematic = isPowered;
            }
        }
    }
    protected new void OnTriggerEnter(Collider _other)
    {
        //No need to include objects that arent a part of the tag
        if (!_other.CompareTag(tagToFreeze))
            return;

        base.OnTriggerEnter(_other);
    }
    protected new void OnDisable()
    {

    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
