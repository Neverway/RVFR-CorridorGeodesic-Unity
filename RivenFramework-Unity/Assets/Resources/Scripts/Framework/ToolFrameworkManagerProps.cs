//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ToolFrameworkManagerProps : EditorWindow
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    private const string ActorsFolder = "Assets/Resources/Actors/Props";
    private List<string> actors = new List<string>();
    private string newPropName = "";


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=

    
    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void Window(ToolFrameworkManager _FrameworkManagerWindow)
    {
        // Description
        EditorGUILayout.HelpBox("Add and modify the assets that appear in your project. This will only handel the scriptable object data, you will still need to create a prefab with the same name as the prop in /Resources/Actors/Objects! Also don't forget to add each of these scriptables to a prop group in the GameInstance prefab's AssetData script found in /Resources/Actors/System. (Sorry, I know this a bit clunky right now but in the future this tool will be able to assign actors to the asset database. Good Luck! ~Liz)", MessageType.None);
        GUILayout.Space(10);
        
        // Headers
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Icon", GUILayout.Width(65));
        EditorGUILayout.LabelField("Scriptable", GUILayout.MinWidth(135));
        EditorGUILayout.LabelField("ID", GUILayout.MinWidth(100));
        EditorGUILayout.LabelField("Actor Name", GUILayout.MinWidth(100));
        EditorGUILayout.LabelField("Associated Prefab", GUILayout.MinWidth(100));
        EditorGUILayout.LabelField("", GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        
        // Actor list
        _FrameworkManagerWindow.scrollPosition = EditorGUILayout.BeginScrollView(_FrameworkManagerWindow.scrollPosition);
        GetActorList();
        EditorGUILayout.EndScrollView();
        GUILayout.Space(10);
        
        // New actor button
        EditorGUILayout.BeginHorizontal();
        newPropName = EditorGUILayout.TextField(newPropName);
        if (GUILayout.Button("Create New Prop") && !string.IsNullOrEmpty(newPropName)) { CreateNewProp(newPropName); newPropName = ""; }
        EditorGUILayout.EndHorizontal();
        
        // Fix ... buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Fix IDs")) { FixPropIDs(_FrameworkManagerWindow); }
        if (GUILayout.Button("Fix Actor Names")) { FixPropActorNames(_FrameworkManagerWindow); }
        if (GUILayout.Button("Fix Associated Prefabs")) { FixPropAssociatedPrefabs(_FrameworkManagerWindow); }
        EditorGUILayout.EndHorizontal();
        
        // Issue check and back buttons
        if (GUILayout.Button("Check for Issues")) {  }
        GUILayout.Space(10); 
        if (GUILayout.Button("Back")) { _FrameworkManagerWindow.currentWindow = "Home"; }
    }
    
    
    //=-----------------=
    // Window Functions
    //=-----------------=
    /// <summary>
    /// Find all the scriptable objects in the specified asset folder
    /// </summary>
    private void GetActorList()
    {
        actors.Clear();
        var guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
        foreach (var guid in guidList)
        {
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
            if (prop != null)
            {
                actors.Add(prop.id);
                DisplayActorField(prop);
            }
        }
    }
    
    /// <summary>
    /// Display the fields for the actor
    /// </summary>
    /// <param name="_prop"></param>
    private void DisplayActorField(Prop _prop)
    {
        EditorGUILayout.BeginHorizontal();
        
        // Icon preview
        if (_prop.icon)
        {
            Texture2D propImage = _prop.icon.texture;
            GUILayout.Label(propImage, GUILayout.Width(25), GUILayout.Height(25));
        }
        else
        {
            GUILayout.Label(GUIContent.none, GUILayout.Width(25), GUILayout.Height(25));
        }

        // Icon Field
        _prop.icon = (Sprite)EditorGUILayout.ObjectField(_prop.icon, typeof(Sprite), false, GUILayout.Width(35));
        
        // Scriptable
        EditorGUILayout.ObjectField(_prop, typeof(Prop), false, GUILayout.MinWidth(100));

        // Id Field
        _prop.id = EditorGUILayout.TextField(_prop.id);

        // Actor Name Field
        _prop.actorName = EditorGUILayout.TextField(_prop.actorName);

        // Prefab Field
        _prop.AssociatedGameObject = (GameObject)EditorGUILayout.ObjectField(_prop.AssociatedGameObject, typeof(GameObject), false);

        // Delete button
        if (GUILayout.Button("Delete", GUILayout.Width(60))) 
        { 
            if (EditorUtility.DisplayDialog("Delete Prop", "Are you sure you want to delete this prop?", "Yes", "No"))
            {
                DeleteProp(_prop);
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    
    private void FixPropIDs(ToolFrameworkManager _FrameworkManagerWindow)
    {
        var guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
        foreach (var guid in guidList)
        {
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
            if (prop != null)
            {
                prop.id = _FrameworkManagerWindow.GenerateIdFromActor(prop, "prop");
            }
        }
    }
    
    private void FixPropActorNames(ToolFrameworkManager _FrameworkManagerWindow)
    {
        var guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
        foreach (var guid in guidList)
        {
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
            if (prop != null)
            {
                prop.actorName = _FrameworkManagerWindow.GenerateActorNameFromActor(prop);
            }
        }
    }
    
    private void FixPropAssociatedPrefabs(ToolFrameworkManager _FrameworkManagerWindow)
    {
        var guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
        foreach (var guid in guidList)
        {
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
            if (prop != null)
            {
                prop.AssociatedGameObject = _FrameworkManagerWindow.GetPrefabAtPath(prop.name, ToolFrameworkManager.ObjectsFolder);
            }
        }
    } 
    
    private void DeleteProp(Prop _prop)
    {
        string assetPath = AssetDatabase.GetAssetPath(_prop);
        AssetDatabase.DeleteAsset(assetPath);
        actors.Remove(_prop.id);
    }

    private void CreateNewProp(string _propName)
    {
        // Create a new instance of the Prop scriptable object
        Prop newProp = CreateInstance<Prop>();
    
        // Set the name of the new prop
        newProp.name = _propName;
    
        // Create asset file for the new prop
        string assetPath = $"{ActorsFolder}/{_propName}.asset";
        AssetDatabase.CreateAsset(newProp, assetPath);
    }
}
