using Neverway.Framework.LogicValueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineStrings : MonoBehaviour
{
    public LogicInput<string> string1 = new("");
    public LogicInput<string> string2 = new("");

    public LogicOutput<string> combinedString = new("");

    void Update()
    {
        combinedString.Set(string1.Get() + string2.Get());
    }
}
