//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
// Reference: https://youtu.be/1qbjmb_1hV4
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WB_DialogueBox : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public DialogueEvent dialogueEvent;
    public float printSpeed = 0.05f;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private int currentIndex;
    private string currentText;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text dialogue;


    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void PrintFrame()
    {
        name.text = dialogueEvent.dialogue[currentIndex].name;
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        for (int i = 0; i < dialogueEvent.dialogue[currentIndex].text.Length; i++)
        {
            currentText = dialogueEvent.dialogue[currentIndex].text.Substring(0, i+1);
            dialogue.text = currentText;
            yield return new WaitForSeconds(printSpeed / dialogueEvent.dialogue[currentIndex].textSpeed);
        }

        StartCoroutine(NextFrame());
    }

    private IEnumerator NextFrame()
    {
        yield return new WaitForSeconds(dialogueEvent.dialogue[currentIndex].endDelay);
        if (dialogueEvent.dialogue.Count-1 == currentIndex)
        {
            Destroy(gameObject);
        }
        else
        {
            currentIndex++;
            PrintFrame();
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
