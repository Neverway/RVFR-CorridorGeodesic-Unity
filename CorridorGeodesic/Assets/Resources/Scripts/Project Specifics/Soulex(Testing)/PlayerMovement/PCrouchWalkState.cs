using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCrouchWalkState : PlayerState
{
    //private EventInstance concreteWalk;
    //private EventInstance metalWalk;
    //private EventInstance glassWalk;
    //private EventInstance plasticWalk;
    public PCrouchWalkState(PlayerContext ctx) : base(ctx)
    {
        //concreteWalk = Audio_FMODAudioManager.CreateInstance(Audio_FMODEvents.Instance.footstepsConcrete);
        //metalWalk = Audio_FMODAudioManager.CreateInstance(Audio_FMODEvents.Instance.footstepsMetal);
        //glassWalk = Audio_FMODAudioManager.CreateInstance(Audio_FMODEvents.Instance.footstepsGlass);
        //plasticWalk = Audio_FMODAudioManager.CreateInstance(Audio_FMODEvents.Instance.footstepsPlatic);
    }
    public override void EnterState()
    {
        //concreteWalk.start();

        ctx.SetCamToCrouchHeight();
        ctx.playerController.height = 1;
        ctx.playerController.center = Vector3.up * -0.5f;
    }
    public override void ExitState()
    {
        //concreteWalk.stop(STOP_MODE.ALLOWFADEOUT);

        if (ctx.nextPState == PlayerStateEnum.CrouchIdle)
            return;

        ctx.SetCamToStandHeight();
        ctx.playerController.height = 2;
        ctx.playerController.center = Vector3.zero;
    }
    public override void UpdateState()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
            ctx.SwitchState(PlayerStateEnum.Walk);
        if (Input.GetKeyDown(KeyCode.Space))
            ctx.SwitchState(PlayerStateEnum.Jump);
        if (!ctx.moving)
            ctx.SwitchState(PlayerStateEnum.CrouchIdle);

        ctx.GroundGravity();
        ctx.RotatePlayer();

        ctx.playerController.Move(ctx.moveVector * ctx.crouchSpeed);
    }
}