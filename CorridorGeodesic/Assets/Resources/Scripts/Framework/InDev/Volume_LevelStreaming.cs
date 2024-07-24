//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Volume_LevelStreaming : Volume
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


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private new void OnTriggerEnter2D(Collider2D _other)
    {
        DontDestroyOnLoad(_other.gameObject);
    }
    
    private new void OnTriggerEnter(Collider _other)
    {
        DontDestroyOnLoad(_other.gameObject);
    }
    
    private new void OnTriggerExit2D(Collider2D _other)
    {
        SceneManager.MoveGameObjectToScene(_other.gameObject, SceneManager.GetActiveScene());
    }
    
    private new void OnTriggerExit(Collider _other)
    {
        SceneManager.MoveGameObjectToScene(_other.gameObject, SceneManager.GetActiveScene());
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
