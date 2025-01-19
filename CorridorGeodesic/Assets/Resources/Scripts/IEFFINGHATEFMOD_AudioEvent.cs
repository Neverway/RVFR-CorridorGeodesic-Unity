//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: I hate fmod I hate fmod I hate fmod
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace Neverway
{
public class IEFFINGHATEFMOD_AudioEvent : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public bool play;
    

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private EventReference audio;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
    }

    private void Update()
    {
        if (play)
        {
            Play();
            play = false;
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void Play()
    {
        Audio_FMODAudioManager.PlayOneShot(audio, transform.position);
    }
}
}
