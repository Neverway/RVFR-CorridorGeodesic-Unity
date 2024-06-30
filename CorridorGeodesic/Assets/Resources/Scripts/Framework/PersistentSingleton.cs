//======== Neverway 2023 Project Script | Written by Arthur Aka Liz ===========
// 
// Type: Utility
// Purpose: Keep this object when changing scenes. Destroy the new instance
//  if the object exists in the next scene.
// Applied to: The persistent system manager
//
//=============================================================================

using UnityEngine;

public class PersistentSingleton : MonoBehaviour
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
    private static GameObject instance;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = gameObject;
        DontDestroyOnLoad(instance);
    }
    
    
    //=-----------------=
    // Internal Functions
    //=-----------------=
    
    
    //=-----------------=
    // External Functions
    //=-----------------=
}

