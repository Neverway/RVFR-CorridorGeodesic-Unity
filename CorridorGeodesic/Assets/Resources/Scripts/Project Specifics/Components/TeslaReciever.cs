//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaReciever : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [DebugReadOnly] public bool isRecievingPower;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private GameObject lightObject;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private IEnumerator Start()
    {
        yield return null;
        TeslaManager.recievers.Add (this);
    }

    private void Update ()
    {
        lightObject.SetActive(isRecievingPower);
        isPowered = isRecievingPower;
    }

    private void OnDestroy ()
    {
        TeslaManager.recievers.Remove(this);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}