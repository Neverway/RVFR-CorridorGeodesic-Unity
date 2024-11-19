//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose: Kills or damages a pawn if they are in too tight of a space for too long
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.PawnManagement;

public class Pawn_CrushDetection: MonoBehaviour
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
    [SerializeField] private Pawn pawn;
	
	
    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    }

    private void OnCollisionEnter(Collision other)
    {
	    print($"Colliding with {other.gameObject.name}");
	    pawn.ModifyHealth(-5);
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
	
	
    //=-----------------=
    // External Functions
    //=-----------------=
}
