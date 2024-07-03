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
    [SerializeField] private float projectileForce;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public override void UsePrimary()
    {
        if (currentAmmo <= 0) return;
        currentAmmo--;
        var projectile = Instantiate(vacuumProjectile, barrelTransform.transform.position, barrelTransform.transform.rotation, null);
        projectile.GetComponent<Rigidbody>().AddForce(transform.forward * -projectileForce, ForceMode.Impulse);
        projectile.GetComponent<VacuumProjectile>().geoFolder = this; // Get a reference to the gun that spawned the projectile, so we know who to give ammo to on a lifetime expiration
    }
    
    public override void UseSecondary()
    {
        var projectiles = FindObjectsOfType<VacuumProjectile>();
        foreach (var projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }

        currentAmmo = maxAmmo;
    }
    
    public override void UseSpecial()
    {
        
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
