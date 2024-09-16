//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_LevelLoadManager_EventPlayer : MonoBehaviour
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
	
	
    //=-----------------=
    // Internal Functions
    //=-----------------=
	
	
    //=-----------------=
    // External Functions
    //=-----------------=
    public void LoadLevel(string levelID)
    {
        Game_LevelLoadManager.Instance.LoadLevel(levelID);
    }
}
