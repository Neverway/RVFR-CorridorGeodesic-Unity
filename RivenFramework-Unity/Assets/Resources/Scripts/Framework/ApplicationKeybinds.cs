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
    [Tooltip("1-Keyboard, 2-Controller")]
    public int GetCurrentInputDevice()
    {        
        // Check for controller input
        string[] joystickNames = Input.GetJoystickNames();
        if (joystickNames.Length > 0) return 2;
        return 1;
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
                    {
                        return keybind.keybindSprite;
                    }
                }
                break;
            case 2:
                foreach (var keybind in controllerList)
                {
                    if (_keybindID.ToLower() == keybind.keybindID.ToLower())
                    {
                        return keybind.keybindSprite;
                    }
                }
                break;
        }
        Debug.LogWarning($"{_deviceID} {_keybindID}");
        return _image;
    }
}

[Serializable]
public class KeybindList
{
    public string keybindID;
    public Sprite keybindSprite;
}
