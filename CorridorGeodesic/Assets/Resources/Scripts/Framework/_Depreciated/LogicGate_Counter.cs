//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: One input will add to the counter, and another will subtract,
//  this is paired with a logic comparator to check for certain values
// Notes:
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework.LogicSystem
{
    public class LogicGate_Counter : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [Tooltip("Channels to compare to see if it's powered")]
        public NEW_LogicProcessor addSignal;

        public NEW_LogicProcessor subtractSignal;

        public int currentValue;


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
            if (addSignal && addSignal.hasPowerStateChanged && addSignal.isPowered) currentValue++;
            if (subtractSignal && subtractSignal.hasPowerStateChanged && subtractSignal.isPowered) currentValue--;
        }

        private void OnDrawGizmos()
        {
            if (addSignal) Debug.DrawLine(gameObject.transform.position, addSignal.transform.position, Color.green);
            if (subtractSignal)
                Debug.DrawLine(gameObject.transform.position, subtractSignal.transform.position, Color.red);
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}
