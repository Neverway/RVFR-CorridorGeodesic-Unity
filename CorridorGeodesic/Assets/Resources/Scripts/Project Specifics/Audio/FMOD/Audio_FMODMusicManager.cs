//===================== (Neverway 2024) Written by _____ =====================
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

public class Audio_FMODMusicManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static Audio_FMODMusicManager Instance;

    //=-----------------=
    // Private Variables
    //=-----------------=
    private EventInstance currentInstance;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        Instance = this;
        Audio_FMODAudioManager.SetGlobalParameter("MusicPlaying", 0);

        print("im doing things");
        print(Instance);
    }
    void FixedUpdate()
    {
        if(Instance == null)
            Instance = this;
        //print(Instance);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    IEnumerator ReleaseInstance(EventInstance instance)
    {
        yield return new WaitForSeconds(1);

        instance.release();
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void PlayMusic(EventReference musicReference)
    {
        print("music playsinas");
        if (currentInstance.isValid())
        {
            currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            StartCoroutine(ReleaseInstance(currentInstance));

            Audio_FMODAudioManager.SetGlobalParameter("MusicPlaying", 1);
        }

        currentInstance = Audio_FMODAudioManager.CreateInstance(musicReference);

        currentInstance.start();
    }
    public void StopMusic(bool fadeout = false)
    {
        if (currentInstance.isValid())
            currentInstance.stop(fadeout ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);

        Audio_FMODAudioManager.SetGlobalParameter("MusicPlaying", 0);
    }
}
