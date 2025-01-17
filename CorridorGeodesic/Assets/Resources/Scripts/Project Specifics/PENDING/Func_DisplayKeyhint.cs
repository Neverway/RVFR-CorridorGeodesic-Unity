//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.LogicSystem;

namespace Neverway
{
public class Func_DisplayKeyhint : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent inputSignal;
    
    [Header("Dynamic Assignment")]
    public string targetActionMap;
    public string targetAction;
    [Header("0 - Display button for current input device, 1 - Keyboard, 2 - Controller, 3 - Motion Controller (VR)")]
    [Range(0, 2)]
    public int targetInputDevice;

    [Header("Manuel Assignment")] 
    public bool useManuelAssignment = false;
    public string keyhintImage;
    public string keyhintText;
    

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
        // Find the game instance
        
        // Fire the create widget function for the keyhint
        
        // Pass the arguments
    }
}
}
