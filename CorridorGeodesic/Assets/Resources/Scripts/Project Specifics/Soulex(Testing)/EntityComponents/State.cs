using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public bool canEnterSelf = false;
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}