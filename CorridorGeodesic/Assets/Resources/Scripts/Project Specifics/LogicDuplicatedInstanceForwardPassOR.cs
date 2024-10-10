using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

//This component passes its power FORWARDS instead of reference powered states backwards
//This is used for when a LogicComponent gets duplicated when Mesh_Sliceables slice the mesh its a part of
//such as for volumes
public class LogicDuplicatedInstanceForwardPassOR : LogicComponent
{
    public LogicOr toForwardPassTo;
    public LogicComponent inputSignalToPass;

    public new void OnEnable()
    {
        if (toForwardPassTo == null || inputSignalToPass == null)
            return;

        if (!toForwardPassTo.inputSignals.Contains(this))
        {
            toForwardPassTo.inputSignals.Add(this);
            toForwardPassTo.SourcePowerStateChanged(inputSignalToPass.isPowered);
        }
    }

    public void OnDisable()
    {
        if (toForwardPassTo == null || inputSignalToPass == null)
            return;

        toForwardPassTo.inputSignals.Remove(this);
        toForwardPassTo.SourcePowerStateChanged(false);
    }
}
