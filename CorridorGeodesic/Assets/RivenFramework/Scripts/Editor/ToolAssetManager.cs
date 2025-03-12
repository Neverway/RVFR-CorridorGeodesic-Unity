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

public class ToolAssetManager : MonoBehaviour
{
    [MenuItem("Neverway/Assets/Sort highlighted assets to selected 'Area'")]
    private static void OrganizeAllAssets()
    {
        RenameSelectedToPrefabName();
        
        // Get the containers
        Transform actorContainer = null;
        Transform logicContainer = null;
        Transform propContainer = null;
        Transform fxContainer = null;
        Transform structureContainer = null;
        var area = Selection.activeGameObject.transform;
        for (int i = 0; i < area.childCount; i++)
        {
            switch (area.GetChild(i).tag)
            {
                case "[ActorContainer]":
                    actorContainer = area.GetChild(i);
                    break;
                case "[LogicContainer]":
                    logicContainer = area.GetChild(i);
                    break;
                case "[PropContainer]":
                    propContainer = area.GetChild(i);
                    break;
                case "[FXContainer]":
                    fxContainer = area.GetChild(i);
                    break;
                case "[StructureContainer]":
                    structureContainer = area.GetChild(i);
                    break;
                default:
                    Debug.LogWarning($"Unknown container {area.GetChild(i).name} in {Selection.activeGameObject.name}, skipping");
                    break;
            }
        }

        // Exit if the player did not select an area/the containers for the asset types couldn't be located
        if (!actorContainer || !logicContainer || !propContainer || !fxContainer || !structureContainer)
        {
            Debug.LogWarning($"Could not find containers! Please shift+click the assets you'd like to sort followed by ctrl clicking the 'Area' you'd like to sort them to");
            return;
        }
        
        // Get all unparented assets in the hierarchy (ignoring the first 3 since those should be the system stuff)
        foreach (var highlightedAsset in Selection.gameObjects)
        {
            if (highlightedAsset != Selection.activeGameObject && highlightedAsset != actorContainer.gameObject && highlightedAsset != logicContainer.gameObject && highlightedAsset != propContainer.gameObject && highlightedAsset != fxContainer.gameObject && highlightedAsset != structureContainer.gameObject)
            {
                // Asset is actor
                if (highlightedAsset.name.Contains("Actor_") || highlightedAsset.name.Contains("Pawn_"))
                {
                    highlightedAsset.transform.SetParent(actorContainer.transform);
                    continue;
                }
                // Asset is logic
                if (highlightedAsset.name.Contains("Logic_") || highlightedAsset.name.Contains("Volume3D") || highlightedAsset.name.Contains("Volume2D"))
                {
                    highlightedAsset.transform.SetParent(logicContainer.transform);
                    continue;
                }
                // Asset is prop
                if (highlightedAsset.name.Contains("Prop_") || highlightedAsset.name.Contains("Phys_"))
                {
                    highlightedAsset.transform.SetParent(propContainer.transform);
                    continue;
                }
                // Asset is fx
                if (highlightedAsset.name.Contains("Fx_") || highlightedAsset.GetComponent(typeof(ReflectionProbe)))
                {
                    highlightedAsset.transform.SetParent(fxContainer.transform);
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
                    highlightedAsset.transform.SetParent(actorContainer.transform);
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
