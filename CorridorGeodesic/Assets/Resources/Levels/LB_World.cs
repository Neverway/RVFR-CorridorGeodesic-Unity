//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: 
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LB_World : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public bool shouldHaveGeoGun=true;
    public bool shouldHaveUpgradedGeoGund = false;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;
    private Pawn_WeaponInventory weaponInventory;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        gameInstance.UI_ShowHUD();
        UpdateGeoGunUpgrade();
    }
    private void Update()
    {
        UpdateGeoGunUpgrade();
    }

    public void UpdateGeoGunUpgrade()
    {
        if (weaponInventory == null)
        {
            weaponInventory = FindObjectOfType<Pawn_WeaponInventory>();
            if (weaponInventory == null)
            {
                Debug.LogWarning("Could not find " + nameof(Pawn_WeaponInventory) + " to update geogun");
                return;
            }
        }

        if (shouldHaveGeoGun)
            weaponInventory.GiveGeoGun();

        if (shouldHaveUpgradedGeoGund)
            weaponInventory.UpgradeGeoGun();
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
