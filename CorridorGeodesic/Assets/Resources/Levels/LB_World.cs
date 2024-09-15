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


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        gameInstance.UI_ShowHUD();
        if (shouldHaveGeoGun)
        {
            if (FindObjectOfType<Pawn_WeaponInventory>())
            {
                FindObjectOfType<Pawn_WeaponInventory>().GiveGeoGun();
            }
        }
        if (shouldHaveUpgradedGeoGund)
        {
            if (FindObjectOfType<Pawn_WeaponInventory>())
            {
                FindObjectOfType<Pawn_WeaponInventory>().UpgradeGeoGun();
            }
        }
    }
    private void Update()
    {
        if (shouldHaveGeoGun)
        {
            if (FindObjectOfType<Pawn_WeaponInventory>())
            {
                FindObjectOfType<Pawn_WeaponInventory>().GiveGeoGun();
            }
        }
        if (shouldHaveUpgradedGeoGund)
        {
            if (FindObjectOfType<Pawn_WeaponInventory>())
            {
                FindObjectOfType<Pawn_WeaponInventory>().UpgradeGeoGun();
            }
        }
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
