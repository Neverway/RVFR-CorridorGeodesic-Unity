//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Keeps this object when changing scenes and ensures there is only
//  ever one of them present in a scene
// Notes: 
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
        // Reload Static Fields
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitializeStaticFields()
        {
            instance = null;
        }
    }
}

