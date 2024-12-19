//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPropertiesFinder : MonoBehaviour
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
    public void AddToDisabledObjList(GameObject obj)
    {
        LevelProperties.Instance.AddToDisabledObjList(obj);
    }
    public void AddToEnabledObjList(GameObject obj)
    {
        LevelProperties.Instance.AddToEnabledObjList(obj);
    }
}
