//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStateData
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string characterName;
    public float health = 100;
    public float movementSpeed = 5; // Please do not delete me, I am valuable. Feel free to add more variables here though. 
    public string team;
    public RuntimeAnimatorController animator;
    public Sounds sounds;


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

[Serializable]
public class Sounds
{
    public AudioClip hurt;
    public AudioClip heal;
    public AudioClip death;
    public AudioClip alerted;
}
