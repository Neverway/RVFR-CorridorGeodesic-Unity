using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "QuickLevelDemoSetup")]
public class QuickLevelDemoSetup : ScriptableObject
{
#if UNITY_EDITOR
    public List<SceneAsset> sceneList;

    public void OnValidate()
    {
        List<string> newSceneNames = new List<string>();
        foreach(SceneAsset scene in sceneList)
        {
            newSceneNames.Add(scene.name);
        }
        sceneNames = newSceneNames.ToArray();
    }
#endif
    [DebugReadOnly] public string[] sceneNames;  

    public QuickLevelDemoSetup nextLevelDemoSetup;
}
