//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumProjectile : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private float castRadius;
    [SerializeField] private Vector3 castOffset;
    [SerializeField] private float pinOffset;
    [HideInInspector] public bool pinned;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [HideInInspector] public GameObject geoFolder; // Get a reference to the gun that spawned the projectile, so we know who to give ammo to on a lifetime expiration
    private Rigidbody objectRigidbody;
    private RaycastHit faceHit;
    [SerializeField] private LayerMask layerMask;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();

        StartCoroutine(Lifetime());
    }

    private void Update()
    {
        if (pinned) return;
        CollisionTest();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void CollisionTest()
    {
        if (Physics.SphereCast(transform.position+castOffset, castRadius, transform.forward, out faceHit, layerMask))
        {
            // Disable physics
            objectRigidbody.isKinematic = true;
            
            // Set rotation to match face normal
            transform.rotation = Quaternion.LookRotation(-faceHit.normal);
            transform.position = faceHit.point;
            transform.localPosition += faceHit.normal*pinOffset;
            
            pinned = true;
        }
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(1);
        if (pinned) yield break; // Exit if the lamp has landed 
        // Give the projectile back to the weapon
        if (geoFolder)
        {
            if (geoFolder.GetComponent<ALTItem_Geodesic_Utility_GeoFolder>())
            {
                if (geoFolder.GetComponent<ALTItem_Geodesic_Utility_GeoFolder>().currentAmmo < geoFolder.GetComponent<ALTItem_Geodesic_Utility_GeoFolder>().maxAmmo)
                {
                    geoFolder.GetComponent<ALTItem_Geodesic_Utility_GeoFolder>().currentAmmo++;
                }

                geoFolder.GetComponent<ALTItem_Geodesic_Utility_GeoFolder>().deployedInfinityMarkers.Remove(gameObject);
            }
            if (geoFolder.GetComponent<Item_Geodesic_Utility_GeoFolder>())
            {
                if (geoFolder.GetComponent<Item_Geodesic_Utility_GeoFolder>().currentAmmo < geoFolder.GetComponent<Item_Geodesic_Utility_GeoFolder>().maxAmmo)
                {
                    geoFolder.GetComponent<Item_Geodesic_Utility_GeoFolder>().currentAmmo++;
                }

                geoFolder.GetComponent<Item_Geodesic_Utility_GeoFolder>().deployedInfinityMarkers.Remove(gameObject);
            }
        }
        // Erase the projectile
        Destroy(gameObject);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
