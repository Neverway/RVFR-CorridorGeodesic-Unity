using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A tool for creating and associating props in Unity's Editor.
/// </summary>
public class EditorPropAssociator : EditorWindow
{
    private const string ScriptableObjectFolderPath = "Assets/Resources/Actors/Props";
    private const string PrefabFolderPath = "Assets/Resources/Actors/Objects";
    private List<string> propIDs = new List<string>();
    private Dictionary<string, Color> scriptableObjectColors = new Dictionary<string, Color>();

    [MenuItem("Tools/Prop Associator")]
    public static void ShowWindow()
    {
        GetWindow<EditorPropAssociator>("Prop Associator");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Scriptable Objects in Folder", EditorStyles.boldLabel);
        CollectPropIDs();
        DisplayScriptableObjects();

        GUILayout.Space(20); // Space for aesthetics
        if (GUILayout.Button("Associate Prefabs"))
        {
            AssociatePrefabsWithScriptables();
            ApplyScriptableObjectColors();
        }

        GUILayout.Space(20); // Additional space for the refresh button
        if (GUILayout.Button("Refresh"))
        {
            RefreshScriptableObjects();
        }
    }
    
    private void CollectPropIDs()
    {
        propIDs.Clear();
        var guids = AssetDatabase.FindAssets("", new[] { ScriptableObjectFolderPath });
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
        var guids = AssetDatabase.FindAssets("", new[] { ScriptableObjectFolderPath });
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
        bool prefabExists = File.Exists($"{PrefabFolderPath}/{prop.name}.prefab");
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
            var propSOPaths = AssetDatabase.FindAssets(propID, new[] { ScriptableObjectFolderPath })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Where(path => AssetDatabase.LoadAssetAtPath<Prop>(path)?.id == propID)
                .ToList();

            foreach (var propSOPath in propSOPaths)
            {
                Prop propSO = AssetDatabase.LoadAssetAtPath<Prop>(propSOPath);
                string prefabPath = $"{PrefabFolderPath}/{propSO.name}.prefab";

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
    
    private void RefreshScriptableObjects()
    {
        // Clear scriptable object colors
        scriptableObjectColors.Clear();
        Repaint(); // Repaint the window to reflect the changes
    }

    private void ApplyScriptableObjectColors()
    {
        foreach (var assetPath in AssetDatabase.FindAssets("", new[] { ScriptableObjectFolderPath }).Select(guid => AssetDatabase.GUIDToAssetPath(guid)))
        {
            var prop = AssetDatabase.LoadAssetAtPath<Prop>(assetPath);
            if (prop != null)
            {
                bool isIDUnique = propIDs.FindAll(id => id == prop.id).Count == 1;
                bool prefabExists = File.Exists($"{PrefabFolderPath}/{prop.name}.prefab");

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
        else if (!isIDUnique || !prefabExists)
        {
            return Color.red;
        }
        else
        {
            return Color.yellow;
        }
    }
}