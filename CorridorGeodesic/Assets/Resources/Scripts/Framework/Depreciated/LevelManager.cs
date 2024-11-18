//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Handles the saving and loading of map data
// Notes: This needs a major rework along with the whole cartographer system
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
using Neverway.Framework.Customs;

namespace Neverway.Framework.Cartographer
{
    public class LevelManager : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        /*
        [Tooltip("A list of all the tiles and their categories used for the current project")]
        public List<TileMemoryGroup> tileMemory;
        [Tooltip("A list of all the objects and their categories used for the current project")]
        public List<PropMemoryGroup> assetMemory;
        [Tooltip("A list of all the sprites that can be used for decor props for the current project")]
        public List<Sprite> spriteMemory;
        public List<Item> itemMemory;
        public Tile missingTileFallback;
        public GameObject missingObjectFallback;
        public Sprite missingSpriteFallback;*/
        [Header("READ-ONLY (Don't touch!)")] [Tooltip("A list of all of the 'tile layers' used in the scene")]
        public List<Tilemap> tilemaps;

        public GameObject assetsRoot;
        public string filePath;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private ProjectData projectData;
        [SerializeField] private UISkin fileBrowserSkin;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Awake()
        {
            projectData = FindObjectOfType<ProjectData>();
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
                "Load" => FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, false, startingPath, null,
                    "Load Cartographer Map File", "Load"),
                "Save" => FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, false, startingPath, null,
                    "Save Cartographer Map File", "Save"),
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
                        foreach (var group in projectData.tiles)
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
                            assetId = tempTile.name, // Tile name
                            tilePosition = new Vector3Int(x, y, 0), // Tile position
                            layer = tilemaps.IndexOf(tilemap), // Tilemap layer
                            layerID = tilemap.name // Tilemap name
                        };
                        _data.tiles.Add(newSpotData); // Add tile data to the level data
                    }
                }
            }
        }

        private void SaveActors(LevelData _data)
        {
            // Save object data
            // Find all objects with an asset_instanceID component
            // For each one, see if we can find the root asset in assetMemory
            // If not, skip it
            // else, add the asset data for name, unique id, position, and any variable data

            foreach (var actor in FindObjectsOfType<ActorData>())
            {
                // Only save objects that are in the actor container
                // (I added this here because it was trying to save objects that were part of the level editor world)
                if (!actor.gameObject.transform.parent) continue;
                if (assetsRoot.gameObject != actor.gameObject.transform.parent.gameObject) continue;

                GameObject tempAsset = null;
                if (projectData.GetActorFromMemory(actor.actorId))
                {
                    // If the asset is found in assetMemory, set it as tempAsset
                    tempAsset = actor.gameObject;
                }

                // If the asset is not found in assetMemory, skip it
                if (tempAsset == null)
                {
                    tempAsset = projectData.missingObjectFallback;
                    print($"Object {actor.actorId} not found! Saving placeholder object.");
                }

                // Create a new SpotData instance to store the asset data
                SpotData newSpotData = new SpotData();
                newSpotData.assetId = actor.actorId;
                newSpotData.worldPosition = tempAsset.transform.position; // Asset position

                // If the asset has runtime data, inspect it for variable data
                actor.Inspect();
                newSpotData.assetData = actor.storedVariableData;

                // Add the asset data to the level data
                _data.assets.Add(newSpotData);
            }
        }

        private void LoadTiles(LevelData _data)
        {
            for (var i = 0; i < _data.tiles.Count; i++)
            {
                TileBase tempTile = null;

                foreach (var group in projectData.tiles)
                {
                    if (group.tiles.Find(t => t.name == _data.tiles[i].assetId))
                    {
                        tempTile = group.tiles.Find(t => t.name == _data.tiles[i].assetId);
                        break;
                    }
                }

                if (tempTile == null) continue;
                tilemaps[_data.tiles[i].layer].SetTile(_data.tiles[i].tilePosition, tempTile);
            }
        }

        private void LoadAssets(LevelData _data)
        {
            // Look through all the spots
            foreach (var spotdata in _data.assets)
            {
                // Create starting values as nulls (in case they don't get a value later we still want them to be defined)
                GameObject tempAsset = null;
                string tempId = "";
                Vector3 tempPosition = new Vector3();
                List<VariableData> tempData = new List<VariableData>();

                // Find the actor that shares the same id as the spot id and assign the object, position, and data of the spot to our temp values
                if (projectData.GetActorFromMemory(spotdata.assetId))
                {
                    tempAsset = projectData.GetActorFromMemory(spotdata.assetId).AssociatedGameObject;
                    tempId = spotdata.assetId;
                    tempPosition = spotdata.worldPosition;
                    tempData = spotdata.assetData;
                }

                // Place a fallback object if we didn't find the actor in the project data
                if (tempAsset == null)
                {
                    tempAsset = projectData.missingObjectFallback;
                    tempId = spotdata.assetId;
                    tempPosition = spotdata.worldPosition;
                    tempData = spotdata.assetData;
                    print($"Object {spotdata.assetId} not found! Placing fallback object.");
                }

                // Create the actor at the specified transform
                var assetRef = Instantiate(tempAsset, tempPosition, new Quaternion(0, 0, 0, 0), assetsRoot.transform);
                // Remove the "(Clone)" part from the name since we're not cringe
                assetRef.name = assetRef.name.Replace("(Clone)", "").Trim();

                // Assign the actor data
                assetRef.GetComponent<ActorData>().actorId = tempId;
                assetRef.GetComponent<ActorData>().storedVariableData = tempData;
                assetRef.GetComponent<ActorData>().SendVariableDataToScripts();
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
            SaveActors(data);
            print($"Writing {data.assets.Count} actors & {data.tiles.Count} tiles to {_filePath}");

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
            SaveActors(data);

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

        public void LoadLevelFromMemory(string _levelID, bool _waitForSceneLoad = false)
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
    }
}
