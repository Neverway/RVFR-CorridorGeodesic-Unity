//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeEventManager: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private GameObject dialogueBoxWidget;


    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void StartDialogueEvent(DialogueEvent _dialogueEvent)
    {
        // Stop all existing dialogue events
        StopDialogueEvent();
        GameInstance.AddWidget(dialogueBoxWidget);
        FindObjectOfType<WB_DialogueBox>().dialogueEvent = _dialogueEvent;
        FindObjectOfType<WB_DialogueBox>().PrintFrame();
    }

    public void StopDialogueEvent()
    {
        Destroy(GameInstance.GetWidget("WB_DialogueBox"));
    }
}
