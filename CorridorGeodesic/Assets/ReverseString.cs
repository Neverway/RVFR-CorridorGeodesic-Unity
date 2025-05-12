using Neverway.Framework.LogicValueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseString : MonoBehaviour
{
    public LogicInput<string> inputString;
    public LogicOutput<string> reversedString;

    public void Update()
    {
        string output = "";
        foreach(char c in inputString.Get())
        {
            output = c + output;
        }
        reversedString.Set(output);
    }
}
