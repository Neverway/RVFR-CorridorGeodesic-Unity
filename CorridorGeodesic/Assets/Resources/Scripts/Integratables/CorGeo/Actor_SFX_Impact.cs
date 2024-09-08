//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: 
// Source: https://youtu.be/-Gb_7fulyLk
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource_PitchVarienceModulator))]
public class Actor_SFX_Impact : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public float lightImpactThreshold, heavyImpactThreshold;
    public AudioClip lightImpact, heavyImpact;
    public float repeatDelay = 0.2f;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool isPlayingSound;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private AudioSource_PitchVarienceModulator audioSource;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        audioSource = GetComponent<AudioSource_PitchVarienceModulator>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isPlayingSound) return;
        StartCoroutine(RepeatDelay());
        if (other.relativeVelocity.magnitude >= lightImpactThreshold && other.relativeVelocity.magnitude < heavyImpactThreshold)
        {
            audioSource.PlaySound(lightImpact);
        }
        if (other.relativeVelocity.magnitude >= heavyImpactThreshold)
        {
            audioSource.PlaySound(heavyImpact);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator RepeatDelay()
    {
        isPlayingSound = true;
        yield return new WaitForSeconds(repeatDelay);
        isPlayingSound = false;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
