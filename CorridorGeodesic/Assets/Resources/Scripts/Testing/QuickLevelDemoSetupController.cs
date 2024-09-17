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
    public static QuickLevelDemoSetupController Instance;

    public QuickLevelDemoSetup levelSetup;

    private Volume_LevelChange levelChangeVolume;
    private PlayerStart playerStart;
    private Pawn player;
    private int levelID = 0;
    private string nextSceneName;
    private Scene previousScene, currentScene;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        nextSceneName = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += (s, m) => { OverrideLevelExit(s); } ;
    }
    public void Update()
    {
        OverrideVolumeLevelChangeWorldID(nextSceneName);
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.N))
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

    public void OverrideLevelExit(Scene scene)
    {
        if (scene.name != nextSceneName)
            return;

        previousScene = currentScene;
        currentScene = scene;

        //levelChangeVolume = FindObjectOfType<Volume_LevelChange>();
        //playerStart = FindAnyObjectByType<PlayerStart>();
        //player = FindAnyObjectByType<Pawn>();

        int bigNumber = 100;
        while (levelID >= levelSetup.sceneNames.Length)
        {
            //Replace levelSetup with next set of levels if it exists
            if (levelSetup.nextLevelDemoSetup != null)
            {
                levelSetup = levelSetup.nextLevelDemoSetup;
                levelID = 0;
            }
            else
            {
                Destroy(this);
                return;
            }
            //Protects infinite loops and if no next levelSetup was found
            if (bigNumber-- <= 0) 
            {
                Destroy(this);
                return;
            }
        }
        
        do
        {
            nextSceneName = levelSetup.sceneNames[levelID];
            levelID += 1;

        } while (nextSceneName == currentScene.name);
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
        if(levelChangeVolume.worldID != newID)
        {
            Debug.Log("OVERRIDING Volume_LevelChange worldID TO: " + newID);
            levelChangeVolume.worldID = newID;
        }
    }
}
