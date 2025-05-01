//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose: When powered, a dialogue textbox will play out
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using UnityEngine;

namespace Neverway.Framework.LogicSystem
{
	public class LogicGate_DialogueEvent : LogicComponent
	{
		//=-----------------=
		// Public Variables
		//=-----------------=
		[LogicComponentHandle, SerializeField] private LogicComponent startSignal;
		[LogicComponentHandle, SerializeField] private LogicComponent resetSignal;
		public bool resetAutomatically;
		public bool autoProgress;
		public bool inProgress;
		public DialogueEvent dialogueEvent;


		//=-----------------=
		// Private Variables
		//=-----------------=
		private bool hasBeenTriggered;


		//=-----------------=
		// Reference Variables
		//=-----------------=


		//=-----------------=
		// Mono Functions
		//=-----------------=
		public void Update()
		{
			if (!inProgress && startSignal.isPowered && !hasBeenTriggered)
			{
				isPowered = true;
				hasBeenTriggered = true;
				StartCoroutine(WaitForReactivation());
				FindObjectOfType<DialogueEventManager>().StopDialogueEvent();
				FindObjectOfType<DialogueEventManager>().StartDialogueEvent(dialogueEvent, autoProgress);
				print("Started");
				inProgress = true;
			}
			else if (!autoProgress && inProgress && startSignal.isPowered && !hasBeenTriggered)
			{
				hasBeenTriggered = true;
				StartCoroutine(WaitForReactivation());
				FindObjectOfType<DialogueEventManager>().ContinueDialogueEvent();
				print("Progressing");
			}

			if (resetAutomatically && !FindObjectOfType<WB_DialogueBox>() && isPowered)
			{
				StartCoroutine(Reset());
				print("resetting");
			}
            if (!resetSignal) return;
			if (resetSignal.isPowered)
			{
				StartCoroutine(Reset());
			}
		}


		//=-----------------=
		// Internal Functions
		//=-----------------=
		private IEnumerator Reset()
		{
			yield return new WaitForSeconds(0.2f);
			isPowered = false;
			inProgress = false;
		}

		IEnumerator WaitForReactivation()
		{
			yield return new WaitForSeconds(0.2f);
			hasBeenTriggered = false;
		}


		//=-----------------=
		// External Functions
		//=-----------------=
		public void ForceReset()
		{
			if (!inProgress) return;
			FindObjectOfType<DialogueEventManager>().StopDialogueEvent();
			isPowered = false;
			inProgress = false;
		}
	}
}
