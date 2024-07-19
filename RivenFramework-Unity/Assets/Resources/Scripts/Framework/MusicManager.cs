//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public AudioClip currentTrack;
    public AudioClip nextTrack;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public AudioSource currentTrackSource;
    public AudioSource nextTrackSource;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator WaitForCrossfade(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        
        // Store the current tracks playback point
        var storedTrackTime = currentTrackSource.time;
        // Swap tracks
        currentTrackSource.clip = nextTrack;
        // Make sure the track swap doesn't affect where the audio left off at
        currentTrackSource.time = storedTrackTime;
        // Clear next track queue
        nextTrackSource.clip = null;
        
        // Swap volumes
        currentTrackSource.volume = 1;
        nextTrackSource.volume = 0;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    /// <summary>
    /// Set the currently playing track immediately with no transition. Set _track to null to stop playing a track.
    /// </summary>
    /// <param name="_track"></param>
    public void SetCurrentTrack(AudioClip _track=null)
    {
        currentTrack = _track;
        currentTrackSource.clip = currentTrack;
        currentTrackSource.Play();
    }
    
    /// <summary>
    /// Set the queued track immediately with no transition. Set _track to null to clear.
    /// </summary>
    /// <param name="_track"></param>
    public void SetNextTrack(AudioClip _track=null)
    {
        nextTrack = _track;
        nextTrackSource.clip = nextTrack;
    }
    
    /// <summary>
    /// Switch smoothly from the current music track to a target track. (Target track can be left empty to make the music fadeout (Fadeout: Underground reference!))
    /// </summary>
    /// <param name="transitionTime">How long to take to fade from one track to another</param>
    /// <param name="_nextTrack">The track we want to switch to. Leave blank to fade music out.</param>
    public void CrossfadeTracks(float transitionTime=1f, AudioClip _nextTrack=null)
    {
        // Set next track
        SetNextTrack(_nextTrack);
        nextTrackSource.Play();
        // Lerp volumes
        GetComponent<ApplicationSettings>().audioMixer.GetFloat("Music", out var _volume);
        Mathf.Lerp(currentTrackSource.volume, 0, transitionTime);
        Mathf.Lerp(nextTrackSource.volume, _volume, transitionTime);
        // Once volumes have been crossfaded, set the current track to the next track and clear the next track queue
        StartCoroutine(WaitForCrossfade(transitionTime));
    }
}
