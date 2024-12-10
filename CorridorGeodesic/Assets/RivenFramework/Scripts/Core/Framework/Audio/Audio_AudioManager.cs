//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_AudioManager: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static Audio_AudioManager Instance;

    [Header("Components")]
    public AudioClip buttonPress;

    public AudioClip floorButtonOn;
    public AudioClip floorButtonOff;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public AudioSource managerSource;

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
    public void PlayAudioOnSource(AudioSource source, AudioClip clip, bool oneShot = true, float pitchMin = 1, float pitchMax = 1, float volumeScale = 1)
    {
        source.pitch = Random.Range(pitchMin, pitchMax);
        
        if (oneShot)
            source.PlayOneShot(clip, volumeScale);
        else
        {
            source.clip = clip;
            source.volume *= volumeScale;
            source.Play();
        }
    }
}
