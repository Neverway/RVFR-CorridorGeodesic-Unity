//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Geodesic_Utility_GeoFolder : Item_Geodesic_Utility
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public int maxAmmo=2;
    public int currentAmmo=2;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private GameObject vacuumProjectile;
    [SerializeField] private GameObject riftObject;
    [SerializeField] private float projectileForce;
    public List<GameObject> deployedInfinityMarkers = new List<GameObject>();
    private GameObject deployedRift;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public override void UsePrimary()
    {
        DeployInfinityMarker();
    }
    
    public override void UseSecondary()
    {
        ConvergeInfinityMarkers();
    }
    
    public override void UseSpecial()
    {
        RecallInfinityMarkers();
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void RecallInfinityMarkers()
    {
        var projectiles = FindObjectsOfType<VacuumProjectile>();
        foreach (var projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }
        if (deployedRift) Destroy(deployedRift);
        currentAmmo = maxAmmo;
        deployedInfinityMarkers.Clear();
    }
    private void DeployInfinityMarker()
    {
        if (currentAmmo <= 0) return;
        currentAmmo--;
        var projectile = Instantiate(vacuumProjectile, barrelTransform.transform.position, barrelTransform.transform.rotation, null);
        projectile.GetComponent<Rigidbody>().AddForce(transform.forward * -projectileForce, ForceMode.Impulse);
        projectile.GetComponent<VacuumProjectile>().geoFolder = this; // Get a reference to the gun that spawned the projectile, so we know who to give ammo to on a lifetime expiration
        deployedInfinityMarkers.Add(projectile);
    }

    private void ConvergeInfinityMarkers()
    {
        if (deployedInfinityMarkers.Count >= 2)
        {
            if (!deployedRift)
            {
                Vector3 midPoint = (deployedInfinityMarkers[0].transform.position + deployedInfinityMarkers[1].transform.position) * 0.5f;
                deployedRift = Instantiate(riftObject, midPoint, new Quaternion());
                deployedRift.transform.LookAt(deployedInfinityMarkers[0].transform);
                deployedRift.transform.rotation = new Quaternion(0, deployedRift.transform.rotation.y, 0, deployedRift.transform.rotation.w);
                deployedRift.GetComponent<Rift>().distanceToMarker = Vector3.Distance(deployedInfinityMarkers[0].transform.position, deployedInfinityMarkers[1].transform.position) * 0.5f;
            }
            else
            {
                deployedRift.GetComponent<Rift>().DivergePortals(5, 0.25f);
            }
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
