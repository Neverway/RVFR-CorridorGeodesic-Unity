//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LogicGate_Timer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("Channels to compare to see if it's powered")]
    public NEW_LogicProcessor inputSignal;
    public float timerDuration;


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
            return;
        }

        if (inputSignal.hasPowerStateChanged && inputSignal.isPowered)
        {
            StartCoroutine(Countdown());
        }
    }

    private void OnDrawGizmos()
    {
        if (inputSignal) Debug.DrawLine(gameObject.transform.position, inputSignal.transform.position, Color.red);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator Countdown()
    {
        logicProcessor.isPowered = true;
        yield return new WaitForSeconds(timerDuration);
        logicProcessor.isPowered = false;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
