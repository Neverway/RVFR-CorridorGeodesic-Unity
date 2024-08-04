//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using UnityEngine;

[CreateAssetMenu(fileName="CharacterData", menuName="Neverway/ScriptableObjects/Pawns & Gamemodes/CharacterData")]
public class CharacterData : Actor
{
    // DON'T FORGET TO MODIFY THE Pawn & CharacterState CLASS TO MATCH THIS DATA!!!
    public string characterName;
    public float health;
    public float invulnerabilityTime;
    public float movementSpeed;
    public string team;
    public RuntimeAnimatorController animationController;
    public CharacterSounds characterSounds;
    public Vector3 groundCheckOffset;
    public float groundCheckRadius;
    public LayerMask groundMask;
    // Add project specific variables below this line!
    public float groundDrag;
    public float airDrag;
    public float movementMultiplier;
    public float airMovementMultiplier;
    public float gravityMultiplier;
    public float jumpForce;
    [Tooltip("The multiplier added to the movementSpeed while grounded and sprinting")]
    public float sprintSpeedMultiplier;
    [Tooltip("Essentially how long it takes you to get up to speed while sprinting")]
    public float sprintAcceleration;
    [Tooltip("The maximum a player can set upwards in units when they hit a wall that's potentially a step")]
    public float maxStepHeight;
    [Tooltip("How much to overshoot into the direction a potential step in units when testing. High values prevent player from walking up tiny steps but may cause problems.")]
    public float stepSearchOvershoot;
}

[Serializable]
public class CharacterSounds
{
    public AudioClip hurt;
    public AudioClip heal;
    public AudioClip death;
    public AudioClip alerted;
    // Add project specific variables below this line!
}
