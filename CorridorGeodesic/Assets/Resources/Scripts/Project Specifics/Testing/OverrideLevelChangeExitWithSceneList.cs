using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Neverway.Framework.LogicSystem;
using Neverway.Framework.PawnManagement;
using Neverway.Framework;

public class OverrideLevelChangeExitWithSceneList : MonoBehaviour
{
    public static OverrideLevelChangeExitWithSceneList Instance;

    public SceneList levelpack;
    public bool destroyStreamingVolumes = true;


    private Volume_LevelChange levelChangeVolume;
    private PlayerStart playerStart;
    private Pawn player;
    private int currentSceneIndex;

    public void Initialize(SceneList levelpack, bool destroyStreamingVolumes)
    {
        this.levelpack = levelpack;
        this.destroyStreamingVolumes = destroyStreamingVolumes;
    }

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += (s, m) => { OnLevelLoad(s.name); };
    }

    public void Update()
    {
        OverrideVolumeLevelChangeWorldID(IndexToSceneName(currentSceneIndex + 1));

        //if pressing shift
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            if (Input.GetKeyDown(KeyCode.N)) // Go to Next level
            {
                FindObjectOfType<WorldLoader>().LoadWorld(IndexToSceneName(++currentSceneIndex));
                return;
            }
            if (Input.GetKeyDown(KeyCode.B)) //Go Back a level
            {
                FindObjectOfType<WorldLoader>().LoadWorld(IndexToSceneName(--currentSceneIndex));
                return;
            }
        }
        
    }
    
    public string IndexToSceneName(int index)
    {
        string toReturn = 
            (index < 0 || index >= levelpack.sceneNames.Length) ? //check if out of range of levelpack.sceneNames
        "_Title" : levelpack.sceneNames[index]; //use the indexed scene name, or if out of range, use "_Title"

        return toReturn;
    }

    public void OnLevelLoad(string loadingScene)
    {
        //Destroy self on title screen to avoid making duplicates
        if (loadingScene == "_Title")
        {
            Instance = null;
            Destroy(gameObject);
            return;
        }

        //Destroy streaming volumes if setting was enabled
        if (destroyStreamingVolumes)
        {
            Volume_LevelStreaming[] streamingVolumes = FindObjectsOfType<Volume_LevelStreaming>();
            foreach (Volume_LevelStreaming v in streamingVolumes)
            {
                Destroy(v);
            }
        }
    }

    public void OverrideVolumeLevelChangeWorldID(string newID)
    {
        if(levelChangeVolume == null)
        {
            levelChangeVolume = levelChangeVolume = FindObjectOfType<Volume_LevelChange>();
            if (levelChangeVolume == null)
            {
                return;
            }
        }
        if (levelChangeVolume.gameObject.scene.name == IndexToSceneName(currentSceneIndex) && levelChangeVolume.worldID != newID)
        {
            Debug.Log("OVERRIDING Volume_LevelChange worldID TO: " + newID);
            levelChangeVolume.worldID = newID;
        }
    }

    private void MovePlayerToLevelChangeVolume()
    {
        if (player == null)
            player = FindAnyObjectByType<Pawn>();
        if (levelChangeVolume == null)
            levelChangeVolume = FindObjectOfType<Volume_LevelChange>();

        if (player != null && levelChangeVolume != null)
        {
            player.transform.position = levelChangeVolume.transform.position;
        }
    }
}
