//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Stores the lists of input actions to button hint sprites.
//          Also holds functions for rebinding controls
// Notes: Originally this was supposed to also handel saving and loading the controls
//        to a file like controls.config, but I guess unity already saves them somewhere?
//
//=============================================================================

using System;
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
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;


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

    public void SetBinding(string _actionMap, string _action, bool _isComposite)
    {
        Debug.Log($"[{this.name}] Executing function 'SetBinding(actionMap={_actionMap}, action={_action}, isComposite={_isComposite})'");
        // Code courtesy of VoidSay
        // Source: https://forum.unity.com/threads/changing-inputactions-bindings-at-runtime.842188/
        // must dispose of collection or else you will have a memory leak and error with crash!
        rebindOperation?.Cancel();

        void CleanUp()
        {
            rebindOperation?.Dispose();
            rebindOperation = null;
        }

        if (!_isComposite)
        {
            Debug.Log("Not a composite");
            var action = inputActionAsset.FindActionMap(_actionMap).FindAction(_action);
            print($"_AM [{_actionMap}] _A[{_action}] A[{action}]");
            rebindOperation = action.PerformInteractiveRebinding().Start()
            .OnCancel(something =>
            {
                CleanUp();
            })
            .OnComplete(something =>
            {
                Destroy(GameInstance.GetWidget("WB_Settings_Controls_Rebinding"));
                // Save
                string device = string.Empty;
                string key = string.Empty;
                action.GetBindingDisplayString(0, out device, out key);
                print("OUTPUT "+"<" + device + ">/" + key);
                action.ChangeBinding(0).WithPath($"<{device}>/{key}");
                CleanUp();
            });
        }
        else
        {
            Debug.Log("Composite");
            string input = _action;
            string[] parts = input.Split(' ');
            var parsedAction = parts[0];
            var action = inputActionAsset.FindActionMap(_actionMap).FindAction(parsedAction);
            print($"_AM [{_actionMap}] _A[{_action}] A[{action}] PA[{parsedAction}]");
            rebindOperation = action.PerformInteractiveRebinding(1).Start()
            .OnCancel(something =>
            {
                CleanUp();
            })
            .OnComplete(something =>
            {
                Destroy(GameInstance.GetWidget("WB_Settings_Controls_Rebinding"));
                // Save
                string device = string.Empty;
                string key = string.Empty;
                action.GetBindingDisplayString(1, out device, out key);
                print("OUTPUT "+"<" + device + ">/" + key);
                action.ChangeBinding(1).WithPath($"<{device}>/{key}");
                CleanUp();
            });
        }
    }
}

[Serializable]
public class KeybindList
{
    public string keybindID;
    public Sprite keybindSprite;
}
