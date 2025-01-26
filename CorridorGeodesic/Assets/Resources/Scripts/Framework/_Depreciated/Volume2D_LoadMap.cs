//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.LogicSystem;
using Neverway.Framework.Cartographer;

public class Volume2D_LoadMap : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string levelID;
    public float exitOffsetX, exitOffsetY;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector3 exitOffset;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private new void OnTriggerEnter2D(Collider2D _other)
    {
        base.OnTriggerEnter2D(_other); // Call the base class method
        if (_other.CompareTag("Pawn") && GetPlayerInTrigger())
        {
            exitOffset = new Vector3(exitOffsetX, exitOffsetY);
            targetEntity.transform.position += exitOffset;
            FindObjectOfType<LevelManager>().LoadLevelFromMemory(levelID);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
