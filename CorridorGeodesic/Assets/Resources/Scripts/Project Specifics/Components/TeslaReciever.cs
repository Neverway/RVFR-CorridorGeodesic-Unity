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
    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private GameObject lightObject;
    private TeslaConductor conductor;

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private new void Awake()
    {
        base.Awake();
        conductor = GetComponent<TeslaConductor>();
    }

    private void OnDisable()
    {
        isPowered = false;
    }

    private void Update()
    {
        lightObject.SetActive(conductor.IsTeslaPowered());
        isPowered = conductor.IsTeslaPowered();
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}