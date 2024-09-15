//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Logic_Not : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField]
    [LogicComponentHandle]
    public LogicComponent inputSignal;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    //private void OnEnable()
    //{
    //    if (input)
    //        input.OnPowerStateChanged += SourcePowerStateChanged;
    //}
    //private void OnDestroy()
    //{
    //    if (input)
    //        input.OnPowerStateChanged -= SourcePowerStateChanged;
    //}
    private void Start()
    {
        if (!inputSignal)
        {
            isPowered = true;
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public override void AutoSubscribe()
    {
        subscribeLogicComponents.Add(inputSignal);
        base.AutoSubscribe();
    }
    public override void SourcePowerStateChanged(bool powered)
    {
        isPowered = !powered;
    }
}
