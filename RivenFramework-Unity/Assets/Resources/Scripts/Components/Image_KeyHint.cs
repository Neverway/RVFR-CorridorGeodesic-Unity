//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Display the image for the button for the corresponding action based on the current input device type
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Image_KeyHint : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string targetActionMap;
    public string targetAction;
    [Header("0 - Display button for current input device, 1 - Keyboard, 2 - Controller, 3 - Motion Controller (VR)")]
    [Range(0,2)] public int targetInputDevice;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private ApplicationKeybinds applicationKeybinds;
    private Image hintImage; // UI Image to display the keybind sprite


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        applicationKeybinds = FindObjectOfType<ApplicationKeybinds>();
        hintImage = GetComponent<Image>();
    }

    private void Update()
    {
        UpdateKeyHint();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void UpdateKeyHint()
    {
        // Get the current input device

        // Get the control for the target action
        
        // Depending on the target input device, get the correct list of controls to images
        KeybindList keybind = null;/*
        if (currentControlScheme.Contains("Keyboard") || currentControlScheme.Contains("Mouse"))
        {
            //keybind = applicationKeybinds.controllerList.Find(k => k.keybindID == targetAction);
        }
        else if (currentControlScheme.Contains("Gamepad"))
        {
            //keybind = applicationKeybinds.keyboardList.Find(k => k.keybindID == targetAction);
        }

        // Set the image sprite
        if (keybind != null)
        {
            //hintImage.sprite = keybind.keybindSprite;
        }
        else
        {
            //Debug.LogWarning("Keybind not found for action: " + targetAction);
        }*/
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
