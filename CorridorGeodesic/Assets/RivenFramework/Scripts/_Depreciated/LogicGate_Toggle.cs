//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: When powered, the output state switches
// Notes:
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework.LogicSystem
{
    [RequireComponent(typeof(NEW_LogicProcessor))]
    public class LogicGate_Toggle : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [Tooltip("Channels to compare to see if it's powered")]
        public NEW_LogicProcessor inputSignal;


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
                logicProcessor.isPowered = !logicProcessor.isPowered;
            }
        }

        private void OnDrawGizmos()
        {
            if (inputSignal) Debug.DrawLine(gameObject.transform.position, inputSignal.transform.position, Color.red);
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}
