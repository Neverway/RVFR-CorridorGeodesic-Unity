//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic_Comparator: LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private Logic_Counter counter;
    [SerializeField] private CompareOperation compareOperation;
    [SerializeField] private int compareValue;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    //private void OnEnable()
    //{
    //    if (counter)
    //        counter.OnPowerStateChanged += SourcePowerStateChanged;
    //}
    //private void OnDestroy()
    //{
    //    if (counter)
    //        counter.OnPowerStateChanged -= SourcePowerStateChanged;
    //}

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public override void AutoSubscribe()
    {
        subscribeLogicComponents.Add(counter);
        base.AutoSubscribe();
    }
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        switch (compareOperation)
        {
            case CompareOperation.LessThan:
                isPowered = counter.count < compareValue;
                break;
            case CompareOperation.LessThanEqualTo:
                isPowered = counter.count <= compareValue;
                break;
            case CompareOperation.EqualTo:
                isPowered = counter.count == compareValue;
                break;
            case CompareOperation.GreaterThanEqualTo:
                isPowered = counter.count >= compareValue;
                break;
            case CompareOperation.GreaterThan:
                isPowered = counter.count > compareValue;
                break;
            default:
                break;
        }
    }
}
