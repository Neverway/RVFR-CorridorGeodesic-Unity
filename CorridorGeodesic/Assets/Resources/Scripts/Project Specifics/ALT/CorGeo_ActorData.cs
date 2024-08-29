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
    [ReadOnly] [SerializeField] public bool nullSpace = false;
    public event Action OnRiftRestore;

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
        if (TryGetComponent<Light> (out Light light))
        {
            activeInNullSpace = true;
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    public void GoHome ()
    {
        OnRiftRestore?.Invoke();
        gameObject.SetActive (true);
        transform.SetParent(homeParent);
        transform.localScale = homeScale;
        if (nullSpace)
        {
            transform.position = homePosition;
            return;
        }
        if (ALTItem_Geodesic_Utility_GeoFolder.plane1.GetDistanceToPoint (transform.position) > 0)
        if (ALTItem_Geodesic_Utility_GeoFolder.planeA.GetDistanceToPoint (transform.position) > 0)
        {
            if (!ALTItem_Geodesic_Utility_GeoFolder.deployedRift) return;
            //move actor away from collapse direction scaled by the rift timer's progress
            // move actor away from collapse direction scaled by the rift timer's progress
            transform.position += ALTItem_Geodesic_Utility_GeoFolder.deployedRift.transform.forward * 
                                  ALTItem_Geodesic_Utility_GeoFolder.riftWidth * 
                                  (ALTItem_Geodesic_Utility_GeoFolder.lerpAmount);
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
