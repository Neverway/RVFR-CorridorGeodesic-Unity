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
        // Iterate through all action maps
        foreach (var actionMap in applicationKeybinds.inputActionAsset.actionMaps)
        {
            // Get target actionMap
            if (actionMap.name != targetActionMap) continue;
            
            // Iterate through all actions in the action map
            foreach (var action in actionMap.actions)
            {
                // Get target action
                if (action.name != targetAction) continue;
                
                // Iterate through all bindings in the action
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];

                    // Check if the binding is a composite binding
                    if (binding.isComposite)
                    {
                        // Print each part of the composite binding
                        int partIndex = i + 1;
                        while (partIndex < action.bindings.Count && action.bindings[partIndex].isPartOfComposite)
                        {
                            var partBinding = action.bindings[partIndex];
                            var controlScheme = partBinding.groups;
                            if (controlScheme.Contains("Keyboard"))
                            {
                            }
                            else if (controlScheme.Contains("Gamepad"))
                            {
                            }
                            partIndex++;
                        }
                    }
                    else if (!binding.isPartOfComposite)
                    {
                        // Check if it's a keyboard or gamepad binding
                        var controlScheme = binding.groups;
                        
                        if (controlScheme.Contains("Keyboard") && targetInputDevice == 1)
                        {
                            // Set the current image to the relative binding
                            string originalString = binding.path;
                            string[] parts = originalString.Split('/');
                            
                            Debug.Log($"    Keyboard Binding: {parts[1]}");
                            hintImage.sprite = applicationKeybinds.GetKeybindImage(1, parts[1]);
                        }
                        else if (controlScheme.Contains("Gamepad") && targetInputDevice == 2)
                        {
                            
                        }
                    }
                }
            }
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
