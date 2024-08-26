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

[RequireComponent(typeof(NEW_LogicProcessor))]
public class LogicGate_Not : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("Channels to compare to see if it's powered")]
    public NEW_LogicProcessor inputSignal;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private NEW_LogicProcessor logicProcessor;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        logicProcessor = GetComponent<NEW_LogicProcessor>();
    }
    
    private void Update()
    {
        if (!inputSignal)
        {
            logicProcessor.isPowered = false;
            return;
        }
        logicProcessor.isPowered = !inputSignal.isPowered;
    }

    private void OnDrawGizmos()
    {
        if (inputSignal) Debug.DrawLine(gameObject.transform.position, inputSignal.transform.position, Color.red);
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
