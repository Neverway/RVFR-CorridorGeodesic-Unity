//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ToolFrameworkManagerItems : EditorWindow
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    private const string ActorsFolder = "Assets/Resources/Actors/Items";
    private List<string> actors = new List<string>();
    private string newItemName = "";


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=

    
    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void Window(ToolFrameworkManager _frameworkManagerWindow)
    {
        // Description
        EditorGUILayout.HelpBox("Add and modify the items that appear in your project. This will only handel the scriptable object data, you will still need to create a prefab with the same name as the item in /Resources/Actors/Objects! Also don't forget to add each of these scriptables to a item group in the GameInstance prefab's AssetData script found in /Resources/Actors/System. (Sorry, I know this a bit clunky right now but in the future this tool will be able to assign actors to the asset database. Good Luck! ~Liz)", MessageType.None);
        GUILayout.Space(10);
        
        // Headers
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Icon", GUILayout.Width(65));
        EditorGUILayout.LabelField("Scriptable", GUILayout.MinWidth(135));
        EditorGUILayout.LabelField("ID", GUILayout.MinWidth(100));
        EditorGUILayout.LabelField("Actor Name", GUILayout.MinWidth(100));
        EditorGUILayout.LabelField("Associated Prefab", GUILayout.MinWidth(100));
        EditorGUILayout.LabelField("Stack Count", GUILayout.MinWidth(100));
        EditorGUILayout.LabelField("Description", GUILayout.MinWidth(100));
        EditorGUILayout.LabelField("", GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        
        // Actor list
        _frameworkManagerWindow.scrollPosition = EditorGUILayout.BeginScrollView(_frameworkManagerWindow.scrollPosition);
        GetActorList();
        EditorGUILayout.EndScrollView();
        GUILayout.Space(10);
        
        // New actor button
        EditorGUILayout.BeginHorizontal();
        string[] types = new string[]
        {
            "Utility Throwable",
            "Utility Consumable",
            "Weapon Bladed",
            "Weapon Bludgeoning",
            "Weapon Polearm",
            "Weapon Projectile",
            "Weapon Hitscan",
            "Magic",
            "Defense",
            "Defense Head",
            "Defense Chest",
            "Defense Back",
            "Defense Legs",
            "Defense Feet",
            "Other"
        };
        int selectedIndex = Array.IndexOf(types, _frameworkManagerWindow.selectedItemType);
        selectedIndex = EditorGUILayout.Popup("Type", selectedIndex, types);
        _frameworkManagerWindow.selectedItemType = types[selectedIndex];
        newItemName = EditorGUILayout.TextField(newItemName);
        if (GUILayout.Button("Create New Item") && !string.IsNullOrEmpty(newItemName)) { CreateNewItem(_frameworkManagerWindow, newItemName); newItemName = ""; }
        EditorGUILayout.EndHorizontal();
        
        // Fix ... buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Fix IDs")) { FixItemIDs(_frameworkManagerWindow); }
        if (GUILayout.Button("Fix Actor Names")) { FixItemActorNames(_frameworkManagerWindow); }
        if (GUILayout.Button("Fix Associated Prefabs")) { FixItemAssociatedPrefabs(_frameworkManagerWindow); }
        EditorGUILayout.EndHorizontal();
        
        // Issue check and back buttons
        if (GUILayout.Button("Check for Issues")) {  }
        GUILayout.Space(10); 
        if (GUILayout.Button("Back")) { _frameworkManagerWindow.currentWindow = "Home"; }
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
        string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
        foreach (string guid in guidList)
        {
            Item item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
            if (item is null) continue;
            actors.Add(item.id);
            DisplayActorField(item);
        }
    }
    
    /// <summary>
    /// Display the fields for the actor
    /// </summary>
    /// <param name="_item"></param>
    private void DisplayActorField(Item _item)
    {
        EditorGUILayout.BeginHorizontal();
        
        // Icon preview
        if (_item.icon)
        {
            Texture2D itemImage = _item.icon.texture;
            GUILayout.Label(itemImage, GUILayout.Width(25), GUILayout.Height(25));
        }
        else
        {
            GUILayout.Label(GUIContent.none, GUILayout.Width(25), GUILayout.Height(25));
        }

        // Icon Field
        _item.icon = (Sprite)EditorGUILayout.ObjectField(_item.icon, typeof(Sprite), false, GUILayout.Width(35));
        
        // Scriptable
        EditorGUILayout.ObjectField(_item, typeof(Item), false, GUILayout.MinWidth(100));

        // Id Field
        _item.id = EditorGUILayout.TextField(_item.id);

        // Actor Name Field
        _item.actorName = EditorGUILayout.TextField(_item.actorName);

        // Prefab Field
        _item.AssociatedGameObject = (GameObject)EditorGUILayout.ObjectField(_item.AssociatedGameObject, typeof(GameObject), false);

        // Stack Count Field
        _item.stackCount = EditorGUILayout.IntField(_item.stackCount);

        // Description Field
        _item.description = EditorGUILayout.TextField(_item.description);

        // Delete button
        if (GUILayout.Button("Delete", GUILayout.Width(60))) 
        { 
            if (EditorUtility.DisplayDialog("Delete Item", "Are you sure you want to delete this item?", "Yes", "No"))
            {
                DeleteItem(_item);
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    
    private void FixItemIDs(ToolFrameworkManager _frameworkManagerWindow)
    {
        string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
        foreach (var guid in guidList)
        {
            Item item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
            if (item is not null)
            {
                item.id = _frameworkManagerWindow.GenerateIdFromActor(item, "item");
            }
        }
    }
    
    private void FixItemActorNames(ToolFrameworkManager _frameworkManagerWindow)
    {
        string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
        foreach (string guid in guidList)
        {
            Item item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
            if (item is not null)
            {
                item.actorName = _frameworkManagerWindow.GenerateActorNameFromActor(item);
            }
        }
    }
    
    private void FixItemAssociatedPrefabs(ToolFrameworkManager _frameworkManagerWindow)
    {
        string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
        foreach (string guid in guidList)
        {
            Item item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
            if (item is not null)
            {
                item.AssociatedGameObject = _frameworkManagerWindow.GetPrefabAtPath(item.name, ToolFrameworkManager.ObjectsFolder);
            }
        }
    } 
    
    private void DeleteItem(Item _item)
    {
        string assetPath = AssetDatabase.GetAssetPath(_item);
        AssetDatabase.DeleteAsset(assetPath);
        actors.Remove(_item.id);
    }

    private void CreateNewItem(ToolFrameworkManager _frameworkManagerWindow, string _itemName)
    {
        Item newItem = _frameworkManagerWindow.selectedItemType switch
        {
            "Utility Throwable" => CreateInstance<Item_Utility_Throwable>(),
            "Utility Consumable" => CreateInstance<Item_Utility_Consumable>(),
            "Weapon Bladed" => CreateInstance<Item_Weapon_Bladed>(),
            "Weapon Bludgeoning" => CreateInstance<Item_Weapon_Bludgeoning>(),
            "Weapon Polearm" => CreateInstance<Item_Weapon_Polearm>(),
            "Weapon Projectile" => CreateInstance<Item_Weapon_Projectile>(),
            "Weapon Hitscan" => CreateInstance<Item_Weapon_Hitscan>(),
            "Magic" => CreateInstance<Item_Magic>(),
            "Defense" => CreateInstance<Item_Defense>(),
            "Defense Head" => CreateInstance<Item_Defense_Head>(),
            "Defense Chest" => CreateInstance<Item_Defense_Chest>(),
            "Defense Back" => CreateInstance<Item_Defense_Back>(),
            "Defense Legs" => CreateInstance<Item_Defense_Legs>(),
            "Defense Feet" => CreateInstance<Item_Defense_Feet>(),
            _ => CreateInstance<Item>()
        };

        if (newItem is null)
        {
            Debug.LogError("Failed to create new item: unknown type " + _frameworkManagerWindow.selectedItemType);
            return;
        }
    
        // Set the name of the new item
        newItem.name = _itemName;
    
        // Create asset file for the new item
        string assetPath = $"{ActorsFolder}/{_itemName}.asset";
        AssetDatabase.CreateAsset(newItem, assetPath);
    }
}
