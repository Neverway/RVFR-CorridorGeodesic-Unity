//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework
{
    public class Volume_MusicChange : Volume
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public AudioClip musicTrack;
        public float transitionTime = 1;
        [SerializeField] private bool pawnActivates = true;

        [Tooltip("Requires that pawnActivates is set to true")] [SerializeField]
        private bool onlyPlayerControllerPawns = true;

        [SerializeField] private bool physPropActivates;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private MusicManager musicManager;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private new void OnTriggerEnter2D(Collider2D _other)
        {
            base.OnTriggerEnter2D(_other); // Call the base class method
            if (_other.CompareTag("Pawn") && pawnActivates)
            {
                if (onlyPlayerControllerPawns && GetPlayerInTrigger() || onlyPlayerControllerPawns == false)
                {
                    if (!musicManager) musicManager = FindObjectOfType<MusicManager>();
                    musicManager.CrossfadeTracks(transitionTime, musicTrack);
                }
            }

            if (_other.CompareTag("PhysProp") && physPropActivates)
            {
                if (!musicManager) musicManager = FindObjectOfType<MusicManager>();
                musicManager.CrossfadeTracks(transitionTime, musicTrack);
            }
        }

        private new void OnTriggerEnter(Collider _other)
        {
            base.OnTriggerEnter(_other); // Call the base class method
            if (_other.CompareTag("Pawn") && pawnActivates)
            {
                if (onlyPlayerControllerPawns && GetPlayerInTrigger() || onlyPlayerControllerPawns == false)
                {
                    if (!musicManager) musicManager = FindObjectOfType<MusicManager>();
                    musicManager.CrossfadeTracks(transitionTime, musicTrack);
                }
            }

            if (_other.CompareTag("PhysProp") && physPropActivates)
            {
                if (!musicManager) musicManager = FindObjectOfType<MusicManager>();
                musicManager.CrossfadeTracks(transitionTime, musicTrack);
            }
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}