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
    private string path;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;

    
    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        path = $"{Application.persistentDataPath}/settings.json";
        gameInstance = GetComponent<GameInstance>();
        //LoadControlls();
        //ApplyContolls();
    }


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
    public Sprite GetKeybindImage(int _deviceID, string _keybindID)
    {
        Sprite _image = null;
        switch (_deviceID)
        {
            case 0:
                break;
            case 1:
                foreach (var keybind in keyboardList)
                {
                    if (_keybindID.ToLower() == keybind.keybindID.ToLower())
                        return keybind.keybindSprite;
                }
                break;
            case 2:
                foreach (var keybind in controllerList)
                {
                    if (_keybindID.ToLower() == keybind.keybindID.ToLower())
                        return keybind.keybindSprite;
                }
                break;
        }
        return _image;
    }
}

[Serializable]
public class KeybindList
{
    public string keybindID;
    public Sprite keybindSprite;
}
