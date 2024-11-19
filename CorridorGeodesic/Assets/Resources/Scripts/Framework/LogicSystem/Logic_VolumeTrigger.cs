//===================== (Neverway 2024) Written by Andre Blunt =====================
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
    public class Logic_VolumeTrigger : LogicComponent
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


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Pawn"))
                isPowered = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Pawn"))
                isPowered = false;
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=

        //=-----------------=
        // External Functions
        //=-----------------=
    }
}
