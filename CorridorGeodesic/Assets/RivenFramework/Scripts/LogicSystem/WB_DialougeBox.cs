//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: This is currently CorGeo specific, it should be adopted into the framework
// Reference: https://youtu.be/1qbjmb_1hV4
//
//=============================================================================

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Neverway;
using Neverway.Framework.PawnManagement;
using UnityEngine.UI;

namespace Neverway.Framework.LogicSystem
{
    public class WB_DialogueBox : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public DialogueEvent dialogueEvent;
        public float printSpeed = 0.05f;
        public bool autoProgress;


        //=-----------------=
        // Private Variables
        //=-----------------=
        private int currentIndex;
        private string currentText;
        private bool enablePortrait;


        //=-----------------=
        // Reference Variables
        //=-----------------=
        [SerializeField] private Image portrait;
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text dialogue;
        [SerializeField] private GameObject dialogueObject;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=
        private IEnumerator ShowText()
        {
            for (int i = 0; i < dialogueEvent.dialogue[currentIndex].text.Length; i++)
            {
                currentText = dialogueEvent.dialogue[currentIndex].text.Substring(0, i + 1);
                dialogue.text = currentText;
                yield return new WaitForSeconds(printSpeed / dialogueEvent.dialogue[currentIndex].textSpeed);
            }
            dialogueEvent.dialogue[currentIndex].OnCompleted.Invoke();

            if (autoProgress)
            {
                StartCoroutine(NextFrame());
            }
        }

        private void SetDialogueMode()
        {
            var textOffset = dialogue.rectTransform;
            if (enablePortrait)
            {
                // Set to dialogue mode where we show the portrait and name
                dialogueObject.SetActive(true);
                //dialogue.rectTransform.offsetMin = new Vector2(280, textOffset.offsetMin.y);
            }
            else
            {
                // Set to dialogue mode where we show the portrait and name
                dialogueObject.SetActive(false);
                //dialogue.rectTransform.offsetMin = new Vector2(32, textOffset.offsetMin.y);
            }
        }


        //=-----------------=
        // External Functions
        //=-----------------=
        public void PrintFrame()
        {
            portrait.sprite = dialogueEvent.dialogue[currentIndex].portraitSpr;
            portrait.material = dialogueEvent.dialogue[currentIndex].portraitMat;
            name.text = dialogueEvent.dialogue[currentIndex].name;
            enablePortrait = dialogueEvent.dialogue[currentIndex].enablePortrait;
            SetDialogueMode();
            StartCoroutine(ShowText());
        }

        public IEnumerator NextFrame()
        {
            yield return new WaitForSeconds(dialogueEvent.dialogue[currentIndex].endDelay);
            if (dialogueEvent.dialogue.Count - 1 == currentIndex)
            {
                Destroy(gameObject);
            }
            else
            {
                currentIndex++;
                PrintFrame();
            }
        }

        public void QuickNextFrame()
        {
            if (dialogueEvent.dialogue.Count - 1 == currentIndex)
            {
                Destroy(gameObject);
            }
            else
            {
                currentIndex++;
                PrintFrame();
            }
        }
    }
}