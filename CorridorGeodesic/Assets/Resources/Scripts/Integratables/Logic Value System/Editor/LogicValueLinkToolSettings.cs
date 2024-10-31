using System;
using UnityEditor;
using UnityEngine;

public class LogicValueLinkToolSettings : ScriptableObject
{
    public static LogicValueLinkToolSettings _instance;
    public static LogicValueLinkToolSettings Instance => (_instance != null ? _instance : _instance = LoadOrCreateSettings());
    public event Action OnSettingsValidate;

    public bool showToolButton = true;
    public bool showLinksWhenNotUsingTool = true;
    public bool selectTransformWhenNotUsingTool = true;

    [Space]
    public Texture2D toolbarIcon;

    [HideInInspector] public bool EditorToolUsesSceneGUI => showToolButton || showLinksWhenNotUsingTool;

    public void OnValidate()
    {
        OnSettingsValidate?.Invoke();
    }

    [MenuItem("Neverway/Tools/Logic Value Link Tool/Settings")]
    private static void SelectSettings()
    {
        Selection.activeObject = _instance;
    }
    private static LogicValueLinkToolSettings LoadOrCreateSettings()
    {
        string path;
        string[] settings = AssetDatabase.FindAssets($"t:{nameof(LogicValueLinkToolSettings)}");
        if (settings.Length > 0)
        {
            path = AssetDatabase.GUIDToAssetPath(settings[0]);
            return AssetDatabase.LoadAssetAtPath<LogicValueLinkToolSettings>(path);
        }

        LogicValueLinkToolSettings newSettings = ScriptableObject.CreateInstance<LogicValueLinkToolSettings>();
        try 
        {
            MonoScript script = MonoScript.FromScriptableObject(newSettings);
            path = AssetDatabase.GetAssetPath(script);

            path = path.Substring(0, path.LastIndexOf('/'));
            path += "/LogicValueLinkToolSettings.asset";

            AssetDatabase.CreateAsset(newSettings, path);
            AssetDatabase.SaveAssets();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Could not find nor create {nameof(LogicValueLinkToolSettings)} Asset, using defaults as fallback");
            Debug.LogException(e);
        }
        
        return newSettings;
    }
}