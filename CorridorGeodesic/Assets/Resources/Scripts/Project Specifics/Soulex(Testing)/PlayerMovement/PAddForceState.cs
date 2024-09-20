using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAddForceState : PlayerState
{
    private Vector3 moveVelocity;
    private float airTime;
    public PAddForceState(PlayerContext ctx) : base(ctx)
    {
        canEnterSelf = true;
    }
    public override void EnterState()
    {
        moveVelocity = ctx.forceDirection;
        ctx.gravityVector.y = ctx.forceDirection.y;

        moveVelocity.y = 0;

        airTime = 0;
    }
    public override void ExitState()
    {
        if (ctx.nextPState == PlayerStateEnum.Idle)
            ctx.CameraLand();
        if ((CommonStateEnum)ctx.nextState != CommonStateEnum.AddForce)
            ctx.forceDirection = Vector3.zero;

        ctx.playerController.Move(moveVelocity * Time.deltaTime + ctx.gravityVector * Time.deltaTime);
    }
    public override void UpdateState()
    {
        airTime += Time.deltaTime;

        if (ctx.grounded && airTime > 0.05f)
            ctx.SwitchState(PlayerStateEnum.Idle);

        ctx.AirMovement(ref moveVelocity, airTime);

        ctx.playerController.Move(moveVelocity * Time.deltaTime);

        //if (airTime > 0.2f)
        //    ctx.CheckForWall();
        ctx.Gravity();
        ctx.RotatePlayer();
    }
}