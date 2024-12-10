//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose: When powered, a dialogue textbox will play out
// Notes:
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework.LogicSystem
{
	public class LogicGate_DialogueEvent : LogicComponent
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
}
