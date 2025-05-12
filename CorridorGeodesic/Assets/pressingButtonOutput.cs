using Neverway.Framework.LogicValueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressingButtonOutput : MonoBehaviour
{
    public KeyCode buttonToCheck = KeyCode.Space;
    public LogicOutput<bool> pressingButtonState = new(false);

    void Update()
    {
        pressingButtonState.Set(Input.GetKey(buttonToCheck));
    }
}
