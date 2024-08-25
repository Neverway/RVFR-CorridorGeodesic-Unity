//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Volume_TriggerEvent : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public bool resetsAutomatically = true;
    public bool checksOnlyForPlayer = true;
    
    [Header("Channel Events")]
    [Tooltip("A readable value to tell if this trigger is currently transmitting a power signal")]
    [ReadOnly] public bool isPowered;
    [Tooltip("Transmits a power signal to all devices when this volume has a target enter")]
    public GameObject[] onOccupiedTransmit;
    [Tooltip("When this trigger is powered, this event will be fired (this is used to trigger things that don't use our signal system)")]
    public UnityEvent onOccupied;


    //=-----------------=
    // Private Variables
    //=-----------------=
    [Tooltip("A variable to keep track of if this volume has already been trigger")]
    [HideInInspector] public bool hasBeenTriggered;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    //private SignalTransmitter//signalTransmitter;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        //signalTransmitter = GetComponent<SignalTransmitter>();
    }

    private new void OnTriggerEnter2D(Collider2D _other)
    {
        base.OnTriggerEnter2D(_other); // Call the base class method
        if (!resetsAutomatically && hasBeenTriggered) return;
        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEnt.isPossessed) return;
            //logicProcessor.UpdateState(onOccupiedSignal, true);
            //logicProcessor.UpdateState(onUnoccupiedSignal, false);
            //onOccupied.Invoke();
           //signalTransmitter.isPowered = true;
        }
        else if (pawnsInTrigger.Count != 0 || propsInTrigger.Count != 0)
        {
            //logicProcessor.UpdateState(onOccupiedSignal, true);
            //logicProcessor.UpdateState(onUnoccupiedSignal, false);
            //onOccupied.Invoke();
           //signalTransmitter.isPowered = true;
        }
        hasBeenTriggered = true;
    }

    private new void OnTriggerExit2D(Collider2D _other)
    {
        base.OnTriggerExit2D(_other); // Call the base class method
        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEnt.isPossessed) return;
            //logicProcessor.UpdateState(onOccupiedSignal, false);
            //logicProcessor.UpdateState(onUnoccupiedSignal, true);
            //onUnoccupied.Invoke();
           //signalTransmitter.isPowered = false;
        }
        else if (pawnsInTrigger.Count == 0 && propsInTrigger.Count == 0)
        {
            //logicProcessor.UpdateState(onOccupiedSignal, false);
            //logicProcessor.UpdateState(onUnoccupiedSignal, true);
            //onUnoccupied.Invoke();
           //signalTransmitter.isPowered = false;
        }
        if (resetsAutomatically) hasBeenTriggered = false;
    }

    private new void OnTriggerEnter(Collider _other)
    {
        base.OnTriggerEnter(_other); // Call the base class method
        if (!resetsAutomatically && hasBeenTriggered) return;
        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEnt.isPossessed) return;
            //logicProcessor.UpdateState(onOccupiedSignal, true);
            //logicProcessor.UpdateState(onUnoccupiedSignal, false);
            //onOccupied.Invoke();
           //signalTransmitter.isPowered = true;
        }
        else if (pawnsInTrigger.Count != 0 || propsInTrigger.Count != 0)
        {
            //logicProcessor.UpdateState(onOccupiedSignal, true);
            //logicProcessor.UpdateState(onUnoccupiedSignal, false);
            //onOccupied.Invoke();
           //signalTransmitter.isPowered = true;
        }
        hasBeenTriggered = true;
    }

    private new void OnTriggerExit(Collider _other)
    {
        base.OnTriggerExit(_other); // Call the base class method
        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEnt.isPossessed) return;
            //logicProcessor.UpdateState(onOccupiedSignal, false);
            //logicProcessor.UpdateState(onUnoccupiedSignal, true);
            //onUnoccupied.Invoke();
           //signalTransmitter.isPowered = false;
        }
        else if (pawnsInTrigger.Count == 0 && propsInTrigger.Count == 0)
        {
            //logicProcessor.UpdateState(onOccupiedSignal, false);
            //logicProcessor.UpdateState(onUnoccupiedSignal, true);
            //onUnoccupied.Invoke();
           //signalTransmitter.isPowered = false;
        }
        if (resetsAutomatically) hasBeenTriggered = false;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
