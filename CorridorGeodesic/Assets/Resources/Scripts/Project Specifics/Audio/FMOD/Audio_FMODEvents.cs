//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_FMODEvents: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static Audio_FMODEvents Instance;

    [Header("Components")]

    [Header("Physics")]
    public EventReference metalImpact;
    public EventReference footstepsConcrete;
    public EventReference footstepsMetal;
    public EventReference footstepsGlass;
    public EventReference footstepsPlastic;
    public EventReference liquidSplatter;

    [Header("Weapons")]
    public EventReference nixieCrossShoot;
    public EventReference nixieTubeBreak;
    public EventReference nixieTubePin;

    [Header("FrameWork")]
    public EventReference hover;
    public EventReference select;

    [Header("Rift")]
    public EventReference riftSpawned;
    public EventReference riftKilled;
    public EventReference riftCollapsing;
    public EventReference riftExpanding;
    public EventReference riftIdle;

    [Header("Elevator")]
    public EventReference elevatorOpen;
    public EventReference elevatorClose;
    public EventReference elevatorReady;
    public EventReference elevatorMove;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        Instance = this;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
