//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_MusicManager: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static Audio_MusicManager Instance;

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private float fadeTime = 1;

    private bool queueActive;
    private bool fadeActive;

    private int currentSourceIndex = 0;
    private bool musicPlaying = false;

    private List<Audio_MusicSetting> musicSettings = new List<Audio_MusicSetting>();

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Audio_MusicSource[] sources;

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
    IEnumerator MusicQueue()
    {
        queueActive = true;

        while (musicSettings.Count > 0)
        {
            while(fadeActive)
                yield return null;

            if (musicPlaying)
                StartCoroutine(FadeOutIn());
            else
                StartNewTrack();

            yield return null;
        }

        queueActive = false;
    }
    IEnumerator FadeOutIn()
    {
        if (musicSettings.Count <= 0)
            yield break;

        fadeActive = true;
        musicPlaying = true;

        Audio_MusicSource fadeOutSource = sources[currentSourceIndex];

        //Increment current source index so that the value
        IncrementSourceIndex();

        Audio_MusicSource fadeInSource = sources[currentSourceIndex];

        fadeInSource.musicSetting = musicSettings[0];
        fadeInSource.Play();

        float timer = 0;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            fadeOutSource.volume = Mathf.Lerp(1, 0, timer/fadeTime);
            fadeInSource.volume = Mathf.Lerp(0, 1, timer/fadeTime);

            yield return null;
        }

        fadeOutSource.volume = 0;
        fadeInSource.volume = 1;

        fadeOutSource.Stop();

        musicSettings.RemoveAt(0);

        fadeActive = false;
    }
    void StartNewTrack()
    {
        Audio_MusicSource source = sources[currentSourceIndex];

        source.musicSetting = musicSettings[0];
        source.volume = 1;
        source.Play();

        musicSettings.RemoveAt(0);

        musicPlaying = true;
    }
    void IncrementSourceIndex()
    {
        currentSourceIndex = (int)Mathf.Repeat(currentSourceIndex + 1, sources.Length);
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void SwitchMusic(Audio_MusicSetting musicSetting)
    {
        musicSettings.Add(musicSetting);

        if (!queueActive)
            StartCoroutine(MusicQueue());
    }
}
