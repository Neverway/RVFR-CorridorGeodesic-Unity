//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Modulates the pitch variance of an audio source and provides options to play sounds with varying pitches.
// Notes: This script assumes an AudioSource component is attached to the same GameObject.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Modulates the pitch variance of an audio source and provides options to play sounds with varying pitches.
/// </summary>
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
    public IEnumerator PlaySound(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        SetAndPlayRandomPitch(null, false);
    }

    /// <summary>
    /// Plays a specific audio clip with a randomized pitch within the defined range.
    /// </summary>
    /// <param name="_audioClip">The audio clip to play.</param>
    /// <param name="_isOneShot">If true, multiple sound effects can be emmited from one source</param>
    public void PlaySound(AudioClip _audioClip, bool _isOneShot = true)
    {
        audioSource.clip = _audioClip;
        SetAndPlayRandomPitch(_audioClip, _isOneShot);
    }
    
    public void PlaySound()
    {
        SetAndPlayRandomPitch(null, false);
    }

    /// <summary>
    /// Sets the pitch of the audio source to a random value within the defined range and plays the audio.
    /// </summary>
    private void SetAndPlayRandomPitch(AudioClip _audioClip, bool _isOneShot)
    {
        audioSource.pitch = Random.Range(pitchVariance.x, pitchVariance.y);
        if (!_isOneShot) audioSource.Play();
        if (_isOneShot) audioSource.PlayOneShot(_audioClip);
    }
}
