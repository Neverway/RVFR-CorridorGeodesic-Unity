//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_FMODAudioManager: MonoBehaviour
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


    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public static void PlayOneShot(EventReference sound, Vector3 position = default)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }
    public static EventInstance CreateInstance(EventReference sound)
    {
        return RuntimeManager.CreateInstance(sound);
    }
    public static void DestroyInstance(EventInstance instance)
    {
        instance.release();
    }
    public static void SetParameter(EventInstance instance, string parameter, float value)
    {
        instance.setParameterByName(parameter, value);
    }
    public static void SetGlobalParameter(string parameter, float value)
    {
        RuntimeManager.StudioSystem.setParameterByName(parameter, value);
    }
}
