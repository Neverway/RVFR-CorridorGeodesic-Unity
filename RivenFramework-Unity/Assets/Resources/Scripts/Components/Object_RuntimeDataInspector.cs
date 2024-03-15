//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: This script allows us to look at and change variables in our game's objects while the game is running.
// Notes:
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Object_RuntimeDataInspector : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public List<ScriptData> scriptDataList; // MonoData class stores the target script, the variables we want to expose, 
    [HideInInspector] public List<VariableData> storedVariableData; // The variable's name, type, and value that we are storing in the runtime data inspector


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // MonoBehaviour Functions
    //=-----------------=

    
    //=-----------------=
    // Internal Functions
    //=-----------------=
    /// <summary>
    /// Changes the value of a specific variable in a MonoBehaviour script.
    /// </summary>
    /// <param name="_targetScript">The object where the variable is located.</param>
    /// <param name="_variableName">The name of the variable we want to change.</param>
    /// <param name="_value">The new value we want to assign to the variable.</param>
    // ADD FUNCTION TO FIGURE OUT WHICH SCRIPT THIS DATA IS GOING TO IF THE TARGET SCRIPT IS NULL
    public void SetVariableValue(MonoBehaviour _targetScript, string _variableName, string _value)
    {
        // If the object we're trying to change is not there, we need to find a replacement script.
        if (_targetScript == null)
        {
            // Iterate through each script data stored in the list
            foreach (var scriptData in scriptDataList)
            {
                // Check if the target script or its exposed variables are not assigned
                if (scriptData.targetScript == null || scriptData.exposedVariables == null) continue;

                // Check if the exposed variables of the current script data contain the name of the current variable data
                if (scriptData.exposedVariables.Contains(_variableName))
                {
                    // If we find the right script, we're in luck! Let's use it.
                    _targetScript = scriptData.targetScript;
                    break;
                }
            }
        }

        // We find out what type of object our script is.
        Type scriptType = _targetScript.GetType();

        // Then we find the specific variable we want to change in that script.
        FieldInfo field = scriptType.GetField(_variableName);

        // If we can't find the variable, we can't change it, so we stop here.
        if (field == null) return;

        // If the new value we want to give to the variable is empty and the variable is not a string, we don't do anything.
        if (_value == "" && field.FieldType != typeof(string)) return;

        // We convert the new value to match the type of the variable we're changing.
        object convertedValue = Convert.ChangeType(_value, field.FieldType);
            
        // Finally, we set the variable in our object to its new value.
        field.SetValue(_targetScript, convertedValue);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    /// <summary>
    /// Looks at the variables we can see in our game objects and creates a list of information about them.
    /// </summary>
    public void Inspect()
    {
        // If we don't have any objects to inspect, we stop here.
        if (scriptDataList == null)
        {
            return;
            
        }
        
        // We prepare a list to hold information about the variables we find.
        storedVariableData = new List<VariableData>();
        
        // We go through each object in our list.
        foreach (var scriptData in scriptDataList)
        {
            // If the object or the list of variables we want to look at is empty, we skip to the next one.
            if (scriptData.targetScript == null || scriptData.exposedVariables == null) continue;


            // Now, for each variable we want to look at in this object...
            foreach (var variable in scriptData.exposedVariables)
            {
                // We get information about that variable.
                var data = new VariableData();
                Type scriptType = scriptData.targetScript.GetType();
                FieldInfo field = scriptType.GetField(variable);

                // If we can't find the variable, we move on to the next one.
                if (field == null) continue;

                // We collect information about the variable, like its name and type.
                data.name = variable;
                data.type = field.FieldType.ToString();
                if (field.GetValue(scriptData.targetScript) != null) data.value = field.GetValue(scriptData.targetScript).ToString();
                storedVariableData.Add(data);
            }
        }
    }


    /// <summary>
    /// Sends the new values set for variables back to game objects.
    /// </summary>
    public void SendVariableDataToScripts()
    {
        // Check if the list of stored variable data is empty or not assigned
        if (storedVariableData == null) return;

        // Iterate through each variable data stored in the list
        foreach (var variableData in storedVariableData)
        {
            // Iterate through each script data stored in the list
            foreach (var scriptData in scriptDataList)
            {
                // Check if the target script or its exposed variables are not assigned
                if (scriptData.targetScript == null || scriptData.exposedVariables == null) continue;

                // Check if the exposed variables of the current script data contain the name of the current variable data
                if (scriptData.exposedVariables.Contains(variableData.name))
                {
                    // If the variable is found, call the SetVariableValue function to update its value in the target script
                    SetVariableValue(scriptData.targetScript, variableData.name, variableData.value);
                }
            }
        }
    }

}

[Serializable]
public class ScriptData
{
    public MonoBehaviour targetScript; // The script we want to communicate with
    public List<string> exposedVariables; // The variables from that script we want to be able to change in the runtime inspector
}

[Serializable]
public class VariableData
{
    public string name; // The name of the variable
    public string type; // The type of variable
    public string value; // The stored value of the variable
}