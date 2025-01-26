using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSlideState : PlayerState
{
    private Vector3 inDirection;
    private Vector3 slideDir;
    private Vector3 slopeMove;
    public PSlideState(PlayerContext ctx) : base(ctx)
    {
    }
    public override void EnterState()
    {
        inDirection = new Vector3(ctx.playerController.velocity.x, 0, ctx.playerController.velocity.z);

        slideDir = ctx.playerController.velocity * 1.25f;

        slopeMove = Vector3.Project(Vector3.down * 15, inDirection);

        ctx.SetCamToCrouchHeight();
    }
    public override void ExitState()
    {
        ctx.SetCamToStandHeight();
        ctx.playerController.Move(slideDir * Time.deltaTime);
    }
    public override void UpdateState()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
            ctx.SwitchState(PlayerStateEnum.Idle);
        if (Input.GetKeyDown(KeyCode.Space))
            ctx.SwitchState(PlayerStateEnum.Jump);
        if (!ctx.grounded)
            ctx.SwitchState(PlayerStateEnum.Airborne);

        slopeMove = Vector3.Project(Vector3.down * 15, ctx.playerController.velocity) * ctx.slideSpeed;

        slideDir += slopeMove * Time.deltaTime;

        Vector3 frictionVector = ctx.playerController.velocity * ctx.friction;
        frictionVector += frictionVector.normalized * ctx.staticFriction;

        slideDir -= frictionVector * Time.deltaTime;

        if (slideDir.magnitude < 0.05f)
            slideDir = Vector3.zero;

        float slideSpeed = slideDir.magnitude;

        slideDir = Vector3.ProjectOnPlane(slideDir, ctx.groundNormal).normalized * slideSpeed;

        ctx.playerController.Move(slideDir * Time.deltaTime);
        //ctx.GroundGravity();
    }
}