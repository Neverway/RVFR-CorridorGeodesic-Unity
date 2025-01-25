using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public static PlayerStateMachine Instance;
    public Vector3 playerCastCenter => (context as PlayerContext).playerCastCenter;
    public override void Awake()
    {
        base.Awake();

        Instance = this;

        stateDictionary.Add(PlayerStateEnum.Idle, new PIdleState(context as PlayerContext));
        stateDictionary.Add(PlayerStateEnum.Walk, new PWalkState(context as PlayerContext));
        stateDictionary.Add(PlayerStateEnum.Airborne, new PAirborneState(context as PlayerContext));
        stateDictionary.Add(PlayerStateEnum.CrouchIdle, new PCrouchIdleState(context as PlayerContext));
        stateDictionary.Add(PlayerStateEnum.CrouchWalk, new PCrouchWalkState(context as PlayerContext));
        //stateDictionary.Add(PlayerStateEnum.Slide, new PSlideState(context as PlayerContext));
        stateDictionary.Add(PlayerStateEnum.Jump, new PJumpState(context as PlayerContext));
        //stateDictionary.Add(PlayerStateEnum.WallRun, new PWallRunState(context as PlayerContext));
        //stateDictionary.Add(PlayerStateEnum.WallJump, new PWallJumpState(context as PlayerContext));
        stateDictionary.Add(CommonStateEnum.AddForce, new PAddForceState(context as PlayerContext));

        SwitchState(stateDictionary[PlayerStateEnum.Idle]);
    }
}