//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: Since a binding entry has a button for binding keyboard and controller
//     we'll only create a binding entry for each keyboard binding
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public GameObject headerObject;
    public GameObject bindingObject;
    public GameObject bindingWidget;
    


    //=-----------------=
    // Mono Functions
    //=-----------------=
    void Start()
    {
        applicationKeybinds = FindObjectOfType<ApplicationKeybinds>();
        
        // Check if the input action asset is assigned
        if (applicationKeybinds.inputActionAsset == null)
        {
            Debug.LogError("InputActionAsset is not assigned.");
            return;
        }

        // Iterate through all action maps
        foreach (var actionMap in applicationKeybinds.inputActionAsset.actionMaps)
        {
            // Only continue if the current action map is in the list of requested maps
            //Debug.Log($"Action Map: {actionMap.name}");
            if (!targetActionMaps.Contains(actionMap.name)) continue;
            
            // Iterate through all actions in the action map
            foreach (var action in actionMap.actions)
            {
                //Debug.Log($"  Action: {action.name}");

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
                            if (controlScheme.Contains("Keyboard"))
                            {
                                // Create a binding entry
                                var newBindingEntry = Instantiate(bindingObject, contentRoot.transform);
                                string partName = partBinding.name;
                                newBindingEntry.GetComponent<ControlBindingEntry>().text.text = $"{action.name} {partName}";
                                foreach (var keyHint in newBindingEntry.GetComponent<ControlBindingEntry>().keyHints)
                                {
                                    keyHint.targetAction = $"{action.name} {partName}";
                                }
                                newBindingEntry.SetActive(true);
                                //Debug.Log($"      Keyboard Part: {partBinding.path}");
                            }
                            else if (controlScheme.Contains("Gamepad"))
                            {
                                //Debug.Log($"      Controller Part: {partBinding.path}");
                            }
                            partIndex++;
                        }
                    }
                    else if (!binding.isPartOfComposite)
                    {
                        // Check if it's a keyboard or gamepad binding
                        var controlScheme = binding.groups;
                        //Debug.Log($" Action: {action.name} CS: {controlScheme}");
                        if (controlScheme.Contains("Keyboard"))
                        {
                            // Create a binding entry
                            var newBindingEntry = Instantiate(bindingObject, contentRoot.transform);
                            newBindingEntry.GetComponent<ControlBindingEntry>().text.text = action.name;
                            foreach (var keyHint in newBindingEntry.GetComponent<ControlBindingEntry>().keyHints)
                            {
                                keyHint.targetAction = action.name;
                            }
                            newBindingEntry.SetActive(true);
                            //Debug.Log($"    Keyboard Binding: {binding.path}");
                        }
                        else if (controlScheme.Contains("Gamepad"))
                        {
                            //Debug.Log($"    Controller Binding: {binding.path}");
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
    public void Rebind(string _actionMap, string _action)
    {
        GameInstance.AddWidget(bindingWidget);
        applicationKeybinds.SetBinding(_actionMap, _action);
        print($"{_actionMap} {_action}");
    }
}
