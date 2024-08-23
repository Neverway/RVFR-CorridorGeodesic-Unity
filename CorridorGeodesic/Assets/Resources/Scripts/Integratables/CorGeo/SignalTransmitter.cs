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
using UnityEngine.Timeline;

public class SignalTransmitter : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("If this is true, any receivers will be powered")]
    public bool isPowered;
    [Tooltip("These objects will receive power from this transmitter")]
    public SignalReceiver[] receivers;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool previousIsPoweredState; // this is used to check to see if the powered state has changed


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if (isPowered == previousIsPoweredState) return;
        foreach (var receiver in receivers)
        {
            receiver.SetIsPowered(isPowered);
        }
        previousIsPoweredState = isPowered;
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
