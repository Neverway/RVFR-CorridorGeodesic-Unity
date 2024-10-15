using Neverway.Framework.LogicValueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_SetPower : LogicComponent
{
    public LogicInput<bool> powerLogicComponent = new(false);

    public void Update()
    {
        if (powerLogicComponent != isPowered)
            SourcePowerStateChanged(powerLogicComponent);

        isPowered = powerLogicComponent;
    }
}
