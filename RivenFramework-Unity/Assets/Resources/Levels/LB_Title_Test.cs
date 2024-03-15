//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: Adding a test comment for framework merging test
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LB_Title_Test : MonoBehaviour
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
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
