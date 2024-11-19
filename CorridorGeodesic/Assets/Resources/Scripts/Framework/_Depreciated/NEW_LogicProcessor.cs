//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class NEW_LogicProcessor : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("A readable value to tell if this is currently transmitting a power signal")]
    public bool isPowered;
    [HideInInspector] public bool hasPowerStateChanged;
    [HideInInspector] public List<NEW_LogicProcessor> incomingPowerSignals;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool previousIsPoweredState; // Used to check for any overrides to the isPowered state


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if (isPowered != previousIsPoweredState)
        {
            hasPowerStateChanged = true;
            previousIsPoweredState = isPowered;
        }
        else
        {
            hasPowerStateChanged = false;
        }

        //if (!powerTransmitter)
        //isPowered = CheckIncomingPowerSignals();
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    /// <summary>
    /// Checks all incoming power signals for a voltage, if no incoming power signals are powered, this object is not powered (unless it's a power emitter)
    /// </summary>
    /// <returns></returns>
    private bool CheckIncomingPowerSignals()
    {
        foreach (var powerSignal in incomingPowerSignals)
        {
            if (powerSignal.isPowered)
            {
                return true;
            }
        }

        return false;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}*/
