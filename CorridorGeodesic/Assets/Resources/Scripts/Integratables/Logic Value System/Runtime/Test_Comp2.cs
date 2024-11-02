using Neverway.Framework.LogicValueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Comp2 : MonoBehaviour
{
    [SerializeReference][Polymorphic] public SomethingBase something;

    public LogicInput<bool>[] listOfInputs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class SomethingBase
{
    public int baseInt;
}
[Serializable]
public class SomethingMore : SomethingBase
{
    public int anotherInt;
}
[Serializable]
public class SomethingElse : SomethingBase
{
    public string someString;
}