//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Runtime.CompilerServices;
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
    [LogicComponentHandle] public LogicComponent resetSignal;

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
    //public override void OnEnable()
    //{
    //    if (resetSignal)
    //        resetSignal.OnPowerStateChanged += SourcePowerStateChanged;
    //}

    //private void OnDrawGizmos()
    //{
    //    if (resetSignal) Debug.DrawLine(transform.position, resetSignal.transform.position, new Color(1,0.5f,0,1));
    //}

     /* Trigger enter/exit events
    private new void OnTriggerEnter2D(Collider2D _other)
    {
        base.OnTriggerEnter2D(_other); // Call the base class method

        if (!resetsAutomatically && hasBeenTriggered) return;
        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEntity.isPossessed) return; 
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

    private new void OnTriggerExit2D(Collider2D _other)
    {
        base.OnTriggerExit2D(_other); // Call the base class method
        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEntity.isPossessed) return;

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

        if (checksOnlyForPlayer && _other.tag == "PhysProp")
        {
            Debug.LogWarning("QUICKFIX: Just making sure cubes dont trigger this when they shouldn't, fix this properly later");
            return;
        }

        if (!resetsAutomatically && hasBeenTriggered) return;
        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEntity.isPossessed) return;
            if (!isPowered) //todo: quickfix
                onOccupied.Invoke();
            isPowered = true;
            hasBeenTriggered = true;
        }
        else if (pawnsInTrigger.Count != 0 || propsInTrigger.Count != 0)
        {
            if (!isPowered) //todo: quickfix
                onOccupied.Invoke();
            isPowered = true;
            hasBeenTriggered = true;
        }
    }

    private new void OnTriggerExit(Collider _other)
    {
        base.OnTriggerExit(_other); // Call the base class method

        if (checksOnlyForPlayer && _other.tag == "PhysProp")
        {
            Debug.LogWarning("QUICKFIX: Just making sure cubes dont trigger this when they shouldn't, fix this properly later");
            return;
        }

        if (checksOnlyForPlayer && _other.CompareTag("Pawn"))
        {
            if (!targetEntity.isPossessed) return;
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
    // */

    public override void OnDisable ()
    {
        base.OnDisable ();
        isPowered = false;
        if (resetsAutomatically) hasBeenTriggered = false;
        onUnoccupied?.Invoke ();
    }

    new private void OnDestroy ()
    {
        base.OnDestroy ();
        onUnoccupied?.Invoke ();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void Reset()
    {
        hasBeenTriggered = false;
    }

    protected override bool AddPawnToVolume(Pawn pawn)
    {
        //todo: Add field for only checking for props
        return base.AddPawnToVolume(pawn);
    }
    protected override bool RemovePawnFromVolume(Pawn pawn)
    {
        //todo: Add field for only checking for props
        return base.RemovePawnFromVolume(pawn);
    }
    protected override bool AddPropToVolume(GameObject prop)
    {
        //ignore prop logic if only checking for player
        if (checksOnlyForPlayer) 
            return false;

        return base.AddPropToVolume(prop);
    }
    protected override bool RemovePropFromVolume(GameObject prop)
    {
        //ignore prop logic if only checking for player
        if (checksOnlyForPlayer)
            return false;

        return base.RemovePropFromVolume(prop);
    }
    protected override void OnObjectsInVolumeUpdated(VolumeUpdateType updateType)
    {
        base.OnObjectsInVolumeUpdated(updateType);

        //if anything was added to volume
        if ((updateType & VolumeUpdateType.AnythingAdded) != 0)
            //And if has already been triggered
            if (!resetsAutomatically && hasBeenTriggered)
                return;

        ValidatePowerState();
    }
    private void ValidatePowerState()
    {
        bool previouslyPowered = isPowered;

        //if "checksOnlyForPlayer", then power this component if a pawn is in this trigger
        if (checksOnlyForPlayer)
            isPowered = pawnsInTrigger.Count > 0;
        //otherwise, power this component if EITHER a pawn OR a prop is in this trigger
        else
            isPowered = pawnsInTrigger.Count > 0 || propsInTrigger.Count > 0;

        //if went from not powered to powered 
        if (!previouslyPowered && isPowered)
        {
            onOccupied?.Invoke();
            hasBeenTriggered = true;
        }
        //if went from powered to not powered
        else if (previouslyPowered && !isPowered)
        {
            onUnoccupied?.Invoke();
            if (resetsAutomatically) hasBeenTriggered = false;
        }
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
