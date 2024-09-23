//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TeslaConductor))]
public class TeslaReciever : LogicComponent
{
    private TeslaConductor conductor;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private GameObject lightObject;

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private new void Awake()
    {
        base.Awake();
        conductor = GetComponent<TeslaConductor>();
    }

    private void Update()
    {
        lightObject.SetActive(conductor.isRecievingPower);
        isPowered = conductor.isRecievingPower;
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}