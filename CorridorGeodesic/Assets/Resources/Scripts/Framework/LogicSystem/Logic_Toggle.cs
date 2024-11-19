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
    public class Logic_Toggle : LogicComponent
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [Tooltip("If true, the toggle will stay powered once powered a single time. No other signals will turn it off")]
        public bool stayPowered = false;

        //=-----------------=
        // Private Variables
        //=-----------------=
        [SerializeField, LogicComponentHandle] private LogicComponent inputSignal;

        //=-----------------=
        // Reference Variables
        //=-----------------=


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private new void OnEnable()
        {
            //This override is necessary to avoid SourcePowerStateChanged(); from getting called
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=

        //=-----------------=
        // External Functions
        //=-----------------=
        //public override void AutoSubscribe()
        //{
        //    subscribeLogicComponents.Add(inputSignal);
        //    base.AutoSubscribe();
        //}
        public override void SourcePowerStateChanged(bool powered)
        {
            base.SourcePowerStateChanged(powered);

            if (stayPowered)
            {
                isPowered = isPowered || powered;
                return;
            }

            isPowered = powered ? !isPowered : isPowered;
        }
    }
}