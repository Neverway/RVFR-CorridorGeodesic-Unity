using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PWalkState : PlayerState
{
    private Vector3 enterVelocity;
    private float rampVelocity;

    private EventInstance concreteWalk;
    private EventInstance metalWalk;
    private EventInstance glassWalk;
    private EventInstance plasticWalk;
    public PWalkState(PlayerContext ctx) : base(ctx)
    {
        //concreteWalk = Audio_FMODAudioManager.CreateInstance(Audio_FMODEvents.Instance.footstepsConcrete);
    }
    public override void EnterState()
    {
        //concreteWalk.start();

        rampVelocity = 0.5f;

        enterVelocity = ctx.playerController.velocity;
        enterVelocity.y = 0;

        if (enterVelocity.magnitude > ctx.walkSpeed * 0.7f)
            rampVelocity = 0.7f;
    }
    public override void ExitState()
    {
        //concreteWalk.stop(STOP_MODE.ALLOWFADEOUT);

        ctx.playerController.Move(ctx.moveVector * ctx.walkSpeed * rampVelocity + enterVelocity * Time.deltaTime);
    }
    public override void UpdateState()
    {
        rampVelocity = Mathf.Lerp(rampVelocity, 1, Time.deltaTime * 5);

        if (Input.GetKeyDown(KeyCode.Space))
            ctx.SwitchState(PlayerStateEnum.Jump);
        if (!ctx.moving)
            ctx.SwitchState(PlayerStateEnum.Idle);
        if(!ctx.grounded)
            ctx.SwitchState(PlayerStateEnum.Airborne);

        enterVelocity = Vector3.Slerp(enterVelocity, Vector3.zero, Time.deltaTime * 10);
        ctx.GroundGravity();

        ctx.RotatePlayer();
        ctx.playerController.Move(ctx.moveVector * ctx.walkSpeed * rampVelocity + enterVelocity * Time.deltaTime);
    }
}