//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Modulates the pitch variance of an audio source and provides options to play sounds with varying pitches.
// Notes: This script assumes an AudioSource component is attached to the same GameObject.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioSource_PitchVarienceModulator : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private Vector2 pitchVariance = new Vector2(0.8f,1.2f);
    [SerializeField] private bool playOnAwake;
    [SerializeField] private float playOnAwakeDelay;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private AudioSource audioSource;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    /// <summary>
    /// Initializes the audio source and optionally plays the sound on awake.
    /// </summary>
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (playOnAwake) StartCoroutine(PlaySound(playOnAwakeDelay));
    }

    
    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    /// <summary>
    /// Plays the sound after a specified delay.
    /// </summary>
    /// <param name="_delay">The delay before playing the sound.</param>
    /// <returns>An IEnumerator for coroutine purposes.</returns>
    public IEnumerator PlaySound(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        SetAndPlayRandomPitch();
    }

    /// <summary>
    /// Plays a specific audio clip with a randomized pitch within the defined range.
    /// </summary>
    /// <param name="_audioClip">The audio clip to play.</param>
    public void PlaySound(AudioClip _audioClip)
    {
        audioSource.clip = _audioClip;
        SetAndPlayRandomPitch();
    }

    /// <summary>
    /// Sets the pitch of the audio source to a random value within the defined range and plays the audio.
    /// </summary>
    private void SetAndPlayRandomPitch()
    {
        audioSource.pitch = Random.Range(pitchVariance.x, pitchVariance.y);
        audioSource.Play();
    }
}
