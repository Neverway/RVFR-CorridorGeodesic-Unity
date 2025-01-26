using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJumpState : PlayerState
{
    private float enterYPos;
    private float jumpTime;
    private Vector3 moveVelocity;
    private float maxSpeed;
    public PJumpState(PlayerContext ctx) : base(ctx)
    {
    }
    public override void EnterState()
    {
        jumpTime = 0;

        ctx.gravityVector.y = Mathf.Sqrt(-2 * Physics.gravity.y * ctx.jumpHeight);

        enterYPos = ctx.transform.position.y;

        moveVelocity = ctx.playerController.velocity;
        moveVelocity.y = 0;

        maxSpeed = moveVelocity.magnitude/1.25f;

        if(maxSpeed < ctx.walkSpeed)
            maxSpeed = ctx.walkSpeed;
    }
    public override void ExitState()
    {
        if (ctx.nextPState == PlayerStateEnum.Idle)
            ctx.CameraLand();
        ctx.playerController.Move(moveVelocity * Time.deltaTime + ctx.gravityVector * Time.deltaTime);
    }
    public override void UpdateState()
    {
        jumpTime += Time.deltaTime;

        if (ctx.grounded && jumpTime > 0.2f)
            ctx.SwitchState(PlayerStateEnum.Idle);
        if (ctx.transform.position.y < enterYPos)
            ctx.SwitchState(PlayerStateEnum.Airborne);

        if (ctx.moving)
        {
            moveVelocity += ctx.moveVector * ctx.airSpeed;

            if (moveVelocity.magnitude > maxSpeed)
                moveVelocity *= maxSpeed / moveVelocity.magnitude;
        }

        ctx.playerController.Move(moveVelocity * Time.deltaTime);

        ctx.Gravity();
        ctx.RotatePlayer();
        //ctx.CheckForWall();
    }
}