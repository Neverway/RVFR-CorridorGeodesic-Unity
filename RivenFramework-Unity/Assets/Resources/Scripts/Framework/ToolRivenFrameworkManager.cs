//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

public class ToolRivenFrameworkManager : EditorWindow
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private string currentWindow = "Home";
    private Vector2 scrollPosition = Vector2.zero;
    private const string SystemObjectsFolder = "Assets/Resources/Actors/System";
    private const string ObjectsFolder = "Assets/Resources/Actors/Objects";
    
    private const string PropsFolder = "Assets/Resources/Actors/Props";
    private List<string> props = new List<string>();
    private string newPropName = "";
    
    private const string ItemsFolder = "Assets/Resources/Actors/Items";
    private List<string> items = new List<string>();
    private string newItemName = "";
    private string selectedType = "Utility Throwable";


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    [MenuItem("Neverway/Framework Manager")]
    public static void ShowWindow()
    {
        GetWindow<ToolRivenFrameworkManager>("Riven Framework Manager");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Framework Version: 1.0.0a");
        HeaderImage();
        EditorGUILayout.LabelField(currentWindow, EditorStyles.boldLabel);
        switch (currentWindow)
        {
            case "Home":
                Home();
                break;
            case "ProjectSetup":
                ProjectSetup();
                break;
            case "PropManager":
                PropManager();
                break;
            case "ItemManager":
                ItemManager();
                break;
            default:
                Missing();
                break;
        }
    }

    //=-----------------=
    // Window Functions
    //=-----------------=

    private void Home()
    {
        EditorGUILayout.HelpBox("Welcome to the Riven Framework Manager! This window will allow you to access a variety of tools for setting up your Riven Framework project!", MessageType.None);
        GUILayout.Space(10); 
        if (GUILayout.Button("Project Setup")) { currentWindow = "ProjectSetup"; }
        GUILayout.Space(10); 
        if (GUILayout.Button("Prop Manager")) { currentWindow = "PropManager"; }
        if (GUILayout.Button("Item Manager")) { currentWindow = "ItemManager"; }
        if (GUILayout.Button("Character Manager")) { currentWindow = "CharacterManager"; }
    }

    private void Missing()
    {
        EditorGUILayout.HelpBox("Whoops, it looks like you reached a dead end! This section may not be complete or an issue may have occured. Please report this to a developer.", MessageType.Error);
        GUILayout.Space(10); 
        if (GUILayout.Button("Back")) { currentWindow = "Home"; }
    }

    private void ProjectSetup()
    {
        EditorGUILayout.HelpBox("", MessageType.None);
        GUILayout.Space(10); 

        // Game instance settings
        if (GetPrefabAtPath("GameInstance", SystemObjectsFolder) != null)
        {
            GameInstance gameInstance = GetPrefabAtPath("GameInstance", SystemObjectsFolder).GetComponent<GameInstance>();
            if (gameInstance != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Default Gamemode:", GUILayout.Width(120));
                EditorGUILayout.ObjectField(gameInstance.defaultGamemode, typeof(GameMode), false);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("HUD Widget:", GUILayout.Width(120));
                EditorGUILayout.ObjectField(gameInstance.UserInterfaceWidgets[4], typeof(GameObject), false);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("HUD Inventory:", GUILayout.Width(120));
                EditorGUILayout.ObjectField(gameInstance.UserInterfaceWidgets[5], typeof(GameObject), false);
                EditorGUILayout.EndHorizontal();
            }
            else { Debug.LogError("GameInstance component not found on the prefab."); }
            /* TODO Finish this section
            GUILayout.Space(10); 
            
            EditorGUILayout.LabelField("Project Assets", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("These are the assets that will be usable by the system when loading or saving maps during runtime and in the Cartographer level editor.", MessageType.None);
            ProjectData projectData = GetPrefabAtPath("GameInstance", "Assets/Resources/Actors/System").GetComponent<ProjectData>();
            if (projectData != null)
            {
                // Ensure the size of tileGroupExpandedStates matches the number of tile groups
                while (tileGroupExpandedStates.Count < projectData.tiles.Count)
                {
                    tileGroupExpandedStates.Add(false); // Add default state for new tile groups
                }
                
                // Display Tile groups
                EditorGUILayout.LabelField("Tile Groups:");
                for (int i = 0; i < projectData.tiles.Count; i++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox); // Begin a vertical group with a border
                
                    // Display tile group name and allow editing
                    EditorGUILayout.BeginHorizontal();
                    projectData.tiles[i].name = EditorGUILayout.TextField(projectData.tiles[i].name);
                    if (GUILayout.Button("Delete", GUILayout.Width(60)))
                    {
                        projectData.tiles.RemoveAt(i);
                        tileGroupExpandedStates.RemoveAt(i); // Remove corresponding state
                        i--;
                        continue; // Skip further processing for this iteration
                    }
                    EditorGUILayout.EndHorizontal();

                    // Check if the tile group is expanded based on its index in tileGroupExpandedStates
                    tileGroupExpandedStates[i] = EditorGUILayout.Foldout(tileGroupExpandedStates[i], "Tiles");
                    if (tileGroupExpandedStates[i])
                    {
                        EditorGUI.indentLevel++; // Increase the indent level for nested controls
                        foreach (var tile in projectData.tiles[i].tiles)
                        {
                            EditorGUILayout.LabelField(tile.name);
                            // Add further tile information display here if needed
                        }
                        EditorGUI.indentLevel--; // Reset the indent level
                    }
                
                    EditorGUILayout.EndVertical(); // End the vertical group
                }
                // Add new tile group
                if (GUILayout.Button("Add Tile Group"))
                {
                    projectData.tiles.Add(new TileMemoryGroup());
                }
                // ADD Prop groups
                // ADD item groups
                // ADD character groups
                // ADD sprites groups
                
            }
            else { Debug.LogError("ProjectData component not found on the prefab."); }*/
        }
        GUILayout.Space(10); 
        
        if (GUILayout.Button("Back"))
        {
            currentWindow = "Home";
        }
    }
    
    private void PropManager()
    {
        EditorGUILayout.HelpBox("Add and modify the props that appear in your project. This will only handel the scriptable object data, you will still need to create a prefab with the same name as the prop in /Resources/Actors/Objects! Also don't forget to add each of these scriptables to a prop group in the GameInstance prefab's AssetData script found in /Resources/Actors/System. (Sorry, I know this a bit clunky right now but in the future this tool will be able to assign actors to the asset database. Good Luck! ~Liz)", MessageType.None);
        GUILayout.Space(10);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        GetPropList();
        EditorGUILayout.EndScrollView();
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        newPropName = EditorGUILayout.TextField(newPropName);
        // New Prop button
        if (GUILayout.Button("Create New Prop") && !string.IsNullOrEmpty(newPropName)) { CreateNewProp(newPropName); newPropName = ""; }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Fix IDs")) { FixPropIDs(); }
        if (GUILayout.Button("Fix Actor Names")) { FixPropActorNames(); }
        if (GUILayout.Button("Fix Associated Prefabs")) { FixPropAssociatedPrefabs(); }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Check for Issues")) {  }
        
        GUILayout.Space(10); 
        
        if (GUILayout.Button("Back")) { currentWindow = "Home"; }
    }
    
    private void ItemManager()
    {
        EditorGUILayout.HelpBox("Add and modify the items that appear in your project. This will only handel the scriptable object data, you will still need to create a prefab with the same name as the item in /Resources/Actors/Objects! Also don't forget to add each of these scriptables to a item group in the GameInstance prefab's AssetData script found in /Resources/Actors/System. (Sorry, I know this a bit clunky right now but in the future this tool will be able to assign actors to the asset database. Good Luck! ~Liz)", MessageType.None);
        GUILayout.Space(10);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        GetItemList();
        EditorGUILayout.EndScrollView();
        GUILayout.Space(10);
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
        int selectedIndex = Array.IndexOf(types, selectedType);
        selectedIndex = EditorGUILayout.Popup("Type", selectedIndex, types);
        selectedType = types[selectedIndex];
        newItemName = EditorGUILayout.TextField(newItemName);
        // New Item button
        if (GUILayout.Button("Create New Item") && !string.IsNullOrEmpty(newItemName)) { CreateNewItem(newItemName); newItemName = ""; }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Fix IDs")) { FixItemIDs(); }
        if (GUILayout.Button("Fix Actor Names")) { FixItemActorNames(); }
        //if (GUILayout.Button("Fix Associated Prefabs")) { FixItemAssociatedPrefabs(); }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Check for Issues")) {  }
        
        GUILayout.Space(10); 
        
        if (GUILayout.Button("Back")) { currentWindow = "Home"; }
    }


    //=-----------------=
    // General Functions
    //=-----------------=
    private void HeaderImage()
    {
        Texture2D propImage = Resources.Load<Texture2D>("Sprites/System/Frameworklogo1");
        if (propImage != null)
        {
            // Calculate the scaled size of the image
            float maxWidth = position.width - 10;
            float maxHeight = position.height - 100;
            float aspectRatio = propImage.width / (float)propImage.height;
            float scaledWidth = Mathf.Min(maxWidth, maxHeight * aspectRatio);
            float scaledHeight = scaledWidth / aspectRatio;
            
            // Display the scaled image
            GUILayout.Label(propImage, GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight));
        }
    }
    
    private GameObject GetPrefabAtPath(string _prefabName, string _folderPath)
    {
        string[] prefabGUIDs = AssetDatabase.FindAssets(_prefabName + " t:prefab", new[] { _folderPath });
        
        if (prefabGUIDs.Length > 0)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUIDs[0]);
            return AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        }
        Debug.LogError("Prefab " + _prefabName + " not found in folder: " + _folderPath);
        return null;
    }
    
    // Helper function to split camel case words
    private string[] SplitCamelCase(string input)
    {
        return Regex.Split(input, @"(?<!^)(?=[A-Z])");
    }

    private string GenerateIdFromActor(Actor _actor, string _typeTag)
    {
        string name = _actor.name;

        // Remove "Prop" or "prop" prefix if present
        if (name.StartsWith("Prop"))
            name = name.Substring(4);
        else if (name.StartsWith("prop"))
            name = name.Substring(4);

        // Split the name into words
        string[] words = SplitCamelCase(name);

        List<string> processedWords = new List<string>();

        // Iterate through each word
        for (int i = 0; i < words.Length; i++)
        {
            // Check if the word ends with a number followed by a capital letter
            if (i > 0 && char.IsDigit(words[i - 1][words[i - 1].Length - 1]) && char.IsUpper(words[i][0]))
            {
                // Combine the current word with the previous one without an underscore
                processedWords[processedWords.Count - 1] += words[i].ToLower();
            }
            else
            {
                // Separate words by capital letters and convert to lowercase
                string[] subWords = SplitCamelCase(words[i].ToLower());

                processedWords.AddRange(subWords);
            }
        }

        // Construct the ID
        string id = _typeTag + "_" + string.Join("_", processedWords);

        return id;
    }

    private string GenerateActorNameFromActor(Actor _actor)
    {
        string name = _actor.name;

        // Split the name into words at capital letters
        string[] words = SplitCamelCase(name);

        List<string> processedWords = new List<string>();

        // Iterate through each word
        for (int i = 0; i < words.Length; i++)
        {
            // Check if the word is preceded by a number and followed by a capital letter
            if (i > 0 && char.IsDigit(words[i - 1][words[i - 1].Length - 1]) && char.IsUpper(words[i][0]))
            {
                // Combine the current word with the previous one without a space
                processedWords[processedWords.Count - 1] += words[i];
            }
            else
            {
                processedWords.Add(words[i]);
            }
        }

        // Join the words with spaces
        string actorName = string.Join(" ", processedWords);

        return actorName;
    }
    
    //=-----------------=
    // Prop Manager Functions
    //=-----------------=
    private void GetPropList()
    {
        props.Clear();
        var guidList = AssetDatabase.FindAssets("", new[] { PropsFolder });
        foreach (var guid in guidList)
        {
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
            if (prop != null)
            {
                props.Add(prop.id);
                DisplayPropField(prop);
            }
        }
    }
    

    private void DisplayPropField(Prop _prop)
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
    
    private void FixPropIDs()
    {
        var guidList = AssetDatabase.FindAssets("", new[] { PropsFolder });
        foreach (var guid in guidList)
        {
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
            if (prop != null)
            {
                prop.id = GenerateIdFromActor(prop, "prop");
            }
        }
    }
    
    private void FixPropActorNames()
    {
        var guidList = AssetDatabase.FindAssets("", new[] { PropsFolder });
        foreach (var guid in guidList)
        {
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
            if (prop != null)
            {
                prop.actorName = GenerateActorNameFromActor(prop);
            }
        }
    }
    
    private void FixPropAssociatedPrefabs()
    {
        var guidList = AssetDatabase.FindAssets("", new[] { PropsFolder });
        foreach (var guid in guidList)
        {
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
            if (prop != null)
            {
                prop.AssociatedGameObject = GetPrefabAtPath(prop.name, ObjectsFolder);
            }
        }
    }
    
    private void DeleteProp(Prop _prop)
    {
        string assetPath = AssetDatabase.GetAssetPath(_prop);
        AssetDatabase.DeleteAsset(assetPath);
        props.Remove(_prop.id);
    }

    private void CreateNewProp(string propName)
    {
        // Create a new instance of the Prop scriptable object
        Prop newProp = CreateInstance<Prop>();
    
        // Set the name of the new prop
        newProp.name = propName;
    
        // Create asset file for the new prop
        string assetPath = $"{PropsFolder}/{propName}.asset";
        AssetDatabase.CreateAsset(newProp, assetPath);
    }
    
    //=-----------------=
    // Item Manager Functions
    //=-----------------=
    private void GetItemList()
    {
        items.Clear();
        var guidList = AssetDatabase.FindAssets("", new[] { ItemsFolder });
        foreach (var guid in guidList)
        {
            var item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
            if (item != null)
            {
                items.Add(item.id);
                DisplayItemField(item);
            }
        }
    }
    

    private void DisplayItemField(Item _item)
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
    
    private void FixItemIDs()
    {
        var guidList = AssetDatabase.FindAssets("", new[] { ItemsFolder });
        foreach (var guid in guidList)
        {
            var item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
            if (item != null)
            {
                item.id = GenerateIdFromActor(item, "item");
            }
        }
    }
    
    private void FixItemActorNames()
    {
        var guidList = AssetDatabase.FindAssets("", new[] { ItemsFolder });
        foreach (var guid in guidList)
        {
            var item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
            if (item != null)
            {
                item.actorName = GenerateActorNameFromActor(item);
            }
        }
    }
    
    private void FixItemAssociatedPrefabs()
    {
        var guidList = AssetDatabase.FindAssets("", new[] { ItemsFolder });
        foreach (var guid in guidList)
        {
            var item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
            if (item != null)
            {
                item.AssociatedGameObject = GetPrefabAtPath(item.name, ObjectsFolder);
            }
        }
    }
    
    private void DeleteItem(Item _item)
    {
        string assetPath = AssetDatabase.GetAssetPath(_item);
        AssetDatabase.DeleteAsset(assetPath);
        items.Remove(_item.id);
    }

    private void CreateNewItem(string _itemName)
    {
        Item newItem = null;
    
        // Set the name of the new item
        newItem.name = _itemName;
    
        // Create asset file for the new item
        string assetPath = $"{ItemsFolder}/{_itemName}.asset";
        AssetDatabase.CreateAsset(newItem, assetPath);
    }

}
