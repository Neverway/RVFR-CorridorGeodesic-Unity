//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
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
    [Header("Signal Events")]
    public LogicComponent resetSignal;
    
    //[Header("Channel Events")]
    //[Tooltip("When this trigger is powered, this event will be fired (this is used to trigger things that don't use our signal system)")]
    public UnityEvent onOccupied, onUnoccupied;


    //=-----------------=
    // Private Variables
    //=-----------------=
    [Tooltip("A variable to keep track of if this volume has already been trigger")]
    public bool hasBeenTriggered;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        Reset();
    }
    private void OnEnable()
    {
        if (resetSignal)
            resetSignal.OnPowerStateChanged += SourcePowerStateChanged;
    }

    private void OnDrawGizmos()
    {
        if (resetSignal) Debug.DrawLine(transform.position, resetSignal.transform.position, new Color(1,0.5f,0,1));
    }

    private new void OnTriggerEnter2D(Collider2D _other)
    {
        base.OnTriggerEnter2D(_other); // Call the base class method
        if (!resetsAutomatically && hasBeenTriggered) return;
        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEnt.isPossessed) return; 
            onOccupied.Invoke();
            isPowered = true;
            hasBeenTriggered = true;
        }
        else if (pawnsInTrigger.Count != 0 || propsInTrigger.Count != 0)
        {
            onOccupied.Invoke();
            isPowered = true;
            hasBeenTriggered = true;
        }
        print(_other.name);
    }

    private new void OnTriggerExit2D(Collider2D _other)
    {
        base.OnTriggerExit2D(_other); // Call the base class method
        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEnt.isPossessed) return;
            onUnoccupied.Invoke();
            isPowered = false;
            if (resetsAutomatically) hasBeenTriggered = false;
        }
        else if (pawnsInTrigger.Count == 0 && propsInTrigger.Count == 0)
        {
            onUnoccupied.Invoke();
            isPowered = false;
            if (resetsAutomatically) hasBeenTriggered = false;
        }
    }

    private new void OnTriggerEnter(Collider _other)
    {
        base.OnTriggerEnter(_other); // Call the base class method
        if (!resetsAutomatically && hasBeenTriggered) return;
        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEnt.isPossessed) return;
            onOccupied.Invoke();
            isPowered = true;
            hasBeenTriggered = true;
        }
        else if (pawnsInTrigger.Count != 0 || propsInTrigger.Count != 0)
        {
            onOccupied.Invoke();
            isPowered = true;
            hasBeenTriggered = true;
        }
    }

    private new void OnTriggerExit(Collider _other)
    {
        base.OnTriggerExit(_other); // Call the base class method
        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEnt.isPossessed) return;
            onUnoccupied.Invoke();
            isPowered = false;
            if (resetsAutomatically) hasBeenTriggered = false;
        }
        else if (pawnsInTrigger.Count == 0 && propsInTrigger.Count == 0)
        {
            onUnoccupied.Invoke();
            isPowered = false;
            if (resetsAutomatically) hasBeenTriggered = false;
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void Reset()
    {
        hasBeenTriggered = false;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);
        if (resetSignal && resetSignal.isPowered)
            Reset();
    }
}
