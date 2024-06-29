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
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;


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

    private void Update()
    {
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

    public void SetBinding(string _actionMap, string _action)
    {
        // Code courtesy of VoidSay
        // Source: https://forum.unity.com/threads/changing-inputactions-bindings-at-runtime.842188/
        // must dispose of collection or else you will have a memory leak and error with crash!
        rebindOperation?.Cancel();
 
        void CleanUp()
        {
            rebindOperation?.Dispose();
            rebindOperation = null;
        }

        var action = inputActionAsset.FindActionMap(_actionMap).FindAction(_action);
        print("We reached this point");
        action.PerformInteractiveRebinding().Start().OnCancel(something =>
            {
                Debug.Log("canceled");
                CleanUp();
            })
            .OnComplete(something =>
            {
                Debug.Log("finished");
                
                Destroy(GameInstance.GetWidget("WB_Settings_Controls_Rebinding"));
                // Save
                string device = string.Empty;
                string key = string.Empty;
                action.GetBindingDisplayString(0, out device, out key);
                print("OUTPUT "+"<" + device + ">/" + key);
                action.ChangeBinding(0).WithPath($"<{device}>/{key}");
                //PlayerPrefs.SetString(action.name, "<" + device + ">/" + key);
                //PlayerPrefs.Save();// not necessary if you close the application properly or want to implement confirmation button
                CleanUp();
            });
    }
 
    private void save()
    {
    }
}

[Serializable]
public class KeybindList
{
    public string keybindID;
    public Sprite keybindSprite;
}
