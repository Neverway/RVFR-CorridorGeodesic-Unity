using System;

public enum CompareOperation { LessThan, LessThanEqualTo, EqualTo, GreaterThanEqualTo, GreaterThan }
public enum PropertyType { Color, Float }
public enum DisableType { Destroy, Disable, Event }
public enum MusicPlayType { Play, StopAllowFadeOut, StopImmediate }
public enum CommonStateEnum { AddForce }
public enum PlayerStateEnum { Idle, Walk, Jump, Airborne, CrouchIdle, CrouchWalk, WallSlide }
public enum ElevatorState { Idle, Moving }
[Flags] public enum Hazard { Blank = 1, Confined = 2, Explosive = 4, Fire = 8, Kinetic = 16, Lava = 32 }
public enum SliceSpace
{
    Plane1,
    Plane2,
    Null
}

public enum RiftState
{
    None,
    Preview,
    Collapsing,
    Closed,
    Expanding,
    Idle
}