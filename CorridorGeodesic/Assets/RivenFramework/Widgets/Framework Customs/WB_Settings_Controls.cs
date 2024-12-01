//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Settings sub-widget for changing graphics options
// Notes: Since a binding entry has a button for binding keyboard and controller
//     we'll only create a binding entry for each keyboard binding
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework.ApplicationManagement
{
    public class WB_Settings_Controls : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public List<string> targetActionMaps;
        public string b;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=
        public ApplicationKeybinds applicationKeybinds;
        public GameObject contentRoot;
        public GameObject bindingObject;
        public GameObject bindingWidget;



        //=-----------------=
        // Mono Functions
        //=-----------------=
        void Start()
        {
            //Debug.Log($"[{this.name}] Executing function 'Start()'");
            applicationKeybinds = FindObjectOfType<ApplicationKeybinds>();

            // Check if the input action asset is assigned
            if (applicationKeybinds.inputActionAsset == null)
            {
                //Debug.LogError($"[{this.name}] applicationKeybinds.inputActionAsset is not assigned.");
                return;
            }

            // Iterate through all action maps
            foreach (var actionMap in applicationKeybinds.inputActionAsset.actionMaps)
            {
                // Only continue if the current action map is in the list of requested maps
                //Debug.Log($"Action Map: {actionMap.name}");
                if (!targetActionMaps.Contains(actionMap.name)) continue;
                //Debug.Log($"[{this.name}] In targetActionMaps found {actionMap.name}");

                // Iterate through all actions in the action map
                foreach (var action in actionMap.actions)
                {
                    //Debug.Log($"  Action: {action.name}");
                    //Debug.Log($"[{this.name}] In actionMap.actions found {action.name}");

                    // Iterate through all bindings in the action
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        var binding = action.bindings[i];

                        // Check if the binding is a composite binding
                        if (binding.isComposite)
                        {
                            //Debug.Log($"    Composite Binding: {binding.path}");
                            //Debug.Log($"[{this.name}] {action.name} is a composite binding with path '{binding.path}'");

                            // Print each part of the composite binding
                            int partIndex = i + 1;
                            while (partIndex < action.bindings.Count && action.bindings[partIndex].isPartOfComposite)
                            {
                                var partBinding = action.bindings[partIndex];
                                var controlScheme = partBinding.groups;
                                //Debug.Log($" [COMP] Action: {action.name} CS: {controlScheme}");
                                if (controlScheme.Contains("Keyboard"))
                                {
                                    // Create a binding entry
                                    var newBindingEntry = Instantiate(bindingObject, contentRoot.transform);
                                    string partName = partBinding.name;
                                    newBindingEntry.GetComponent<ControlBindingEntry>().text.text =
                                        $"{action.name} {partName}";
                                    newBindingEntry.GetComponent<ControlBindingEntry>().isComposite = true;
                                    foreach (var keyHint in newBindingEntry.GetComponent<ControlBindingEntry>()
                                                 .keyHints)
                                    {
                                        keyHint.targetAction = $"{action.name} {partName}";
                                    }
                                    //newBindingEntry.SetActive(true);
                                    //Debug.Log($"      Keyboard Part: {partBinding.path}");
                                    //Debug.Log($"[{this.name}] {action.name} contains binding path '{partBinding.path}'");
                                }
                                else if (controlScheme.Contains("Gamepad"))
                                {
                                    //Debug.Log($"      Controller Part: {partBinding.path}");
                                    //Debug.Log($"[{this.name}] {action.name} contains binding path '{partBinding.path}'");
                                }

                                partIndex++;
                            }
                        }
                        else if (!binding.isPartOfComposite)
                        {
                            //Debug.Log($"[{this.name}] {action.name} is not a composite binding");
                            // Check if it's a keyboard or gamepad binding
                            var controlScheme = binding.groups;
                            //Debug.Log($" Action: {action.name} CS: {controlScheme}");
                            if (controlScheme.Contains("Keyboard"))
                            {
                                // Create a binding entry
                                var newBindingEntry = Instantiate(bindingObject, contentRoot.transform);
                                newBindingEntry.GetComponent<ControlBindingEntry>().text.text = action.name;
                                newBindingEntry.GetComponent<ControlBindingEntry>().isComposite = false;
                                foreach (var keyHint in newBindingEntry.GetComponent<ControlBindingEntry>().keyHints)
                                {
                                    keyHint.targetAction = action.name;
                                }

                                newBindingEntry.SetActive(true);
                                //Debug.Log($"    Keyboard Binding: {binding.path}");
                                //Debug.Log($"[{this.name}] {action.name} contains binding path '{binding.path}'");
                            }
                            else if (controlScheme.Contains("Gamepad"))
                            {
                                //Debug.Log($"    Controller Binding: {binding.path}");
                                //Debug.Log($"[{this.name}] {action.name} contains binding path '{binding.path}'");
                            }
                        }
                    }
                }
            }
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
        public void Rebind(string _actionMap, string _action, bool _isComposite)
        {
            Debug.Log(
                $"[{this.name}] Executing function 'Rebind(actionMap={_actionMap}, action={_action}, isComposite={_isComposite})'");
            GameInstance.AddWidget(bindingWidget);
            applicationKeybinds.SetBinding(_actionMap, _action, _isComposite);
        }
    }
}