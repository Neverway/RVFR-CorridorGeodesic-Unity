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
using UnityEngine.UI;

public class WB_LevelEditor_MemoryBrowser : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private string currentCategory = "All Assets";
    private string currentGroup = "All";


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private LevelManager levelManager;
    private ProjectData projectData;
    [SerializeField] private GameObject categoryBrowserRoot, assetBrowserRoot, categoryHeader, groupButton, assetTile, assetSpacer, assetHeader;


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
        InitializeCategoryBrowser();
        SetCurrentCategoryAndGroup(currentCategory, currentGroup);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void InitializeCategoryBrowser()
    {
        levelManager = FindObjectOfType<LevelManager>();
        projectData = FindObjectOfType<ProjectData>();
        
        // Clear menu
        for (var i = 0; i < categoryBrowserRoot.transform.childCount; i++)
        {
            Destroy(categoryBrowserRoot.transform.GetChild(i).gameObject);
        }
        
        CreateCategoryHeader("All Assets");
        CreateGroupButton("All Assets", "All");
        
        CreateCategoryHeader("Tiles");
        CreateGroupButton("Tiles", "All");
        foreach (var tileMemory in projectData.tiles)
        {
            CreateGroupButton("Tiles", tileMemory.name);
        }
        
        CreateCategoryHeader("Props");
        CreateGroupButton("Props", "All");
        foreach (var propMemory in projectData.props)
        {
            CreateGroupButton("Props", propMemory.name);
        }
        
        CreateCategoryHeader("Items");
        CreateGroupButton("Items", "All");
        foreach (var itemMemory in projectData.items)
        {
            CreateGroupButton("Items", itemMemory.name);
        }
        
        CreateCategoryHeader("Characters");
        CreateGroupButton("Characters", "All");
        foreach (var characterMemory in projectData.characters)
        {
            CreateGroupButton("Characters", characterMemory.name);
        }
    }
    
    private void InitializeAssetBrowser()
    {
        levelManager = FindObjectOfType<LevelManager>();
        projectData = FindObjectOfType<ProjectData>();
        
        // Clear asset
        for (var i = 0; i < assetBrowserRoot.transform.childCount; i++)
        {
            Destroy(assetBrowserRoot.transform.GetChild(i).gameObject);
        }
        
        // Create tiles
        if (currentCategory == "Tiles" || currentCategory == "All Assets")
        {
            CreateHeader("Tiles");
            foreach (var tileMemory in projectData.tiles)
            {
                if (tileMemory.name != currentGroup && currentGroup != "All") continue;
                for (int i = 0; i < tileMemory.tiles.Count; i++)
                {
                    if (tileMemory.tiles[i].sprite == null)
                    {
                        Debug.LogWarning($"{tileMemory.tiles[i].name} is missing a sprite! Skipping tile...");
                        continue;
                    }
                    var asset = Instantiate(assetTile, assetBrowserRoot.transform);
                    asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileId = tileMemory.tiles[i].name;
                    asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite = tileMemory.tiles[i].sprite;
                    foreach (var spacer in tileMemory.spacers)
                    {
                        if (spacer.index == i)
                        {
                            for (int j = 0; j < spacer.spacerCount; j++)
                            {
                                Instantiate(assetSpacer, assetBrowserRoot.transform);
                            }
                        }
                    }
                }
            }
        }
        
        // Create actors
        // Create props
        if (currentCategory == "Props" || currentCategory == "All Assets")
        {
            CreateHeader("Props");
            foreach (var propMemory in projectData.props)
            {
                if (propMemory.name != currentGroup && currentGroup != "All") continue;
                for (int i = 0; i < propMemory.props.Count; i++)
                {
                    var asset = Instantiate(assetTile, assetBrowserRoot.transform);
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
                                Instantiate(assetSpacer, assetBrowserRoot.transform);
                            }
                        }
                    }
                }
            }
        }
        
        // Create items
        if (currentCategory == "Items" || currentCategory == "All Assets")
        {
            CreateHeader("Items");
            foreach (var itemMemory in projectData.items)
            {
                if (itemMemory.name != currentGroup && currentGroup != "All") continue;
                for (int i = 0; i < itemMemory.items.Count; i++)
                {
                    var asset = Instantiate(assetTile, assetBrowserRoot.transform);
                    asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileId = itemMemory.items[i].id;
                    asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileName = itemMemory.items[i].actorName;
                    asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite = itemMemory.items[i].icon;
                    if (!itemMemory.items[i].icon)
                        asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite =
                            projectData.missingItemIconFallback;
                    foreach (var spacer in itemMemory.spacers)
                    {
                        if (spacer.index == i)
                        {
                            for (int j = 0; j < spacer.spacerCount; j++)
                            {
                                Instantiate(assetSpacer, assetBrowserRoot.transform);
                            }
                        }
                    }
                }
            }
        }

        // Create characters
        if (currentCategory == "Characters" || currentCategory == "All Assets")
        {
            CreateHeader("Characters");
            foreach (var characterMemory in projectData.characters)
            {
                if (characterMemory.name != currentGroup && currentGroup != "All") continue;
                for (int i = 0; i < characterMemory.characters.Count; i++)
                {
                    var asset = Instantiate(assetTile, assetBrowserRoot.transform);
                    asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileId = characterMemory.characters[i].id;
                    asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileName =
                        characterMemory.characters[i].actorName;
                    asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite =
                        characterMemory.characters[i].icon;
                    if (!characterMemory.characters[i].icon)
                        asset.GetComponent<WB_LevelEditor_MemoryBrowser_Item>().tileSprite =
                            projectData.missingCharacterIconFallback;
                    foreach (var spacer in characterMemory.spacers)
                    {
                        if (spacer.index == i)
                        {
                            for (int j = 0; j < spacer.spacerCount; j++)
                            {
                                Instantiate(assetSpacer, assetBrowserRoot.transform);
                            }
                        }
                    }
                }
            }
        }
    }

    
    private void CreateCategoryHeader(string _title)
    {
        var asset = Instantiate(categoryHeader, categoryBrowserRoot.transform);
        asset.transform.GetChild(0).GetComponent<TMP_Text>().text = _title;
    }
    
    private void CreateGroupButton(string _category, string _title)
    {
        var asset = Instantiate(groupButton, categoryBrowserRoot.transform);
        asset.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = _title;
        asset.GetComponent<WB_LevelEditor_MemoryBrowser_GroupButton>().category = _category;
        asset.GetComponent<WB_LevelEditor_MemoryBrowser_GroupButton>().group = _title;
    }

    private void CreateHeader(string _title)
    {
        var asset = Instantiate(assetHeader, assetBrowserRoot.transform);
        asset.transform.GetChild(0).GetComponent<TMP_Text>().text = _title;
        for (int j = 0; j < 9; j++)
        {
            Instantiate(assetSpacer, assetBrowserRoot.transform);
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    // Called by the group buttons for filtering asset results
    public void SetCurrentCategoryAndGroup(string _catgeory, string _group)
    {
        currentCategory = _catgeory;
        currentGroup = _group;
        InitializeAssetBrowser();
        // Reset background color used for indicating current group
        for (var i = 0; i < categoryBrowserRoot.transform.childCount; i++)
        {
            categoryBrowserRoot.transform.GetChild(i).GetComponent<Image>().color = new Color(0,0,0,0.1529412f);
        }
    }
}
