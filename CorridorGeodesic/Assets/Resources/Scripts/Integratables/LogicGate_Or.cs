//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NEW_LogicProcessor))]
public class LogicGate_Or : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("Channels to compare to see if it's powered")]
    public NEW_LogicProcessor inputA, inputB;


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
        if (!inputA || !inputB)
        {
            logicProcessor.isPowered = false;
            return;
        }
        logicProcessor.isPowered = inputA.isPowered || inputB.isPowered;
    }

    private void OnDrawGizmos()
    {
        if (inputA) Debug.DrawLine(gameObject.transform.position, inputA.transform.position, Color.red);
        if (inputB) Debug.DrawLine(gameObject.transform.position, inputB.transform.position, Color.green);
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
