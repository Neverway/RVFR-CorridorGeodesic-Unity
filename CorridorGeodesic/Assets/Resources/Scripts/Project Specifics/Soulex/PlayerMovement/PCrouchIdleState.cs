using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCrouchIdleState : PlayerState
{
    public PCrouchIdleState(PlayerContext ctx) : base(ctx)
    {
    }
    public override void EnterState()
    {
        ctx.SetCamToCrouchHeight();
        ctx.playerController.height = 1;
        ctx.playerController.center = Vector3.up * -0.5f;
    }
    public override void ExitState()
    {
        if (ctx.nextPState == PlayerStateEnum.CrouchWalk)
            return;

        ctx.SetCamToStandHeight();
        ctx.playerController.height = 2;
        ctx.playerController.center = Vector3.zero;
    }
    public override void UpdateState()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
            ctx.SwitchState(PlayerStateEnum.Idle);
        if (Input.GetKeyDown(KeyCode.Space))
            ctx.SwitchState(PlayerStateEnum.Jump);
        if (ctx.moving)
            ctx.SwitchState(PlayerStateEnum.CrouchWalk);

        ctx.GroundGravity();
        ctx.RotatePlayer();
    }
}