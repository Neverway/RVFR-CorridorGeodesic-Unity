//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Neverway.Framework.LogicSystem
{
    public class SignalEventPlayer : LogicComponent
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public UnityEvent onPoweredEvent, onUnpoweredEvent;
        public event Action OnPowered, OnUnpowered;


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

            if (isPowered == powered)
                return;

            isPowered = powered;

            if (isPowered)
            {
                onPoweredEvent?.Invoke();
                OnPowered?.Invoke();
            }
            else
            {
                onUnpoweredEvent?.Invoke();
                OnUnpowered?.Invoke();
            }
        }

        public void SetIsPowered(bool _isPowered)
        {
            isPowered = _isPowered;
        }
    }
}