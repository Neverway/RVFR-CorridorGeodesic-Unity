using Neverway.Framework.LogicValueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObjectLogicInOut : MonoBehaviour
{
    public LogicInput<string> inputString = new("");
    public LogicOutput<string> outputString = new("");
    public TextObject textObject;

    public void Update()
    {
        textObject.text = inputString;
        outputString.Set(textObject.text);
    }
}
