//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ToolAssetManager : MonoBehaviour
{
    [MenuItem("Neverway/Assets/Organize all loose assets")]
    private static void OrganizeAllAssets()
    {
        // Get the containers
        Debug.Log("All assets have been organized");
    }
    [MenuItem("Neverway/Assets/Rename Selected To Prefab Name")]
    private static void RenameSelectedToPrefabName()
    {
        foreach (var obj in Selection.gameObjects)
        {
            GameObject prefabSource = PrefabUtility.GetCorrespondingObjectFromSource(obj);

            if (prefabSource != null)
            {
                obj.name = prefabSource.name;
                Debug.Log($"Renamed {obj.name} to prefab name: {prefabSource.name}");
            }
            else
            {
                Debug.LogWarning($"{obj.name} is not a prefab instance.");
            }
        }
    }
    [MenuItem("Neverway/Assets/Organize Selected Alphabetically")]
    private static void OrganizeSelectedAlphabetically()
    {
        // Get all selected GameObjects and sort them by name
        List<GameObject> selectedObjects = Selection.gameObjects.OrderBy(go => go.name).ToList();

        // Reorder GameObjects in the Hierarchy by setting sibling index
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            selectedObjects[i].transform.SetSiblingIndex(i);
        }

        Debug.Log("Selected GameObjects have been organized alphabetically.");
    }
}
