//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate_DialogueEvent: LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [LogicComponentHandle, SerializeField] private LogicComponent startSignal;
    public bool inProgress;
    public DialogueEvent dialogueEvent;
	
	
    //=-----------------=
    // Private Variables
    //=-----------------=
	
	
    //=-----------------=
    // Reference Variables
    //=-----------------=
	
	
    //=-----------------=
    // Mono Functions
    //=-----------------=
    public void Update()
    {
	    if (!inProgress && startSignal.isPowered)
	    {
		    isPowered = true;
		    FindObjectOfType<DialougeEventManager>().StartDialogueEvent(dialogueEvent);
		    inProgress = true;
	    }
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
	
	
    //=-----------------=
    // External Functions
    //=-----------------=
}
