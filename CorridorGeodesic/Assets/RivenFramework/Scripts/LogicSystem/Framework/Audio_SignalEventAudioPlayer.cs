//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.LogicSystem;

public class Audio_SignalEventAudioPlayer : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent inputSignal;
    [SerializeField] private Transform audioTransform;
    [SerializeField] private EventReference onPoweredAudio;
    [SerializeField] private EventReference onUnPoweredAudio;

    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        if (isPowered == powered)
            return;

        isPowered = powered;

        Vector3 audioPosition = audioTransform ? audioTransform.position : transform.position;

        if (isPowered && !onPoweredAudio.IsNull)
            Audio_FMODAudioManager.PlayOneShot(onPoweredAudio, audioPosition);
        else if (!isPowered && !onUnPoweredAudio.IsNull)
            Audio_FMODAudioManager.PlayOneShot(onUnPoweredAudio, audioPosition);
    }
}
