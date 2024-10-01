//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Display a keyhint for a target action of a target input device
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Display a keyhint for a target action of a target input device
/// </summary>
[RequireComponent(typeof(Image))]
public class Image_KeyHint : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string targetActionMap;
    public string targetAction;
    public TargetInputDevice targetInputDevice;
    public enum TargetInputDevice
    {
        Current,
        Keyboard,
        Controller,
        MotionController,
    }


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
        applicationKeybinds = FindObjectOfType<ApplicationKeybinds>();
        var action = applicationKeybinds.inputActionAsset.FindActionMap(targetActionMap).FindAction(targetAction);
        // If action is null it's probably because the target action is a composite, so we'll need to parse the direction we want
        if (action == null)
        {
            // FUTURE ME PUT CODE HERE FOR DISPLAYING COMPOSITE BINDINGS!!!
            /*
            foreach (var inputAction in applicationKeybinds.inputActionAsset.FindActionMap(targetActionMap).actions)
            {
                if (inputAction.bindings.Count > 0 && inputAction.bindings[0].isComposite)
                {
                    foreach (var binding in inputAction.bindings)
                    {
                        if (binding.isPartOfComposite)
                        {
                            string pattern = @"/([^/\[]+)(?:/([^/\[]+))?";
                            Match match = Regex.Match(binding.path, pattern);

                            if (match.Success)
                            {
                                // Extract the matched part from the regex
                                var bindingKey = match.Groups[1].Value;
                                if (match.Groups[2].Success)
                                {
                                    bindingKey += "/" + match.Groups[2].Value;
                                }
                                Debug.Log($"{bindingKey}");

                                var controlScheme = binding.groups;

                                // Set the hint image based on the control scheme
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
                    }
                }
            }*/

            return;
        }
        // Since we got a return from action, we are not a composite, if we are abandoned all hope!
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            var controlScheme = binding.groups;

            string[] splitBinding = binding.path.Split("/");
            var bindingKey = splitBinding[1];

            switch (controlScheme)
            {
                case "Keyboard&Mouse" when targetInputDevice == TargetInputDevice.Keyboard:
                case "Keyboard&Mouse" when targetInputDevice == TargetInputDevice.Current && applicationKeybinds.currentDeviceID == 1:
                    hintImage.sprite = applicationKeybinds.GetKeybindImage(1, bindingKey);
                    break;
                case "Gamepad" when targetInputDevice == TargetInputDevice.Controller:
                case "Gamepad" when targetInputDevice == TargetInputDevice.Current && applicationKeybinds.currentDeviceID == 2:
                    hintImage.sprite = applicationKeybinds.GetKeybindImage(2, bindingKey);
                    break;
            }
        }

    }


    //=-----------------=
    // External Functions
    //=-----------------=

}
