using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State
{
    public PlayerState(PlayerContext ctx)
    {
        this.ctx = ctx;
    }
    public PlayerContext ctx;
    public abstract override void EnterState();
    public abstract override void ExitState();
    public abstract override void UpdateState();
}