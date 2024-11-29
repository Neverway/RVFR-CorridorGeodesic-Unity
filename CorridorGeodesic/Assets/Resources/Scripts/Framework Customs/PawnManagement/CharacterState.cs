//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: A duplicate data structure of characterData since dataStructure
//  runs into issues with data being written directly to the Scriptable Object
// Notes: Greetings future me! You are probably here trying to figure out why
//  the Pawn class has defaultState inheriting this class, but currentState
//  inheriting PlayerStateData.
//  To simplify, Scriptable Objects are instance based.
//  So if we tried to change the currentState's movement speed, it would change it 
//  for ALL playerStates. (THAT'S BAD!) So instead we use PlayerStateData to 
//  separate the data from the scriptable object. "Well why not change player
//  state to not be a SO?" I hear you say. Well my dear sweet coding idiot,
//  we still need PlayerState to be a SO so we can instance separate character
//  stats for each type of character. Good effin luck! ~Liz [06:01-Mar09-2024]
//  
//  Ah, well past me is an idiot. I have moved CharacterData to be its own SO
//  class. I am changing this class to now just be called CharacterState since
//  calling it CharacterStateData would be redundant and also this is for characters
//  not just for players. Good effin luck! ~Liz [22:47-Apr04-2024]
//
//=============================================================================

using System;
using UnityEngine;

namespace Neverway.Framework.PawnManagement
{
    [Serializable]
    public class CharacterState
    {
        // DON'T FORGET TO MODIFY THE Pawn & CharacterData CLASS TO MATCH THIS DATA!!!
        public string characterName;
        public float health;
        public float invulnerabilityTime;
        public float movementSpeed;
        public string team;
        public RuntimeAnimatorController animationController;
        public CharacterSounds characterSounds;
        public Vector3 groundCheckOffset;
        public float groundCheckRadius;
        [Tooltip("The collision layers that will be checked when testing if the entity is grounded")]
        public LayerMask groundMask;

        // Add project specific variables below this line!
        public float groundDrag;
        public float airDrag;
        public float maxHorizontalMovementSpeed;
        public float maxHorizontalAirSpeed;
        public float movementMultiplier;
        public float airMovementMultiplier;
        public float gravityMultiplier;
        public float jumpForce;
        public float steepSlopeAngle;
        [Tooltip("The multiplier added to the movementSpeed while grounded and sprinting")]
        public float sprintSpeedMultiplier;
        [Tooltip("Essentially how long it takes you to get up to speed while sprinting")]
        public float sprintAcceleration;
        public bool autoRegenHealth;
        public float healthRegenPerSecond;
        public float healthRegenDelay;
        [Tooltip("Amount of damage taken from falling.")]
        public float fallDamage;
        [Tooltip("Amount of damage taken from falling minimum distance.")]
        public float minFallDamage;
        [Tooltip("Rigidbody Y velocity required for max fall damage.")]
        public float fallDamageVelocity;
        [Tooltip("Rigidbody Y velocity required for minimum fall damage.")]
        public float minFallDamageVelocity;
    }
}