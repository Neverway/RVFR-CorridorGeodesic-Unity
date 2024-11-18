//======== Neverway 2023 Project Script | Written by Arthur Aka Liz ===========
// 
// Type: Utility
// Purpose: Keep this object when changing scenes. Destroy the new instance
//  if the object exists in the next scene.
// Applied to: The persistent system manager
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework
{
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
        [IsDomainReloaded] private static GameObject instance;

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


        //=----Reload Static Fields----=
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitializeStaticFields()
        {
            instance = null;
        }
    }
}

