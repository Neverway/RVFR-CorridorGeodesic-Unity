//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audio_MusicSource : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Audio_MusicSetting musicSetting;
    public float volume
    {
        get { return _volume; }
        set
        {
            _volume = value;

            if (!_enabled)
                return;

            source.volume = _volume;
        }
    }
    public float pitch
    {
        get { return _pitch; }
        set
        {
            _pitch = value;

            if (!_enabled)
                return;

            source.pitch = _pitch;
        }
    }
    public bool isPlaying => !_enabled ? false : source.isPlaying;

    //=-----------------=
    // Private Variables
    //=-----------------=
    private float _volume = 1;
    private float _pitch = 1;

    private bool _enabled;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    private AudioSource source;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        _enabled = TryGetComponent<AudioSource>(out source);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void Play()
    {
        if (!_enabled)
            return;

        source.clip = musicSetting.audioClipNoTail;
        source.Play();

        StartCoroutine(PrepareSwitchClip());
    }
    public void Stop()
    {
        if (!_enabled)
            return;

        StopAllCoroutines();
        source.Stop();
    }
    IEnumerator PrepareSwitchClip()
    {
        yield return new WaitForSeconds(source.clip.length);

        source.clip = musicSetting.audioClipTail;
        source.loop = true;
        source.Play();
    }
}
