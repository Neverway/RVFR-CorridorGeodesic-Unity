using BzKovSoft.ObjectSlicer.EventHandlers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class QuickLevelDemoSetupController : MonoBehaviour
{
    public QuickLevelDemoSetup levelSetup;

    private Volume_LevelChange levelChangeVolume;
    private PlayerStart playerStart;
    private Pawn player;
    private int levelID = 0;
    private string nextSceneName;
    private Scene previousScene, currentScene;

    public void Awake()
    {
        DontDestroyOnLoad(this);
        nextSceneName = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += (s, m) => { OverrideLevelExit(s); } ;
    }
    public void Update()
    {
        OverrideVolumeLevelChangeWorldID(nextSceneName);
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.N))
        {
            if (player != null && levelChangeVolume != null)
            {
                player.transform.position = levelChangeVolume.transform.position;
            }
        }
    }

    public IEnumerator PutPlayerAtStart()
    {
        //while (player == null)
        //{
        //    player = FindAnyObjectByType<Pawn>();
        //    yield return null;
        //}
        //while (playerStart == null)
        //{
        //    playerStart = FindAnyObjectByType<PlayerStart>();
        //    yield return null;
        //}
        //yield return new WaitForSeconds(1);

        while(SceneManager.GetAllScenes().Contains(previousScene))
        {
            yield return null;
        }

        while(playerStart == null)
        {
            playerStart = FindAnyObjectByType<PlayerStart>();
            yield return null;
        }

        if (player != null && playerStart != null)
        {
            Debug.LogWarning("SETTING PLAYER POSITION MANUALLY TO THE START, SHOULD FIX LEVEL LOADING INSTEAD");
            player.transform.position = playerStart.transform.position;
        }
    }

    public void OverrideLevelExit(Scene scene)
    {
        if (scene.name != nextSceneName)
            return;

        previousScene = currentScene;
        currentScene = scene;

        levelChangeVolume = FindObjectOfType<Volume_LevelChange>();
        playerStart = FindAnyObjectByType<PlayerStart>();
        player = FindAnyObjectByType<Pawn>();

        StartCoroutine(PutPlayerAtStart());

        QuickLevelDemoSetup firstLevelSetup = levelSetup;
        while (levelID >= levelSetup.sceneNames.Length)
        {
            if (levelSetup.nextLevelDemoSetup != null)
            {
                levelSetup = levelSetup.nextLevelDemoSetup;
                levelID = 0;
            }
            if (levelSetup == firstLevelSetup) //Protects infinite loops and if no next levelSetup was found
            {
                Destroy(this);
                return;
            }
        }

        if (levelChangeVolume == null)
        {
            return;
        }
        nextSceneName = levelSetup.sceneNames[levelID];
        OverrideVolumeLevelChangeWorldID(nextSceneName);

        levelID += 1;
    }

    public void OverrideVolumeLevelChangeWorldID(string newID)
    {
        if(levelChangeVolume == null)
        {
            levelChangeVolume = levelChangeVolume = FindObjectOfType<Volume_LevelChange>();
        }
        if(levelChangeVolume.worldID != newID)
        {
            Debug.Log("OVERRIDING Volume_LevelChange worldID TO: " + newID);
            levelChangeVolume.worldID = newID;
        }
    }
}
