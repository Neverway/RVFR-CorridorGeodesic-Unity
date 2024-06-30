//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Logic_Processor))]
public class LogicGate_Or : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string inputSignalA;
    public string inputSignalB;
    public string outputSignal;

    public bool isAPowered;
    public bool isBPowered;
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
        if (inputSignalA == "" || inputSignalB == "")
        {
            isOutputPowered = false;
            logicProcessor.UpdateState(outputSignal, isOutputPowered);
            return;
        }
        
        // Set isActive
        isOutputPowered = isAPowered || isBPowered;
        
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
