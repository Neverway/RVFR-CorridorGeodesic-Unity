//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WB_LevelEditor_MemoryBrowser : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private LevelManager levelManager;
    [SerializeField] private GameObject inventoryBrowserRoot, inventoryTile, inventorySpacer;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
    
    }

    private void OnEnable()
    {
        InitializeInventory();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void InitializeInventory()
    {
        levelManager = FindObjectOfType<LevelManager>();
        
        // Clear inventory
        for (var i = 0; i < inventoryBrowserRoot.transform.childCount; i++)
        {
            Destroy(inventoryBrowserRoot.transform.GetChild(i).gameObject);
        }
        
        // Create tiles
        foreach (var tileMemory in levelManager.tileMemory)
        {
            for (int i = 0; i < tileMemory.tiles.Count; i++)
            {
                if (tileMemory.tiles[i].sprite == null)
                {
                    Debug.LogWarning($"{tileMemory.tiles[i].name} is missing a sprite! Skipping tile...");
                    continue;
                }
                var asset = Instantiate(inventoryTile, inventoryBrowserRoot.transform);
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileID = tileMemory.tiles[i].name;
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite = tileMemory.tiles[i].sprite;
                foreach (var spacer in tileMemory.spacers)
                {
                    if (spacer.index == i)
                    {
                        for (int j = 0; j < spacer.spacerCount; j++)
                        {
                            Instantiate(inventorySpacer, inventoryBrowserRoot.transform);
                        }
                    }
                }
            }
        }
        
        // Create assets
        foreach (var assetMemory in levelManager.assetMemory)
        {
            for (int i = 0; i < assetMemory.props.Count; i++)
            {
                var asset = Instantiate(inventoryTile, inventoryBrowserRoot.transform);
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileID = assetMemory.props[i].name;
                //asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite = assetMemory.props[i].GetComponent<SpriteRenderer>().sprite; // TODO
                foreach (var spacer in assetMemory.spacers)
                {
                    if (spacer.index == i)
                    {
                        for (int j = 0; j < spacer.spacerCount; j++)
                        {
                            Instantiate(inventorySpacer, inventoryBrowserRoot.transform);
                        }
                    }
                }
            }
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
