//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NEW_LogicProcessor))]
public class Func_GeoGunGiver : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public NEW_LogicProcessor inputSignal;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private NEW_LogicProcessor logicProcessor;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        logicProcessor = GetComponent<NEW_LogicProcessor>();
    }

    private void Update()
    {
        if (!inputSignal) return;
        logicProcessor.isPowered = inputSignal.isPowered;
        if (logicProcessor.hasPowerStateChanged)
        {
            if (logicProcessor.isPowered)
            {
                GiveGeoGun();
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void GiveGeoGun()
    {
        FindObjectOfType<Pawn_WeaponInventory>().GiveGeoGun();
    }
}
