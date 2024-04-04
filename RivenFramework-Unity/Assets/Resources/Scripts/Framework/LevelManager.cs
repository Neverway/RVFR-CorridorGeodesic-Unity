//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Handles the saving and loading of map data
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using SimpleFileBrowser;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("A list of all the tiles and their categories used for the current project")]
    public List<tileMemoryGroup> tileMemory;
    [Tooltip("A list of all the objects and their categories used for the current project")]
    public List<AssetMemoryGroup> assetMemory;
    [Tooltip("A list of all the sprites that can be used for decor props for the current project")]
    public List<Sprite> spriteMemory;
    public List<Item> itemMemory;
    public Tile missingTileFallback;
    public GameObject missingObjectFallback;
    public Sprite missingSpriteFallback;
    [Header("READ-ONLY (Don't touch!)")]
    [Tooltip("A list of all of the 'tile layers' used in the scene")]
    public List<Tilemap> tilemaps;
    public GameObject assetsRoot;
    public string filePath;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private UISkin fileBrowserSkin;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        InitializeSceneReferences();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        InitializeSceneReferences();
    }

    private void InitializeSceneReferences()
    {
        tilemaps.Clear();
        if (tilemaps.Count == 0 && GameObject.FindWithTag("TilemapContainer"))
        {
            var tileMapGroups = GameObject.FindWithTag("TilemapContainer").transform;
            for (int i = 0; i < tileMapGroups.childCount; i++)
            {
                for (int ii = 0; ii < tileMapGroups.GetChild(i).childCount; ii++)
                {
                    tilemaps.Add(tileMapGroups.GetChild(i).GetChild(ii).GetComponent<Tilemap>());
                }
            }
        }
        if (!assetsRoot) assetsRoot = GameObject.FindGameObjectWithTag("AssetContainer");
    }
    
    private IEnumerator ShowFileDialogCoroutine(string _mode)
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("CT Maps", ".ctmap"));
        FileBrowser.AddQuickLink("Data Path", Application.persistentDataPath);
        FileBrowser.AddQuickLink("Application Path", Application.dataPath + "/Maps/");
        FileBrowser.AddQuickLink("Editor Level Path", Application.dataPath + "/Resources/Maps/");
        
        // Set the path to open the dialog to
        var startingPath = Application.persistentDataPath;
        // Load maps while in the Unity editor
        if (Directory.Exists($"{Application.dataPath}/Resources/Maps"))
        {
            startingPath = Application.dataPath + "/Resources/Maps/";
        }
        // Load maps while in the built application
        if (Directory.Exists($"{Application.dataPath}/Maps"))
        {
            startingPath = Application.dataPath + "/Maps/";
        }

        yield return _mode switch
        {
            "Load" => FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, false, startingPath, null, "Load Cartographer Map File", "Load"),
            "Save" => FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, false, startingPath, null, "Save Cartographer Map File", "Save"),
            _ => null
        };

        if (!FileBrowser.Success) yield break;
        filePath = FileBrowser.Result[0];
        switch (_mode)
        {
            case "Load":
                LoadLevel(filePath);
                break;
            case "Save":
                SaveLevel(filePath);
                break;
        }
    }

    private void SaveTiles(LevelData _data)
    {
        // Save tile data
        foreach (var tilemap in tilemaps)
        {
            // Get the size of the tilemap
            var bounds = tilemap.cellBounds;
        
            // Iterate through each tile in the tilemap
            for (var x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (var y = bounds.min.y; y < bounds.max.y; y++)
                {
                    // Check each tileMemory group to see if the tile exists
                    TileBase tempTile = null;
                    foreach (var group in tileMemory)
                    {
                        // If the tile exists in tileMemory, save its data
                        if (group.tiles.Find(t => t == tilemap.GetTile(new Vector3Int(x, y, 0))))
                        {
                            tempTile = group.tiles.Find(t => t == tilemap.GetTile(new Vector3Int(x, y, 0)));
                            break;
                        }
                    }

                    // If the tile is not found, skip it
                    if (tempTile == null) continue;
                
                    // Add the tile data to the level data
                    var newSpotData = new SpotData
                    {
                        id = tempTile.name, // Tile name
                        position = new Vector3Int(x,y,0), // Tile position
                        layer = tilemaps.IndexOf(tilemap), // Tilemap layer
                        layerID = tilemap.name // Tilemap name
                    };
                    _data.tiles.Add(newSpotData); // Add tile data to the level data
                }
            }
        }
    }

    private void SaveAssets(LevelData _data)
    {
        // Save object data
        // Find all objects with an asset_instanceID component
        // For each one, see if we can find the root asset in assetMemory
        // If not, skip it
        // else, add the asset data for name, unique id, position, and any variable data
        for (int i = 0; i < assetsRoot.transform.childCount; i++)
        {
            GameObject tempAsset = null;
            foreach (var group in assetMemory)
            {
                if (group.assets.Find(t => t.name == assetsRoot.transform.GetChild(i).gameObject.name))
                {
                    // If the asset is found in assetMemory, set it as tempAsset
                    tempAsset = assetsRoot.transform.GetChild(i).gameObject;
                    break;
                }
            }
            
            // If the asset is not found in assetMemory, skip it
            if (tempAsset == null)
            {
                tempAsset = assetsRoot.transform.GetChild(i).gameObject;
                
                print($"Object {assetsRoot.transform.GetChild(i).gameObject.name} not found! Saving placeholder object.");
            }
            
            // Create a new SpotData instance to store the asset data
            SpotData newSpotData = new SpotData();
            newSpotData.id = tempAsset.name; // Asset name
            newSpotData.unsnappedPosition = tempAsset.transform.position; // Asset position
            
            // If the asset has an Object_RuntimeDataInspector component, inspect it for variable data
            if (tempAsset.GetComponent<Object_RuntimeDataInspector>())
            {
                tempAsset.GetComponent<Object_RuntimeDataInspector>().Inspect();
                newSpotData.assetData = tempAsset.GetComponent<Object_RuntimeDataInspector>().storedVariableData;
            }
            else
            {
                newSpotData.assetData = new List<VariableData>();
            }
            
            // NEED LAYER/DEPTH ASSIGNMENT HERE
            // newSpotData.layer = tempAsset.layer; // Assign layer information
            
            // Add the asset data to the level data
            _data.assets.Add(newSpotData);
        }
    }

    private void LoadTiles(LevelData _data)
    {
        for (var i = 0; i < _data.tiles.Count; i++)
        {
            TileBase tempTile = null;

            foreach (var group in tileMemory)
            {
                if (group.tiles.Find(t => t.name == _data.tiles[i].id))
                {
                    tempTile = group.tiles.Find(t => t.name == _data.tiles[i].id);
                    break;
                }
            }
            
            if (tempTile == null) continue;
            tilemaps[_data.tiles[i].layer].SetTile(_data.tiles[i].position, tempTile);
        }
    }

    private void LoadAssets(LevelData _data)
    {
        foreach (var spotdata in _data.assets)
        {
            GameObject tempAsset = null;
            Vector3 tempPosition = new Vector3();
            List<VariableData> tempData = new List<VariableData>();

            foreach (var group in assetMemory)
            {
                if (group.assets.Find(t => t.name == spotdata.id))
                {
                    tempAsset = group.assets.Find(t => t.name == spotdata.id);
                    tempPosition = spotdata.unsnappedPosition;
                    tempData = spotdata.assetData;
                    break;
                }
            }

            if (tempAsset == null)
            {
                tempAsset = missingObjectFallback;
                tempAsset.name = spotdata.id;
                tempPosition = spotdata.unsnappedPosition;
                tempData = spotdata.assetData;
                print($"Object {spotdata.id} not found! Placing fallback object.");
            }
            var assetRef = Instantiate(tempAsset, tempPosition, new Quaternion(0, 0, 0, 0), assetsRoot.transform);
            assetRef.name = assetRef.name.Replace("(Clone)", "").Trim();
            if (!assetRef.GetComponent<Object_RuntimeDataInspector>()) continue;
            assetRef.GetComponent<Object_RuntimeDataInspector>().storedVariableData = tempData;
            assetRef.GetComponent<Object_RuntimeDataInspector>().SendVariableDataToScripts();
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    /// <summary>
    /// Saves the current state of the level to a file.
    /// </summary>
    /// <param name="_filePath">The file path to save the level data to.</param>
    public void SaveLevel(string _filePath)
    { 
        // Create a new LevelData instance to store the level information.
        var data = new LevelData();
        
        SaveTiles(data);
        SaveAssets(data);
        
        // Convert the level data to JSON format
        var json = JsonUtility.ToJson(data, true);
        
        // Write the JSON data to the specified file
        File.WriteAllText(_filePath, json); // Save JSON data to file
    }
    
    /// <summary>
    /// Saves the current state of the level to a file.
    /// </summary>
    /// <param name="levelFile">The file path to save the level data to.</param>
    public string GetRawMapData()
    { 
        // Create a new LevelData instance to store the level information.
        var data = new LevelData();
        
        SaveTiles(data);
        SaveAssets(data);
        
        // Convert the level data to JSON format
        var json = JsonUtility.ToJson(data, true);
        
        // Write the JSON data to the specified file
        return json;
    }

    /// <summary>
    /// Loads a level from a file.
    /// </summary>
    /// <param name="_levelFile">The file path to load the level data from.</param>
     public void LoadLevel(string _levelFile)
    {
        var json = File.ReadAllText(_levelFile);
        LoadRawLevel(json);
    }
    
    /// <summary>
    /// Loads a level from a string
    /// </summary>
    /// <param name="_jsonleveldata">The file path to load the level data from.</param>
     public void LoadRawLevel(string _jsonleveldata)
    {
        var data = JsonUtility.FromJson<LevelData>(_jsonleveldata);
        
        foreach (var tilemap in tilemaps) tilemap.ClearAllTiles();
        for (int i = 0; i < assetsRoot.transform.childCount; i++)
        {
            Destroy(assetsRoot.transform.GetChild(i).gameObject);
        }

        LoadTiles(data);
        LoadAssets(data);
    }

    public void LoadLevelFromMemory(string _levelID, bool _waitForSceneLoad=false)
    {
        StartCoroutine(WaitForSceneLoad(_levelID, _waitForSceneLoad));
    }

    private IEnumerator WaitForSceneLoad(string _levelID, bool _waitForSceneLoad)
    {
        while (FindObjectOfType<WorldLoader>().isLoading)
        {
            yield return new WaitForEndOfFrame();
        }
        
        //yield return new WaitForSeconds(0.2f);
        // Load maps while in the Unity editor
        if (Directory.Exists($"{Application.dataPath}/Resources/Maps"))
        {
            LoadLevel($"{Application.dataPath}/Resources/Maps/{_levelID}.ctmap");
        }
        // Load maps while in the built application
        if (Directory.Exists($"{Application.dataPath}/Maps"))
        {
            LoadLevel($"{Application.dataPath}/Maps/{_levelID}.ctmap");
        }
        
    }
    
    
    public void ModifyLevelFile(string _mode)
    {
        StartCoroutine(ShowFileDialogCoroutine(_mode));
    }

    public Tile GetTileFromMemory(string _tileID)
    {
        foreach (var tileMemoryGroup in tileMemory)
        {
            foreach (var tile in tileMemoryGroup.tiles)
            {
                if (tile.name == _tileID) return tile;
            }
        }

        return null;
    }

    public GameObject GetAssetFromMemory(string _assetID)
    {
        foreach (var assetMemoryGroup in assetMemory)
        {
            foreach (var asset in assetMemoryGroup.assets)
            {
                if (asset.name == _assetID) return asset;
            }
        }

        return null;
    }

    public Item GetItemFromMemory(string _itemID)
    {
        foreach (var item in itemMemory)
        {
            if (item.name == _itemID) return item;
        }

        return null;
    }

    public Sprite GetSpriteFromMemory(string _spriteID)
    {
        foreach (var sprite in spriteMemory)
        {
            if (sprite.name == _spriteID) return sprite;
        }

        return missingSpriteFallback;
    }
}
