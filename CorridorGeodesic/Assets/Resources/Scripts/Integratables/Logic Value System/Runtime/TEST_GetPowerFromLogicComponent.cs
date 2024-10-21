using Neverway.Framework.LogicValueSystem;
using UnityEngine;

public class TEST_GetPowerFromLogicComponent : MonoBehaviour
{
    [LogicComponentHandle] public LogicComponent someComponent;
    public LogicOutput<bool> isLogicComponentPowered = new(false);

    public void Update()
    {
        isLogicComponentPowered.Set(someComponent.isPowered);
    }
}
