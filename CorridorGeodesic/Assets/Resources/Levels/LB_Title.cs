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

public class LB_Title : MonoBehaviour
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


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        gameInstance.UI_ShowTitle();
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
