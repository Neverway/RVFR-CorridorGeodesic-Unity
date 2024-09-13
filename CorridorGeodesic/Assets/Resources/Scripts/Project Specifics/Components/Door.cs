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
    [SerializeField] private LogicComponent inputSignal;

    //=-----------------=
    // Reference Variables
    //=-----------------='
    [Header("References")]
    [SerializeField] private Animator animator;
    //[SerializeField] private Material poweredMaterial, unpoweredMaterial;
    //[SerializeField] private GameObject indicatorMesh;
    [HideInInspector] [SerializeField] private UnityEvent onPowered;
    [HideInInspector] [SerializeField] private UnityEvent onUnpowered;


    //=-----------------=
    // Mono Functions
    //=-----------------=


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
        base.SourcePowerStateChanged(powered);

        isPowered = powered;

        if (isPowered)
            onPowered?.Invoke();
        else
            onUnpowered?.Invoke();

        animator.SetBool("Powered", isPowered);
    }
}
