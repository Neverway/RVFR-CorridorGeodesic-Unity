//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
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
//=============================================================================

using System;
using UnityEngine;

[Serializable]
public class PlayerState : ScriptableObject
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public PlayerStateData data = new PlayerStateData();


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
