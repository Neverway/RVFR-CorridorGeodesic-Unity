//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
// Todo: Figure out if this is used and fix MusicPlayType reference
//
//=============================================================================

using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.PawnManagement;

namespace Neverway.Framework.LogicSystem
{
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
        [SerializeField] private bool pawnActivates = true;

        [Tooltip("Requires that pawnActivates is set to true")] [SerializeField]
        private bool onlyPlayerControllerPawns = true;

        [SerializeField] private bool physPropActivates;
        //[SerializeField] private MusicPlayType musicPlayType;

        private bool activated = false;

        //=-----------------=
        // Reference Variables
        //=-----------------=
        //private Audio_FMODMusicManager musicManager => Audio_FMODMusicManager.Instance;

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
            print("something entered the trigger");
            if (activated)
                return;

            base.OnTriggerEnter(_other); // Call the base class method
            if (_other.CompareTag("Pawn") && pawnActivates)
            {
                print("pawn entered");
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
            if (!resetAutomatically)
                activated = true;

            print("dont know if music manager exists");

            print(Audio_FMODMusicManager.Instance);
            if (Audio_FMODMusicManager.Instance.Equals(null))
                return;

            print("music will supposedly change");
            /*switch (musicPlayType)
            {
                case MusicPlayType.Play:
                    Audio_FMODMusicManager.Instance.PlayMusic(musicEvent);
                    break;
                case MusicPlayType.StopAllowFadeOut:
                    Audio_FMODMusicManager.Instance.StopMusic(true);
                    break;
                case MusicPlayType.StopImmediate:
                    Audio_FMODMusicManager.Instance.StopMusic();
                    break;
                default:
                    break;
            }*/
        }

        //=-----------------=
        // External Functions
        //=-----------------=
    }
}