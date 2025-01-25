using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIdleState : PlayerState
{
    private Vector3 enterVelocity;
    public PIdleState(PlayerContext ctx) : base(ctx)
    {
    }
    public override void EnterState()
    {
        enterVelocity = ctx.playerController.velocity * 0.7f;
        enterVelocity.y = 0;
    }
    public override void ExitState()
    {
        ctx.playerController.Move(enterVelocity * Time.deltaTime);
    }
    public override void UpdateState()
    {
        if (ctx.moving)
            ctx.SwitchState(PlayerStateEnum.Walk);
        if (Input.GetKeyDown(KeyCode.LeftControl))
            ctx.SwitchState(PlayerStateEnum.CrouchIdle);
        if (Input.GetKeyDown(KeyCode.Space))
            ctx.SwitchState(PlayerStateEnum.Jump);
        if (!ctx.grounded)
            ctx.SwitchState(PlayerStateEnum.Airborne);

        enterVelocity = Vector3.Slerp(enterVelocity, Vector3.zero, Time.deltaTime * 10);
        ctx.playerController.Move(enterVelocity * Time.deltaTime);

        ctx.GroundGravity();

        ctx.RotatePlayer();
    }
}