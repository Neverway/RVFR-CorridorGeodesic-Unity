using Neverway.Framework.LogicValueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsStringsEqual : MonoBehaviour
{
    public LogicInput<string> string1;
    public LogicInput<string> string2;

    public LogicOutput<bool> isStringsEqual;

    void Update()
    {
        isStringsEqual.Set(string1.Get() == string2.Get());
    }
}
