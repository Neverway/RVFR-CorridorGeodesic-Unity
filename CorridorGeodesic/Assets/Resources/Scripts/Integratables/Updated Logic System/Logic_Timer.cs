//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Logic_Timer: LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent inputSignal;
    [SerializeField] private float timerDuration;
    [SerializeField] private bool runTimerOnStart = true;
    [field:SerializeField] public UnityEvent onTimerStart { get; private set; } = new UnityEvent();
    [field:SerializeField] public UnityEvent onTimerEnd { get; private set; } = new UnityEvent();

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

    //=-----------------=
    // Internal Functions
    //=-----------------=
    IEnumerator CountDown()
    {
        isPowered = true;
        onTimerStart?.Invoke();
        yield return new WaitForSeconds(timerDuration);
        isPowered = false;
        onTimerEnd?.Invoke();
    }
    public override void OnEnable()
    {
        if (runTimerOnStart)
            base.OnEnable();
    }

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

        if (powered)
        {
            StopAllCoroutines();
            StartCoroutine(CountDown());
        }
    }
}
