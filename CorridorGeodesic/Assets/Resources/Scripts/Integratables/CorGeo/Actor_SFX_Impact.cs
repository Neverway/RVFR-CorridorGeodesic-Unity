//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: 
// Source: https://youtu.be/-Gb_7fulyLk
//
//=============================================================================

using FMODUnity;
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
    [SerializeField] private EventReference impactSound;


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
            int contactCount = other.contactCount;
            float averageDot = 0;
            for (int i = 0; i < contactCount; i++) 
            {
                averageDot += Vector3.Dot(other.GetContact(i).normal, other.relativeVelocity);
            }
            averageDot /= contactCount;

            if(averageDot > 0.25f)
                Audio_FMODAudioManager.PlayOneShot(impactSound, transform.position);
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
