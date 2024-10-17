using Neverway.Framework.LogicValueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TEST_Component : MonoBehaviour
{
    [SerializeField] public EasyDrawerTestClass regularTest;
    [SerializeField] public EasyDrawerTestClass[] arrayTest;
    [SerializeField] public List<EasyDrawerTestClass> listTest;
    
    [Space]

    public LogicInput<bool> inputA = new(false);
    public LogicInput<bool> inputB = new(false);

    public LogicOutput<bool> output = new(false);

    public void Awake()
    {
        inputA.CallOnSourceChanged(OnInputUpdate);
        inputB.CallOnSourceChanged(OnInputUpdate);
    }

    public void OnInputUpdate()
    {
        output.Set(inputA.Get() && inputB.Get());
    }
}
[Serializable]
public class EasyDrawerTestClass
{
    public string someStringField;
    public Transform someTransform;
    public ComponentFieldReference<bool> fieldReference;
    public EasyDrawerTestClass waitASecond;
}