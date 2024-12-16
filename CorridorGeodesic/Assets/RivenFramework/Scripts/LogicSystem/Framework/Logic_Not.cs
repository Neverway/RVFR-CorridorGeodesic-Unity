//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Neverway.Framework.LogicSystem
{
    public class Logic_Not : LogicComponent
    {
        //=-----------------=
        // Public Variables
        //=-----------------=


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
        //private void OnEnable()
        //{
        //    if (input)
        //        input.OnPowerStateChanged += SourcePowerStateChanged;
        //}
        //private void OnDestroy()
        //{
        //    if (input)
        //        input.OnPowerStateChanged -= SourcePowerStateChanged;
        //}
        public void Update()
        {
            isPowered = !(inputSignal != null && inputSignal.isPowered);
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
            isPowered = !powered;
        }
    }
}