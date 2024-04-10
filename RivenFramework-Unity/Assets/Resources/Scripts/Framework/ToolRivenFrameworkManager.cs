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

public class ToolRivenFrameworkManager : EditorWindow
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private string currentWindow = "Home";
    private List<bool> tileGroupExpandedStates = new List<bool>();


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
            default:
                Missing();
                break;
        }
    }

    //=-----------------=
    // Internal Functions
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
        if (GetPrefabAtPath("GameInstance", "Assets/Resources/Actors/System") != null)
        {
            GameInstance gameInstance = GetPrefabAtPath("GameInstance", "Assets/Resources/Actors/System").GetComponent<GameInstance>();
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
            else { Debug.LogError("ProjectData component not found on the prefab."); }
            
        }
        GUILayout.Space(10); 
        
        if (GUILayout.Button("Back"))
        {
            currentWindow = "Home";
        }
    }

    private void PropManager()
    {
        GUILayout.Space(20); 
        if (GUILayout.Button("Back"))
        {
            currentWindow = "PropManager";
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
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
}
