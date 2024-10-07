//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn_WeaponInventory : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Alt_Item_Geodesic_Utility_GeoGun geoGun;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
    }

    private void Update()
    {
    
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    //=-----------------=
    // External Functions
    //=-----------------=
    public void GiveGeoGun()
    {
        geoGun.gameObject.SetActive(true);
    }
    
    public void TakeGeoGun()
    {
        geoGun.gameObject.SetActive(false);
    }
    
    public void UpgradeGeoGun()
    {
        geoGun.allowExpandingRift = true;
    }
    
    public void ClearGeoGunRifts()
    {
        geoGun.StartRecallInfinityMarkers();
    }
}
