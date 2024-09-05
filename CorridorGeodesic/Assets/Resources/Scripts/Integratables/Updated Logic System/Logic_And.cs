//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic_And: LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private List<LogicComponent> inputs = new List<LogicComponent>();

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void OnEnable()
    {
        if (inputs.Count == 0)
            return;

        inputs.ForEach(i =>
        {
            if (i)
                i.OnPowerStateChanged += SourcePowerStateChanged;
        });
    }
    private void OnDestroy()
    {
        if (inputs.Count == 0)
            return;

        inputs.ForEach(i => 
        {
            if (i)
                i.OnPowerStateChanged -= SourcePowerStateChanged;
        });
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public override void SourcePowerStateChanged(bool powered)
    {
        isPowered = inputs.TrueForAll(i => i && i.isPowered);
    }
}
