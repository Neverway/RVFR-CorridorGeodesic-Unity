using Neverway.Framework.LogicValueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGrower : MonoBehaviour
{
    public LogicInput<bool> growState = new(false);

    void Update()
    {
        if (growState.Get())
            transform.localScale = Vector3.one * 2f;
        else
            transform.localScale = Vector3.one * 0.5f;

    }
}
