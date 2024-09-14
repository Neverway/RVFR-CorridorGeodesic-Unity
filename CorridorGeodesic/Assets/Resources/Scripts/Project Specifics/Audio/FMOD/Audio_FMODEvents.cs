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

    [Header("Weapons")]
    public EventReference nixieCrossShoot;
    public EventReference nixieTubeBreak;
    public EventReference nixieTubePin;

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
