//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Func_GeoGunFizzler : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private LogicComponent inputSignal;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void ClearGeoGunRifts()
    {
        FindObjectOfType<Pawn_WeaponInventory>().ClearGeoGunRifts();
    }
    public override void AutoSubscribe()
    {
        subscribeLogicComponents.Add(inputSignal);
        base.AutoSubscribe();
    }
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        isPowered = powered;

        if (isPowered)
            ClearGeoGunRifts();
    }
}
