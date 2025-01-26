//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Display the image for the button for the corresponding action based on the current input device type
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

namespace Neverway.Framework.ApplicationManagement
{
    [RequireComponent(typeof(Image))]
    public class Image_KeyHint : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public string targetActionMap;
        public string targetAction;

        [Header(
            "0 - Display button for current input device, 1 - Keyboard, 2 - Controller, 3 - Motion Controller (VR)")]
        [Range(0, 2)]
        public int targetInputDevice;

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
            if (applicationKeybinds == null)
            {
                applicationKeybinds = FindObjectOfType<ApplicationKeybinds>();
                if (applicationKeybinds == null)
                {
                    Debug.LogWarning("Could not find ApplicationKeybinds in scene to update keybinds");
                    return;
                }
            }

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

                if (controlScheme == "Keyboard&Mouse" && targetInputDevice == 1 || controlScheme == "Keyboard&Mouse" &&
                    targetInputDevice == 0 && applicationKeybinds.currentDeviceID == 1)
                {
                    hintImage.sprite = applicationKeybinds.GetKeybindImage(1, bindingKey);
                }
                else if (controlScheme == "Gamepad" && targetInputDevice == 2 || controlScheme == "Gamepad" &&
                         targetInputDevice == 0 && applicationKeybinds.currentDeviceID == 2)
                {
                    hintImage.sprite = applicationKeybinds.GetKeybindImage(2, bindingKey);
                }
            }

        }


        //=-----------------=
        // External Functions
        //=-----------------=

    }
}