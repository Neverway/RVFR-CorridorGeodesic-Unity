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
    public float movementSpeed;
    public string team;
    public RuntimeAnimatorController animationController;
    public CharacterSounds characterSounds;
    public Vector3 groundCheckOffset;
    public float groundCheckRadius;
    public LayerMask groundMask;
    // Add project specific variables below this line!
    public float drag;
    public float movementMultiplier;
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
