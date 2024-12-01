//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Output state is determined on if the counter passes the compare operation
// Notes:
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework.LogicSystem
{
    public class LogicGate_Comparator : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [Tooltip("Channels to compare to see if it's powered")]
        public LogicGate_Counter counterSignal;

        public int targetValue;

        [Header("0 - Less Than, 1 - Equal To, 2 - Greater Than")] [Range(0, 3)]
        public int compareOperation = 1;


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
            if (counterSignal)
            {
                switch (compareOperation)
                {
                    case 0:
                        logicProcessor.isPowered = counterSignal.currentValue < targetValue;
                        break;
                    case 1:
                        logicProcessor.isPowered = counterSignal.currentValue == targetValue;
                        break;
                    case 2:
                        logicProcessor.isPowered = counterSignal.currentValue > targetValue;
                        break;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (counterSignal)
                Debug.DrawLine(gameObject.transform.position, counterSignal.transform.position, Color.red);
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}