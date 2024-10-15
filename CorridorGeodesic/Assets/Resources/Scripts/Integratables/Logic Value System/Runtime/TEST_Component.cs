using Neverway.Framework.LogicValueSystem;
using UnityEngine;

public class TEST_Component : MonoBehaviour
{
    public LogicInput<bool> inputA = new(false);
    public LogicInput<bool> inputB = new(false);

    public LogicOutput<bool> output = new(false);

    public void Update()
    {
        output.Set(inputA.Get() && inputB.Get());
    }

    [ContextMenu("Debuggg")]
    public void Yadda()
    {
        Debug.Log(inputA.IsLinkedToOutput + " + " + inputB.IsLinkedToOutput);
    }
}
