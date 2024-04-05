//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private ProjectData projectData;
    [SerializeField] private GameObject inventoryBrowserRoot, inventoryTile, inventorySpacer, inventoryHeader;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        projectData = FindObjectOfType<ProjectData>();
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
        projectData = FindObjectOfType<ProjectData>();
        
        // Clear inventory
        for (var i = 0; i < inventoryBrowserRoot.transform.childCount; i++)
        {
            Destroy(inventoryBrowserRoot.transform.GetChild(i).gameObject);
        }
        
        // Create tiles
        CreateHeader("Tiles");
        foreach (var tileMemory in projectData.tiles)
        {
            for (int i = 0; i < tileMemory.tiles.Count; i++)
            {
                if (tileMemory.tiles[i].sprite == null)
                {
                    Debug.LogWarning($"{tileMemory.tiles[i].name} is missing a sprite! Skipping tile...");
                    continue;
                }
                var asset = Instantiate(inventoryTile, inventoryBrowserRoot.transform);
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileId = tileMemory.tiles[i].name;
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
        
        // Create actors
        // Create props
        CreateHeader("Props");
        foreach (var propMemory in projectData.props)
        {
            for (int i = 0; i < propMemory.props.Count; i++)
            {
                var asset = Instantiate(inventoryTile, inventoryBrowserRoot.transform);
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileId = propMemory.props[i].id;
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileName = propMemory.props[i].actorName;
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite = propMemory.props[i].icon;
                if (!propMemory.props[i].icon) asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite = projectData.missingSpriteFallback;
                foreach (var spacer in propMemory.spacers)
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
        
        // Create items
        CreateHeader("Items");
        foreach (var itemMemory in projectData.items)
        {
            for (int i = 0; i < itemMemory.items.Count; i++)
            {
                var asset = Instantiate(inventoryTile, inventoryBrowserRoot.transform);
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileId = itemMemory.items[i].id;
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileName = itemMemory.items[i].actorName;
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite = itemMemory.items[i].icon;
                if (!itemMemory.items[i].icon) asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite = projectData.missingItemIconFallback;
                foreach (var spacer in itemMemory.spacers)
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
        
        // Create characters
        CreateHeader("Characters");
        foreach (var characterMemory in projectData.characters)
        {
            for (int i = 0; i < characterMemory.characters.Count; i++)
            {
                var asset = Instantiate(inventoryTile, inventoryBrowserRoot.transform);
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileId = characterMemory.characters[i].id;
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileName = characterMemory.characters[i].actorName;
                asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite = characterMemory.characters[i].icon;
                if (!characterMemory.characters[i].icon) asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite = projectData.missingCharacterIconFallback;
                foreach (var spacer in characterMemory.spacers)
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

    private void CreateHeader(string _title)
    {
        var asset = Instantiate(inventoryHeader, inventoryBrowserRoot.transform);
        asset.transform.GetChild(0).GetComponent<TMP_Text>().text = _title;
        for (int j = 0; j < 9; j++)
        {
            Instantiate(inventorySpacer, inventoryBrowserRoot.transform);
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
