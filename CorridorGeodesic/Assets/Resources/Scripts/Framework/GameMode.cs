//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Used to define the pawn type that should be spawned for the player
//  as well as the fallback "spectator" pawn type that should be used if the player
//  has not actually spawned yet.
// Notes: These objects are assigned in the worldSettings
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework.PawnManagement
{
    [CreateAssetMenu(fileName = "GameMode", menuName = "Neverway/ScriptableObjects/Pawns & Gamemodes/GameMode")]
    public class GameMode : ScriptableObject
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public GameObject defaultPawnClass;
        public GameObject spectatorClass;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=


        //=-----------------=
        // Mono Functions
        //=-----------------=

        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}
