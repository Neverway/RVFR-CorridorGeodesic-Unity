using Neverway.Framework.LogicValueSystem;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public class TEST_GetPowerFromLogicComponent : MonoBehaviour
{
    [LogicComponentHandle] public LogicComponent someComponent;
    public LogicOutput<bool> isLogicComponentPowered = new(false);

    public void Update()
    {
        isLogicComponentPowered.Set(someComponent.isPowered);
    }
}
