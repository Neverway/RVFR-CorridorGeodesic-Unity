//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ApplicationKeybinds : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public InputActionAsset inputActionAsset;
    public List<KeybindList> keyboardList;
    public List<KeybindList> controllerList;


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
    public string GetCurrentInputDevice()
    {
        /*
        if (InputSystem.devices.Count > 0)
        {
            var currentControlScheme = InputSystem.devices[0].device.ToString();
        }
        else
        {
            currentControlScheme = "Keyboard";
        }
        //print(currentControlScheme);*/
        return null;
    }
}

[Serializable]
public class KeybindList
{
    public string keybindID;
    public Sprite keybindSprite;
}
