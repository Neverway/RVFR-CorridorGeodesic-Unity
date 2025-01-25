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
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;

public class ToolAssetManager : MonoBehaviour
{
    [MenuItem("Neverway/Assets/Sort highlighted assets to selected 'Area'")]
    private static void OrganizeAllAssets()
    {
        RenameSelectedToPrefabName();
        
        // Get the containers
        Transform assetContainer = null;
        Transform actorContainer = null;
        Transform lightContainer = null;
        Transform structureContainer = null;
        var area = Selection.activeGameObject.transform;
        for (int i = 0; i < area.childCount; i++)
        {
            switch (area.GetChild(i).tag)
            {
                case "AssetContainer":
                    assetContainer = area.GetChild(i);
                    break;
                case "ActorContainer":
                    actorContainer = area.GetChild(i);
                    break;
                case "LightContainer":
                    lightContainer = area.GetChild(i);
                    break;
                case "StructureContainer":
                    structureContainer = area.GetChild(i);
                    break;
                default:
                    Debug.LogWarning($"Unknown container {area.GetChild(i).name} in {Selection.activeGameObject.name}, skipping");
                    break;
            }
        }

        // Exit if the player did not select an area/the containers for the asset types couldn't be located
        if (!assetContainer || !actorContainer || !lightContainer || !structureContainer)
        {
            Debug.LogWarning($"Could not find containers! Please shift click the assets you'd like to sort followed by ctrl clicking the 'Area' you'd like to sort them to");
            return;
        }
        
        // Get all unparented assets in the hierarchy (ignoring the first 3 since those should be the system stuff)
        foreach (var highlightedAsset in Selection.gameObjects)
        {
            if (highlightedAsset != Selection.activeGameObject && highlightedAsset != assetContainer.gameObject && highlightedAsset != actorContainer.gameObject && highlightedAsset != lightContainer.gameObject && highlightedAsset != structureContainer.gameObject)
            {
                // Asset is actor
                if (highlightedAsset.name.Contains("Actor_") || highlightedAsset.name.Contains("Phys_") || highlightedAsset.name.Contains("Volume") || highlightedAsset.name.Contains("FX_"))
                {
                    highlightedAsset.transform.SetParent(actorContainer.transform);
                    continue;
                }
                // Asset is light
                if (highlightedAsset.name.Contains("Light") || highlightedAsset.GetComponent(typeof(Light)))
                {
                    highlightedAsset.transform.SetParent(lightContainer.transform);
                    continue;
                }
                // Asset is structure
                if (highlightedAsset.name.Contains("Structure_") || highlightedAsset.GetComponent(typeof(ProBuilderMesh)))
                {
                    highlightedAsset.transform.SetParent(structureContainer.transform);
                    continue;
                }
                // Asset is probably generic
                else
                {
                    highlightedAsset.transform.SetParent(assetContainer.transform);
                    continue;
                }
            }
        }
        
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
                Debug.LogWarning($"{obj.name} is not a prefab instance, skipping");
            }
        }
    }
    
    [MenuItem("Neverway/Assets/Sort Selected Alphabetically")]
    private static void OrganizeSelectedAlphabetically()
    {
        // Get all selected GameObjects and sort them by name
        List<GameObject> selectedObjects = Selection.gameObjects.OrderBy(go => go.name).ToList();

        // Reorder GameObjects in the Hierarchy by setting sibling index
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            selectedObjects[i].transform.SetSiblingIndex(i);
        }

        Debug.Log("Selected GameObjects have been organized alphabetically");
    }
}
