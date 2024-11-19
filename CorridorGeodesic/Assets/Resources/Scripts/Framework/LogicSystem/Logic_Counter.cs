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
    public class Logic_Counter : LogicComponent
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public int count;

        //=-----------------=
        // Private Variables
        //=-----------------=
        [SerializeField, LogicComponentHandle] private LogicComponent addSignal;
        [SerializeField, LogicComponentHandle] private LogicComponent subtractSignal;

        //=-----------------=
        // Reference Variables
        //=-----------------=


        //=-----------------=
        // Mono Functions
        //=-----------------=
        //private void OnEnable()
        //{
        //    if (addSignal)
        //        addSignal.OnPowerStateChanged += SourcePowerStateChanged;
        //    if(subtractSignal)
        //        subtractSignal.OnPowerStateChanged += SourcePowerStateChanged;
        //}

        //=-----------------=
        // Internal Functions
        //=-----------------=

        //=-----------------=
        // External Functions
        //=-----------------=
        //public override void AutoSubscribe()
        //{
        //    subscribeLogicComponents.Add(addSignal);
        //    subscribeLogicComponents.Add(subtractSignal);
        //    base.AutoSubscribe();
        //}
        public override void SourcePowerStateChanged(bool powered)
        {
            base.SourcePowerStateChanged(powered);

            if (addSignal && addSignal.isPowered)
                count++;
            if (subtractSignal && subtractSignal.isPowered)
                count--;

            isPowered = !isPowered;
        }
    }
}