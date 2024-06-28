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
                if (action.name == targetAction)
                {
                    // Iterate through all bindings in the action
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        var binding = action.bindings[i];
                        // Check if it's a keyboard or gamepad binding
                        var controlScheme = binding.groups;

                        if (controlScheme.Contains("Keyboard") && targetInputDevice == 1)
                        {
                            // Set the current image to the relative binding

                            //Debug.Log($"    Keyboard Binding: {parts[1]}");
                            hintImage.sprite = applicationKeybinds.GetKeybindImage(1, binding.path.Replace("<Keyboard>/", ""));
                        }
                        else if (controlScheme.Contains("Gamepad") && targetInputDevice == 2)
                        {
                            // Set the current image to the relative binding

                            //Debug.Log($"    Controller Binding: {parts[1]}");
                            hintImage.sprite = applicationKeybinds.GetKeybindImage(2, binding.path.Replace("<Gamepad>/", ""));
                        }
                    }
                }
                else if (targetAction.Contains(action.name))
                {
                    // Iterate through all bindings in the action
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        var binding = action.bindings[i];

                        // Check if the binding is a composite binding
                        if (binding.isComposite)
                        {
                            //Debug.Log($"    Composite Binding: {binding.path}");

                            // Print each part of the composite binding
                            int partIndex = i + 1;
                            while (partIndex < action.bindings.Count && action.bindings[partIndex].isPartOfComposite)
                            {
                                var partBinding = action.bindings[partIndex];
                                var controlScheme = partBinding.groups;
                                //Debug.Log($" [COMP] Action: {action.name} CS: {controlScheme}");
                                if (controlScheme.Contains("Keyboard") && targetInputDevice == 1)
                                {
                                    string input = targetAction;
                                    string[] parts = input.Split(' ');
                                    if (partBinding.path.Contains(parts[1]))
                                    {
                                        hintImage.sprite = applicationKeybinds.GetKeybindImage(1, partBinding.path.Replace("<Keyboard>/", ""));
                                        //Debug.Log($"      Keyboard Part: {partBinding.path.Replace("<Keyboard>/", "")}");
                                    }
                                }
                                else if (controlScheme.Contains("Gamepad") && targetInputDevice == 2)
                                {
                                    string input = targetAction;
                                    string[] parts = input.Split(' ');
                                    if (partBinding.path.Contains(parts[1]))
                                    {
                                        hintImage.sprite = applicationKeybinds.GetKeybindImage(2, partBinding.path.Replace("<Gamepad>/", ""));
                                        //Debug.Log($"      Keyboard Part: {partBinding.path.Replace("<Gamepad>/", "")}");
                                    }
                                }
                                partIndex++;
                            }
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
