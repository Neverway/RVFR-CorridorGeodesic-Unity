//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.LogicSystem;

public class Func_GeoGunGiver : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent inputSignal;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    //public override void AutoSubscribe()
    //{
    //    subscribeLogicComponents.Add(inputSignal);
    //    base.AutoSubscribe();
    //}
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        isPowered = powered;

        if (isPowered)
        {
            GiveGeoGun();
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    //=-----------------=
    // External Functions
    //=-----------------=
    public void GiveGeoGun()
    {
        FindObjectOfType<Pawn_WeaponInventory>().GiveGeoGun();
    }
}
