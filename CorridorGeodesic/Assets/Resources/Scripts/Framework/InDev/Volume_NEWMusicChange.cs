//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume_NEWMusicChange : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private EventReference musicEvent;

    [SerializeField] private bool resetAutomatically = false;
    [SerializeField] private bool pawnActivates=true;
    [Tooltip("Requires that pawnActivates is set to true")]
    [SerializeField] private bool onlyPlayerControllerPawns=true;
    [SerializeField] private bool physPropActivates;
    [SerializeField] private MusicPlayType musicPlayType;

    private bool activated = false;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Audio_FMODMusicManager musicManager => Audio_FMODMusicManager.Instance;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private new void OnTriggerEnter2D(Collider2D _other)
    {
        if (activated)
            return;

        base.OnTriggerEnter2D(_other); // Call the base class method
        if (_other.CompareTag("Pawn") && pawnActivates)
        {
            if (onlyPlayerControllerPawns && GetPlayerInTrigger())
            {
                ChangeMusic();
            }
            else
            {
                ChangeMusic();
            }
        }
        if (_other.CompareTag("PhysProp") && physPropActivates)
        {
            ChangeMusic();
        }
    }

    private new void OnTriggerEnter(Collider _other)
    {
        if (activated)
            return;

        base.OnTriggerEnter(_other); // Call the base class method
        if (_other.CompareTag("Pawn") && pawnActivates)
        {
            if (onlyPlayerControllerPawns && GetPlayerInTrigger())
            {
                ChangeMusic();
            }
            else
            {
                ChangeMusic();
            } 
        }
        if (_other.CompareTag("PhysProp") && physPropActivates)
        {
            ChangeMusic();
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    void ChangeMusic()
    {
        if(!resetAutomatically)
            activated = true;

        if (!musicManager)
            return;

        switch (musicPlayType)
        {
            case MusicPlayType.Play:
                musicManager.SwitchMusic(musicEvent);
                break;
            case MusicPlayType.StopAllowFadeOut:
                musicManager.StopMusic(true);
                break;
            case MusicPlayType.StopImmediate:
                musicManager.StopMusic();
                break;
            default:
                break;
        } 
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
