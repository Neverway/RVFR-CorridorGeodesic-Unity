//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Unity editor tool to manage project settings, as well as props, items,
// & characters that appear in the project
// Notes: This script only handles the main windows. The other windows are stored
// in their own scripts
//
//=============================================================================

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class ToolFrameworkManager : EditorWindow
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string currentWindow = "Home";
    public string newActorName = "";


    //=-----------------=
    // Private Variables
    //=-----------------=
    public Vector2 scrollPosition = Vector2.zero;
    private const string SystemObjectsFolder = "Assets/Resources/Actors/System";
    public const string ObjectsFolder = "Assets/Resources/Actors/Objects";
    public string selectedItemType = "Utility Throwable";


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private ToolFrameworkManagerProps propManager;
    private ToolFrameworkManagerItems itemManager;
    private ToolFrameworkManagerCharacters characterManager;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    [MenuItem("Neverway/Framework Manager")]
    public static void ShowWindow()
    {
        GetWindow<ToolFrameworkManager>("Riven Framework Manager");
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
                if (propManager) propManager.Window(this);
                break;
            case "ItemManager":
                if (itemManager) itemManager.Window(this);
                break;
            case "CharacterManager":
                if (characterManager) characterManager.Window(this);
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
        if (GUILayout.Button("Prop Manager")) 
        {
            if (propManager is null)
            {
                propManager = CreateInstance<ToolFrameworkManagerProps>();
                propManager.initialized = false;
            }
            currentWindow = "PropManager"; 
        }

        if (GUILayout.Button("Item Manager"))
        {
            if (itemManager is null)
            {
                itemManager = CreateInstance<ToolFrameworkManagerItems>();
                itemManager.initialized = false;
            }
            currentWindow = "ItemManager";
        }

        if (GUILayout.Button("Character Manager"))
        {
            if (characterManager is null)
            {
                characterManager = CreateInstance<ToolFrameworkManagerCharacters>();
                characterManager.initialized = false;
            }
            currentWindow = "CharacterManager";
        }
    }

    private void ProjectSetup()
    {
        EditorGUILayout.HelpBox("", MessageType.None);
        GUILayout.Space(10); 

        // Game instance settings
        if (GetPrefabAtPath("GameInstance", SystemObjectsFolder) is not null)
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
                EditorGUILayout.LabelField("Inventory Widget:", GUILayout.Width(120));
                EditorGUILayout.ObjectField(gameInstance.UserInterfaceWidgets[5], typeof(GameObject), false);
                EditorGUILayout.EndHorizontal();
            }
            else { Debug.LogError("GameInstance component not found on the prefab."); }
            
            ApplicationSettings applicationSettings = GetPrefabAtPath("GameInstance", SystemObjectsFolder).GetComponent<ApplicationSettings>();
            if (applicationSettings != null)
            {
                GUILayout.Space(10); 
                EditorGUILayout.LabelField("Default Application Settings", EditorStyles.boldLabel);
                //EditorGUILayout.Vector2Field("Target Resolution", applicationSettings.defaultSettingsData.targetResolution);
                EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField("Window Mode", GUILayout.Width(120));
                //EditorGUILayout.IntSlider(applicationSettings.defaultSettingsData.windowMode, 0,3);
                EditorGUILayout.EndHorizontal();
            }
        }
        GUILayout.Space(10); 
        
        if (GUILayout.Button("Back"))
        {
            currentWindow = "Home";
        }
    }

    private void Missing()
    {
        EditorGUILayout.HelpBox("Whoops, it looks like you reached a dead end! This section may not be complete or an issue may have occured. Please report this to a developer.", MessageType.Error);
        GUILayout.Space(10); 
        if (GUILayout.Button("Back")) { currentWindow = "Home"; }
    }


    //=-----------------=
    // General Functions
    //=-----------------=
    private void HeaderImage()
    {
        Texture2D propImage = Resources.Load<Texture2D>("Sprites/System/Frameworklogo1");
        if (propImage is null) return;
        // Calculate the scaled size of the image
        float maxWidth = position.width - 10;
        float maxHeight = position.height - 100;
        float aspectRatio = propImage.width / (float)propImage.height;
        float scaledWidth = Mathf.Min(maxWidth, maxHeight * aspectRatio);
        float scaledHeight = scaledWidth / aspectRatio;
            
        // Display the scaled image
        GUILayout.Label(propImage, GUILayout.Width(scaledWidth), GUILayout.Height(scaledHeight), GUILayout.MaxHeight(200));
    }
    
    public GameObject GetPrefabAtPath(string _prefabName, string _folderPath)
    {
        string[] prefabGUIDs = AssetDatabase.FindAssets(_prefabName + " t:prefab", new[] { _folderPath });
        
        if (prefabGUIDs.Length > 0)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUIDs[0]);
            return AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        }
        Debug.LogWarning("Prefab " + _prefabName + " not found in folder: " + _folderPath);
        return null;
    }
    
    // Helper function to split camel case words
    private string[] SplitCamelCase(string _input)
    {
        return Regex.Split(_input, @"(?<!^)(?=[A-Z])");
    }

    public string GenerateIdFromActor(Actor _actor, string _typeTag)
    {
        string actorName = _actor.name;

        // Remove "Prop" or "prop" prefix if present
        if (actorName.StartsWith("Prop"))
            actorName = actorName.Substring(4);
        else if (actorName.StartsWith("prop"))
            actorName = actorName.Substring(4);

        // Split the name into words
        string[] words = SplitCamelCase(actorName);

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

    public string GenerateActorNameFromActor(Actor _actor)
    {
        string actorName = _actor.name;

        // Split the name into words at capital letters
        string[] words = SplitCamelCase(actorName);

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
        string outputName = string.Join(" ", processedWords);

        return outputName;
    }
}
