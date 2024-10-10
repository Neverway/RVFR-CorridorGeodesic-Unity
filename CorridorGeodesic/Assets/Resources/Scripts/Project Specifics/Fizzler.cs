//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

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

    private static Pawn_WeaponInventory geoGun;

    //=-----------------=
    // Reference Variables
    //=-----------------=

    //=-----------------=
    // Mono Functions
    //=-----------------=
    public void Update()
    {
        bool disabled = (disableFizzler != null && disableFizzler.isPowered);
        fizzlerObjectToDisable.gameObject.SetActive(!disabled);
        isPowered = !disabled;
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
}
