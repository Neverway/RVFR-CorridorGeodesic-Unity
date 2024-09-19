using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(menuName = "SceneList")]
public class SceneList : ScriptableObject
{
#if UNITY_EDITOR
    public List<SceneAsset> EDITOR_sceneList;
    [DebugReadOnly, SerializeField] private List<SceneAsset> SCENES_NOT_IN_BUILD;

    public void OnValidate()
    {
        try
        {
            List<string> newSceneNames = new List<string>();
            SCENES_NOT_IN_BUILD = new List<SceneAsset>();
            foreach (SceneAsset scene in EDITOR_sceneList)
            {
                if (IsSceneInBuildSettings(scene))
                    newSceneNames.Add(scene.name);
                else
                    SCENES_NOT_IN_BUILD.Add(scene);
            }
            sceneNames = newSceneNames.ToArray();
        }
        catch { }
    }
    public bool IsSceneInBuildSettings(SceneAsset sceneAsset)
    {
        if (sceneAsset == null) return false;

        string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.path == scenePath)
                return true;
        }
        return false;
    }
#endif
    [DebugReadOnly, SerializeField] public string[] sceneNames;
}
