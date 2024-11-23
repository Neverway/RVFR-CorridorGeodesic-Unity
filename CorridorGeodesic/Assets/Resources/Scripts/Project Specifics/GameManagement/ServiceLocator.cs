//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    //public Graphics_LocalEffectsManager localEffectsManager;

    //=-----------------=
    // Private Variables
    //=-----------------=
    private static ServiceLocator Instance;

    //=-----------------=
    // Reference Variables
    //=-----------------=


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
    //public static Graphics_LocalEffectsManager GetLocalEffectsManager()
    //{
    //    if (Instance.localEffectsManager == null)
    //    {
    //        Debug.LogError($"{nameof(Graphics_LocalEffectsManager)} is null on ServiceLocator. Please assign it");
    //        return null;
    //    }
    //    else
    //        return Instance.localEffectsManager;
    //}
}