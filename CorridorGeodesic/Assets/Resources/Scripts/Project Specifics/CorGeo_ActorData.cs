//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Used in Corridor Geodesic to handel [actor <-> rift] interactions
// Notes:
//
//=============================================================================

using System;
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
    public event Action OnRiftRestore;
    
    [Header("Debugging")]
    [ReadOnly] public Vector3 homePosition;
    [ReadOnly] public Vector3 homeScale;
    [ReadOnly] public Transform homeParent;
    [ReadOnly] public bool nullSpace = false;
    [ReadOnly] public bool dynamic = false;
    [ReadOnly] public bool crushInNullSpace = true;
    

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
        Item_Geodesic_Utility_NixieCross.CorGeo_ActorDatas.Add(this);
        
        // Hacky fix to make sure lights are always still visible even when in null-space
        // (Can't we just make sure the base Light prefab has this value enabled? ~Liz)
        if (TryGetComponent<Light> (out Light light))
        {
            activeInNullSpace = true;
        }
    }

    private void OnDestroy ()
    {
        Item_Geodesic_Utility_NixieCross.CorGeo_ActorDatas.Remove(this);
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
        if (Item_Geodesic_Utility_NixieCross.planeA.GetDistanceToPoint (transform.position) > 0)
        {
            if (!Item_Geodesic_Utility_NixieCross.deployedRift) return;
            // move actor away from collapse direction scaled by the rift timer's progress
            transform.position += Item_Geodesic_Utility_NixieCross.deployedRift.transform.forward * (Item_Geodesic_Utility_NixieCross.riftWidth * (Item_Geodesic_Utility_NixieCross.lerpAmount));
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
