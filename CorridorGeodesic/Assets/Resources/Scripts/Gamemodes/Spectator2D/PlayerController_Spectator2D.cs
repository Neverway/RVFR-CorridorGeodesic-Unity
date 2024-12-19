//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.PawnManagement;
using Neverway.Framework;
using UnityEngine.InputSystem;
using GameInstance = Neverway.Framework.PawnManagement.GameInstance;

namespace Neverway
{
    [CreateAssetMenu(fileName = "PlayerController_Spectator2D",
        menuName = "Neverway/ScriptableObjects/Pawns & Gamemodes/Controllers/PlayerController_Spectator2D")]
    public class PlayerController_Spectator2D : PawnController
    {
        //=-----------------=
        // Public Variables
        //=-----------------=


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private GameInstance gameInstance;
        public InputActions.SpectatorActions spectatorActions;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        public override void PawnAwake(Pawn _pawn)
        {
            gameInstance = FindObjectOfType<GameInstance>();
            spectatorActions = new InputActions().Spectator;
            spectatorActions.Enable();
        }

        public override void PawnUpdate(Pawn _pawn)
        {
            if (spectatorActions.Pause.WasPressedThisFrame()) gameInstance.UI_ShowPause();

            if (_pawn.isPaused) return;

            var movement = spectatorActions.Move.ReadValue<Vector2>();
            _pawn.Move(new Vector3(
                movement.x,
                movement.y, 0), "translate");
        }

        public override void PawnFixedUpdate(Pawn _pawn)
        {

        }


        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }


}