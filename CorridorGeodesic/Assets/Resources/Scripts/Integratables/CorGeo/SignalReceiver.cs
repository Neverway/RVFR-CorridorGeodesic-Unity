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
    [SerializeField] [ReadOnly(true)] private bool isPowered;
    public UnityEvent onPoweredEvent, onUnpoweredEvent;
    public event Action OnPowered, OnUnpowered;


    //=-----------------=
    // Private Variables
    //=-----------------=


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
    public void SetIsPowered(bool _isPowered)
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
}
