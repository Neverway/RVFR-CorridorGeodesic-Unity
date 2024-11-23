//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using Neverway.Framework.LogicSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume_ChangeLocalEffect : Volume_NEW
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private LocalEffectSetting localEffect;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=
    public override void OnPawnEntered(Collider other)
    {
        Graphics_LocalEffectsManager.Instance.SetEffectSetting(localEffect);
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}