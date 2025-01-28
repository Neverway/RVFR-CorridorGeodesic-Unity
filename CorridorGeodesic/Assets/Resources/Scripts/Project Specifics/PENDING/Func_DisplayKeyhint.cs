//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.ApplicationManagement;
using Neverway.Framework.LogicSystem;

namespace Neverway
{
public class Func_DisplayKeyhint : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent inputSignal;
    public float duration = 3f;
    public string keyhintText = "Lemon Milk";
    
    [Header("Dynamic Assignment")]
    public string targetActionMap;
    public string targetAction;

    [Header("Manuel Assignment")] 
    public bool useManuelAssignment = false;
    public Sprite keyhintImage;
    

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        isPowered = powered;

        if (isPowered)
        {
            DisplayKeyHint();
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void DisplayKeyHint()
    {
        if (useManuelAssignment)
        {
            FindObjectOfType<GameplayNotificationManager>().DisplayKeyHint(duration, keyhintText, keyhintImage);
        }
        else
        {
            FindObjectOfType<GameplayNotificationManager>().DisplayKeyHint(duration, keyhintText, targetActionMap, targetAction);
        }
    }
}
}
