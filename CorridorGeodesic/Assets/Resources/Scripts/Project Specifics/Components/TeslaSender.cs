//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//todo: make a manager that Lists senders and receivers and calls them accordionly
[RequireComponent(typeof(TeslaConductor))]
public class TeslaSender : LogicComponent, TeslaPowerSource
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [LogicComponentHandle] public LogicComponent inputSignal;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private GameObject lightObject;
    private TeslaConductor conductor;

    public bool IsTeslaPowered() => inputSignal == null || inputSignal.isPowered;

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private new void Awake()
    {
        base.Awake();
        conductor = GetComponent<TeslaConductor>();
    }

    public void Update()
    {
        conductor.SetPowerSource(this);
    }

    public Transform GetZapTargetTransform() => transform;
}