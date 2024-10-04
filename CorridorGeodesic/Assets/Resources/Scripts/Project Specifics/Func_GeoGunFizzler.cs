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
    public Transform fizzlerObject;

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent fizzleBulbsTrigger;
    [SerializeField, LogicComponentHandle] private LogicComponent disableFizzler;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public override void OnEnable()
    {
        //Causes glitch if SourcePowerChanged is called on frame 1
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void ClearGeoGunRifts()
    {
        Pawn_WeaponInventory nixieCross = FindObjectOfType<Pawn_WeaponInventory>();
        if(nixieCross)
            nixieCross.ClearGeoGunRifts();
    }
    //public override void AutoSubscribe()
    //{
    //    subscribeLogicComponents.Add(inputSignal);
    //    base.AutoSubscribe();
    //}
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        isPowered = disableFizzler == null || !disableFizzler.isPowered;

        //if (isPowered && fizzleBulbsTrigger.isPowered)
        //{
            ClearGeoGunRifts();
        //}
        //fizzlerObject.gameObject.SetActive(isPowered);
    }
}
