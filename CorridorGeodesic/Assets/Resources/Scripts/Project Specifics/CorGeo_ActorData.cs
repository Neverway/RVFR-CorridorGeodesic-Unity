//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CorGeo_ActorData : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("If enabled, this object will not be disabled in in fully collapsed null-space")]
    [SerializeField] public bool activeInNullSpace = false;
    [Tooltip("Uncheck this if the object has a special death animation")]
    public bool destroyedInKillTrigger=true;

    public bool debugLogData;
    
    [Header("Debugging")]
    [ReadOnly] [SerializeField] public Vector3 homePosition;
    [ReadOnly] [SerializeField] public Vector3 homeScale;
    [ReadOnly] [SerializeField] public Transform homeParent;
    [ReadOnly] [SerializeField] public bool dynamic = false;
    [ReadOnly] [SerializeField] public bool crushInNullSpace = true;
    [ReadOnly] [SerializeField] public bool isParentedIgnoreOffsets = false;

    /// <summary>
    /// Prevents actor from being moved during rift movement.
    /// Set during when actor is grabbed by Player.
    /// </summary>
    public bool isHeld = false;
    public event Action OnRiftRestore;

    public enum Space
    {
        None, A, B, Null
    }

    public Space space = Space.None;

    //=-----------------=
    // Private Variables
    //=-----------------=

    private Vector3 prevVelocity;

    //=-----------------=
    // Reference Variables
    //=-----------------=

    new private Rigidbody rigidbody;

    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        rigidbody= GetComponent<Rigidbody>();
        homePosition = transform.position;
        homeScale = transform.localScale;
        homeParent = transform.parent;
        Alt_Item_Geodesic_Utility_GeoGun.CorGeo_ActorDatas.Add(this);
        if (TryGetComponent<Light> (out Light light))
        {
            activeInNullSpace = true;
        }
    }

    private void OnDestroy ()
    {
        Alt_Item_Geodesic_Utility_GeoGun.CorGeo_ActorDatas.Remove(this);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    public void GoHome ()
    {
        OnRiftRestore?.Invoke();
        gameObject.SetActive(true); //todo: remember if object was active before crushing
        if (isParentedIgnoreOffsets) return;
        transform.SetParent(homeParent);
        transform.localScale = homeScale;
        if (space == Space.Null && !dynamic)
        {
            transform.position = homePosition;
            return;
        }
        if (space != Space.Null && Alt_Item_Geodesic_Utility_GeoGun.planeA.GetDistanceToPoint (transform.position) > 0)
        {
            if (!Alt_Item_Geodesic_Utility_GeoGun.deployedRift) return;
            //move actor away from collapse direction scaled by the rift timer's progress
            transform.position += Alt_Item_Geodesic_Utility_GeoGun.deployedRift.transform.forward *
                                    Alt_Item_Geodesic_Utility_GeoGun.riftWidth *
                                    (Alt_Item_Geodesic_Utility_GeoGun.lerpAmount);
        }
    }

    public void Freeze ()
    {
        if (rigidbody == null) return;

          prevVelocity = rigidbody.velocity;
        rigidbody.isKinematic = true;
    }

    public void UnFreeze ()
    {
        if (rigidbody == null) return;

        rigidbody.isKinematic = false;
        rigidbody.velocity = prevVelocity;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
