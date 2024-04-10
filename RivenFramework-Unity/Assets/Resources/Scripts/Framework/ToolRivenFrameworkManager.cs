//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

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
    private const string SystemObjectsFolder = "Assets/Resources/Actors/System";
    private const string PropsFolder = "Assets/Resources/Actors/Props";
    private const string ObjectsFolder = "Assets/Resources/Actors/Objects";


    //=-----------------=
    // Private Variables
    //=-----------------=
    private string currentWindow = "Home";
    
    private List<bool> tileGroupExpandedStates = new List<bool>();
    
    private List<string> propIDs = new List<string>();
    private Dictionary<string, Color> scriptableObjectColors = new Dictionary<string, Color>();


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
        EditorGUILayout.HelpBox("", MessageType.None);
        GUILayout.Space(10); 
        
        CollectPropIDs();
        DisplayScriptableObjects();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Fix Missing IDs")) { AutoGenerateMissingIds(); }
        if (GUILayout.Button("Fix Missing Prefabs")) { AssociatePrefabsWithScriptables(); }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh")) { RefreshScriptableObjects(); }
        if (GUILayout.Button("Check Assignments")) { ApplyScriptableObjectColors(); }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(10); 
        
        if (GUILayout.Button("Back"))
        {
            currentWindow = "Home";
        }
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
    
    //=-----------------=
    // Prop Manager Functions
    //=-----------------=
    private void CollectPropIDs()
    {
        propIDs.Clear();
        var guids = AssetDatabase.FindAssets("", new[] { PropsFolder });
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(assetPath);
            if (prop != null)
            {
                propIDs.Add(prop.id);
            }
        }
    }

    private void DisplayScriptableObjects()
    {
        var guids = AssetDatabase.FindAssets("", new[] { PropsFolder });
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Props", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("ID", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Associated Prefab", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(assetPath);
            if (prop != null)
            {
                DisplayScriptableObjectField(assetPath, prop);
            }
        }
    }
    
    private void DisplayScriptableObjectField(string assetPath, Prop prop)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.ObjectField(prop, typeof(Prop), false);

        // ID Field
        bool isIDUnique = propIDs.FindAll(id => id == prop.id).Count == 1;
        prop.id = EditorGUILayout.TextField(prop.id, GUILayout.Width(200));

        // Prefab Field
        bool prefabExists = File.Exists($"{ObjectsFolder}/{prop.name}.prefab");
        prop.AssociatedGameObject = (GameObject)EditorGUILayout.ObjectField(prop.AssociatedGameObject, typeof(GameObject), false);

        EditorGUILayout.EndHorizontal();

        // Set the scriptable object color based on conditions
        if (scriptableObjectColors.ContainsKey(assetPath))
        {
            EditorGUI.DrawRect(GUILayoutUtility.GetLastRect(), scriptableObjectColors[assetPath] * new Color(1, 1, 1, 0.2f)); // Slightly color the background based on the status
        }
    }

    private void AssociatePrefabsWithScriptables()
    {
        foreach (string propID in propIDs)
        {
            var propSOPaths = AssetDatabase.FindAssets(propID, new[] { PropsFolder })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Where(path => AssetDatabase.LoadAssetAtPath<Prop>(path)?.id == propID)
                .ToList();

            foreach (var propSOPath in propSOPaths)
            {
                Prop propSO = AssetDatabase.LoadAssetAtPath<Prop>(propSOPath);
                string prefabPath = $"{ObjectsFolder}/{propSO.name}.prefab";

                if (File.Exists(prefabPath))
                {
                    GameObject prefabGO = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                    propSO.AssociatedGameObject = prefabGO;
                    EditorUtility.SetDirty(propSO); // Mark the Prop SO as dirty to ensure the change is saved
                }
            }
        }

        AssetDatabase.SaveAssets();
    }

    private void AutoGenerateMissingIds()
    {
        foreach (string propID in propIDs)
        {
            var propSOPaths = AssetDatabase.FindAssets(propID, new[] { PropsFolder })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .ToList();

            foreach (var propSOPath in propSOPaths)
            {
                Prop propSO = AssetDatabase.LoadAssetAtPath<Prop>(propSOPath);

                // Generate ID based on scriptable object name
                string[] words = SplitCamelCase(propSO.name);
                List<string> processedWords = new List<string>();

                for (int i = 0; i < words.Length; i++)
                {
                    // Handle special cases for words
                    if (i < words.Length - 1 && char.IsDigit(words[i][words[i].Length - 1]) && char.IsUpper(words[i + 1][0]))
                    {
                        processedWords.Add(words[i] + words[i + 1]);
                        i++; // Skip the next word
                    }
                    else
                    {
                        processedWords.Add(words[i].ToLower());
                    }
                }

                // Construct the ID
                string id = string.Join("_", processedWords);
                if (!id.StartsWith("prop_") && !id.StartsWith("Prop_")) id = "prop_" + id;

                propSO.id = id.ToLower();

                EditorUtility.SetDirty(propSO); // Mark the Prop SO as dirty to ensure the change is saved
            }
        }

        AssetDatabase.SaveAssets();
    }



// Helper function to split camel case words without splitting certain cases
    private string[] SplitCamelCase(string input)
    {
        return Regex.Split(input, @"(?<!^)(?=[A-Z])");
    }



    
    private void RefreshScriptableObjects()
    {
        // Clear scriptable object colors
        scriptableObjectColors.Clear();
        Repaint(); // Repaint the window to reflect the changes
    }

    private void ApplyScriptableObjectColors()
    {
        foreach (var assetPath in AssetDatabase.FindAssets("", new[] { PropsFolder }).Select(guid => AssetDatabase.GUIDToAssetPath(guid)))
        {
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(assetPath);
            if (prop != null)
            {
                bool isIDUnique = !string.IsNullOrEmpty(prop.id) && propIDs.Count(id => id == prop.id) == 1;
                bool prefabExists = File.Exists($"{ObjectsFolder}/{prop.name}.prefab");

                Color statusColor = GetScriptableObjectColor(isIDUnique, prefabExists);
                scriptableObjectColors[assetPath] = statusColor;
            }
        }
    }


    private Color GetScriptableObjectColor(bool isIDUnique, bool prefabExists)
    {
        if (isIDUnique && prefabExists)
        {
            return Color.green;
        }
        else if (!isIDUnique && !prefabExists)
        {
            return Color.red;
        }
        else
        {
            return new Color(1f,0.5f,0f);
        }
    }
}
