//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;

public class Fizzler : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Transform fizzlerObjectToDisable;
    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent disableFizzler;

    [IsDomainReloaded] private static Pawn_WeaponInventory geoGun;

    //=-----------------=
    // Reference Variables
    //=-----------------=

    //=-----------------=
    // Mono Functions
    //=-----------------=
    public void Update()
    {
        bool disabled = (disableFizzler != null && disableFizzler.isPowered);
        isPowered = !disabled;
        if (fizzlerObjectToDisable != null)
            fizzlerObjectToDisable.gameObject.SetActive(!disabled);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=

    //Called by a UnityEvent
    public void ClearGeoGunRifts()
    {
        if (geoGun == null)
        {
            geoGun = FindObjectOfType<Pawn_WeaponInventory>();
            if (geoGun == null)
            {
                Debug.LogError("Fizzler could not find geoGun for some reason? Cant clear rift");
                return;
            }
        }
        geoGun.ClearGeoGunRifts();
    }

    //=----Reload Static Fields----=
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeStaticFields()
    {
        geoGun = null;
    }
}
