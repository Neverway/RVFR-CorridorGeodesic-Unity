using Neverway.Framework.LogicValueSystem;
using System;
using UnityEngine;

public class TEST_Component : MonoBehaviour
{
    public LogicInput<bool>[] inputs;

    public LogicInput<bool> inputA = new(false);
    public LogicInput<bool> inputB = new(false);
    public LogicInput<bool> inputArray;

    public LogicOutput<bool> output;

    public Component otherFields;
    [SerializeReference][Polymorphic] public BaseClass someField;
    public string otherFields2;
    public AnimationCurve evenMoreFields;

    public void Awake()
    {
        inputA.CallOnSourceChanged(OnInputUpdate);
        inputB.CallOnSourceChanged(OnInputUpdate);
    }

    public void OnInputUpdate()
    {
        //output.Set(inputA.Get() && inputB.Get());
    }
}
[Serializable]
public class BaseClass
{
    public string someStringField;
}
[Serializable]
public class SomeClassA : BaseClass
{
    public int someFieldA;
}
[Serializable]
public class SomeClassA2 : SomeClassA
{
    public int someFieldA2;
}
[Serializable]
public class SomeClassB : BaseClass
{
    public AnimationCurve someFieldB;
}