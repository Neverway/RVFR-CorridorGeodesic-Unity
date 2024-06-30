//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Display the image for the button for the corresponding action based on the current input device type
// Notes:
//
//=============================================================================

using System;
using UnityEngine;
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
        var action = applicationKeybinds.inputActionAsset.FindActionMap(targetActionMap).FindAction(targetAction);
        if (action==null) return; // This filters out composite bindings FYI
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            var controlScheme = binding.groups;
            
            string[] splitBinding = binding.path.Split("/");
            var bindingKey = splitBinding[1];
            
            if (controlScheme == "Keyboard&Mouse" && targetInputDevice == 1)
            {
                hintImage.sprite = applicationKeybinds.GetKeybindImage(1, bindingKey);
            }
            else if (controlScheme == "Gamepad" && targetInputDevice == 2)
            {
                hintImage.sprite = applicationKeybinds.GetKeybindImage(2, bindingKey);
            }
        }

    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
