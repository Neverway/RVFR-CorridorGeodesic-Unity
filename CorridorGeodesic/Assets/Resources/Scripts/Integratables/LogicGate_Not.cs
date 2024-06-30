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

[RequireComponent(typeof(Logic_Processor))]
public class LogicGate_Not : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string inputSignal;
    public string outputSignal;
    
    public bool isInputPowered;
    public bool isOutputPowered;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Logic_Processor logicProcessor;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        logicProcessor = GetComponent<Logic_Processor>();
    }
    
    private void Update()
    {
        // If the logic gate is not fully linked, set it's state to false
        if (inputSignal == "")
        {
            isOutputPowered = false;
            logicProcessor.UpdateState(outputSignal, isOutputPowered);
            return;
        }

        isOutputPowered = !isInputPowered;
        
        // Update connected devices
        logicProcessor.UpdateState(outputSignal, isOutputPowered);
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
