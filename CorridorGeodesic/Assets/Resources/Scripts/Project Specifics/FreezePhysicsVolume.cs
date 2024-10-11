using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePhysicsVolume : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string tagToFreeze = "PhysProp";
    public LogicComponent doFreezing;

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

                if (isPowered)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
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
    protected new void OnTriggerExit(Collider _other)
    {
        base.OnTriggerExit(_other);

        // An Pawn has exited the trigger
        if (_other.CompareTag(tagToFreeze))
        {
            Rigidbody rb = _other.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }
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
