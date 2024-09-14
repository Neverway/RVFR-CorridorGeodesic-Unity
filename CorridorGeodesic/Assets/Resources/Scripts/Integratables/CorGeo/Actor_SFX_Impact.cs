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

public class Actor_SFX_Impact : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public float impactThreshold;
    public float repeatDelay = 0.2f;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool isPlayingSound;


    //=-----------------=
    // Reference Variables
    //=-----------------=

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void OnCollisionEnter(Collision other)
    {
        if (isPlayingSound) return;
        StartCoroutine(RepeatDelay());
        if (other.relativeVelocity.magnitude >= impactThreshold)
        {
            Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.metalImpact, transform.position);
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
