using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAirborneState : PlayerState
{
    private Vector3 moveVelocity;
    private float airTime;
    public PAirborneState(PlayerContext ctx) : base(ctx)
    {
    }
    public override void EnterState()
    {
        moveVelocity = ctx.playerController.velocity;
        moveVelocity.y = 0;

        airTime = 0;
    }
    public override void ExitState()
    {
        if (ctx.nextPState == PlayerStateEnum.Idle)
            ctx.CameraLand();

        ctx.playerController.Move(moveVelocity * Time.deltaTime + ctx.gravityVector * Time.deltaTime);
    }
    public override void UpdateState()
    {
        airTime += Time.deltaTime;

        if (ctx.grounded && airTime > 0.05f)
            ctx.SwitchState(PlayerStateEnum.Idle);

        ctx.AirMovement(ref moveVelocity, airTime);

        ctx.playerController.Move(moveVelocity * Time.deltaTime);

        //if(airTime > 0.2f)
        //    ctx.CheckForWall();
        ctx.Gravity();
        ctx.RotatePlayer();
    }
}