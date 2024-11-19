//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: The output power state is the opposite of the input
// Notes:
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework.LogicSystem
{
    [RequireComponent(typeof(NEW_LogicProcessor))]
    public class LogicGate_Not : MonoBehaviour
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
                logicProcessor.isPowered = true;
                return;
            }

            logicProcessor.isPowered = !inputSignal.isPowered;
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
