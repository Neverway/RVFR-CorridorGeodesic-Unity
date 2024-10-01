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
using UnityEngine.Events;

public class Door : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent inputSignal;

    //=-----------------=
    // Reference Variables
    //=-----------------='
    [Header("References")]
    [SerializeField] private Animator animator;
    //[SerializeField] private Material poweredMaterial, unpoweredMaterial;
    //[SerializeField] private GameObject indicatorMesh;
    private UnityEvent onPowered;
    private UnityEvent onUnpowered;

    private bool previousPowered;
    private Coroutine powerCheckRoutine;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    protected new void OnEnable()
    {
        base.OnEnable();
        SourcePowerStateChanged(isPowered);
        previousPowered = isPowered;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void Update()
    {
        if (previousPowered != isPowered)
        {
            powerCheckRoutine = StartCoroutine(WaitToChangePowerState());
        }
        else
        {
            if (powerCheckRoutine != null)
                StopCoroutine(powerCheckRoutine);
        }
    }
    public IEnumerator WaitToChangePowerState()
    {
        yield return null;
        yield return null;
        yield return null;

        if (isPowered)
            onPowered?.Invoke();
        else
            onUnpowered?.Invoke();

        previousPowered = isPowered;
    }
    //=-----------------=
    // External Functions
    //=-----------------=
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        isPowered = powered;

        animator.SetBool("Powered", isPowered);
    }
}
