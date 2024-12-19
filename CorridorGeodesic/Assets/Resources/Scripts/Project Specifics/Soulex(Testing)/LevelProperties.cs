//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProperties: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static LevelProperties Instance;
	public List<string> disabledObjects = new List<string>();
	public List<string> enabledObjects = new List<string>();

    //=-----------------=
    // Private Variables
    //=-----------------=
    private Scene lastScene;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        Instance = this;
        lastScene = SceneManager.GetActiveScene();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //if(arg0 != lastScene)
        //{
        //    enabledObjects.Clear();
        //    disabledObjects.Clear();

        //    lastScene = arg0;

        //    return;
        //}
        foreach (var obj in enabledObjects)
        {
            SearchObjByName(arg0, obj)?.SetActive(true);
        }
        foreach (var obj in disabledObjects)
        {
            SearchObjByName(arg0, obj)?.SetActive(false);
        }
    }
    private GameObject SearchObjByName(Scene scene, string name)
    {
        GameObject[] objs = scene.GetRootGameObjects();

        foreach (var obj in objs)
        {
            if (obj.name == name)
                return obj;
        }
        return null;
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void AddToDisabledObjList(GameObject go)
    {
        disabledObjects.Add(go.name);
    }
    public void AddToEnabledObjList(GameObject go)
    {
        enabledObjects.Add(go.name);
    }
}
