//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class SignalReceiver : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [ReadOnly(true)] public bool isPowered;
    public UnityEvent onPoweredEvent, onUnpoweredEvent;
    public event Action OnPowered, OnUnpowered;


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
        if (isPowered != previousIsPoweredState)
        {
            if (isPowered)
            {
                onPoweredEvent?.Invoke();
                OnPowered?.Invoke();
            }
            else
            {
                onUnpoweredEvent?.Invoke();
                OnUnpowered?.Invoke();
            }
        }
        previousIsPoweredState = isPowered;
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SetIsPowered(bool _isPowered)
    {
        isPowered = _isPowered;
    }
}
