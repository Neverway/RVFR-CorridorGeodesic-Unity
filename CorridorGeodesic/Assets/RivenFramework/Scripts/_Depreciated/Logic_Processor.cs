//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework.LogicSystem
{
    public class Logic_Processor : MonoBehaviour
    {
        /*
            //=-----------------=
            // Public Variables
            //=-----------------=


            //=-----------------=
            // Private Variables
            //=-----------------=


            //=-----------------=
            // Reference Variables
            //=-----------------=


            //=-----------------=
            // Mono Functions
            //=-----------------=


            //=-----------------=
            // Internal Functions
            //=-----------------=
            private void UpdateLogicInteractables(string _targetSignalChannel, bool _isPowered)
            {
                foreach (var interactable in FindObjectsOfType<Logic_Interactable>())
                {
                    if (interactable.signalChannel == _targetSignalChannel) interactable.isPowered = _isPowered;
                }
            }

            private void UpdateLogicGates(string _targetSignalChannel, bool _isPowered)
            {
                foreach (var logicGate in FindObjectsOfType<LogicGate_And>())
                {
                    //if (logicGate.inputSignalA == _targetSignalChannel) logicGate.isAPowered = _isPowered;
                    //if (logicGate.inputSignalB == _targetSignalChannel) logicGate.isBPowered = _isPowered;
                }

                foreach (var logicGate in FindObjectsOfType<LogicGate_Not>())
                {
                    if (logicGate.inputSignal == _targetSignalChannel) logicGate.isInputPowered = _isPowered;
                }

                foreach (var logicGate in FindObjectsOfType<LogicGate_Or>())
                {
                    if (logicGate.inputSignalA == _targetSignalChannel) logicGate.isAPowered = _isPowered;
                    if (logicGate.inputSignalB == _targetSignalChannel) logicGate.isBPowered = _isPowered;
                }
            }

            private void ResetTriggers(string _targetSignalChannel)
            {
                foreach (var trigger in FindObjectsOfType<Trigger2D_Interactable>())
                {
                    if (trigger.resetSignal == _targetSignalChannel) trigger.hasBeenTriggered = false;
                }

                foreach (var trigger in FindObjectsOfType<Trigger2D_Event>())
                {
                    if (trigger.resetSignal == _targetSignalChannel) trigger.hasBeenTriggered = false;
                }
            }


            //=-----------------=
            // External Functions
            //=-----------------=
            public void UpdateState(string _targetSignalChannel, bool _isPowered)
            {
                // Check if the target signal channel is not empty
                if (string.IsNullOrEmpty(_targetSignalChannel)) return;

                // Update all logic objects
                UpdateLogicInteractables(_targetSignalChannel, _isPowered);
                UpdateLogicGates(_targetSignalChannel, _isPowered);
                ResetTriggers(_targetSignalChannel);
            }*/
    }
}
