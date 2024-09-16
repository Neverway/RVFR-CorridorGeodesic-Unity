//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_LevelManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
	public static Game_LevelManager Instance;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public Game_LevelStartIdentifier levelStartIdentifier;
    public Game_LevelStartIdentifier levelEndIdentifier;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        Instance = this;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}