//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_LevelLoadManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static Game_LevelLoadManager Instance;

    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector3 relativePosition;
    private bool levelEndNotFound = true;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        Instance = this;
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
    //private IEnumerator LoadLevelAsync(string levelID)
    //{
        //AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(levelID, LoadSceneMode.Single);
        //sceneLoad.allowSceneActivation = false;

        //while (sceneLoad.progress < 1f)
        //{
        //    yield return null;
        //}

        //sceneLoad.allowSceneActivation = true;
    //}
    //private void LoadLevelAsync(string levelID)
    //{
        //AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(levelID, LoadSceneMode.Single);

        //sceneLoad.completed += SceneLoad_completed;
    //}
    //private void SceneLoad_completed(AsyncOperation obj)
    //{
    //    if (levelEndNotFound)
    //    {
    //        if (!Game_LevelStartIdentifier.Instance)
    //        {
    //            Player.Instance.transform.position = Vector3.zero;
    //            Debug.LogError("No Game Level Start ID");
    //        }
    //        else
    //            Player.Instance.transform.position = Game_LevelStartIdentifier.Instance.transform.position;

    //        levelEndNotFound = false;
    //        return;
    //    }
    //    StartCoroutine(WaitForPlayer());

    //    obj.completed -= SceneLoad_completed;
    //}
    //IEnumerator WaitForPlayer()
    //{
    //    while (!Player.Instance)
    //    {
    //        yield return null;
    //    }
    //    Player.Instance.transform.position = Game_LevelStartIdentifier.Instance.startPosition.TransformPoint(relativePosition);

    //    relativePosition = Vector3.zero;
    //}
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(SceneStart());
    }
    IEnumerator SceneStart()
    {
        while (!Player.Instance)
            yield return null;

        if (!Game_LevelStartIdentifier.Instance)
        {
            SpawnDefault();
            yield break;
        }

        if (levelEndNotFound)
        {
            Player.Instance.transform.position = Game_LevelStartIdentifier.Instance.transform.position;

            relativePosition = Vector3.zero;

            levelEndNotFound = false;
            yield break;
        }

        Player.Instance.transform.position = Game_LevelStartIdentifier.Instance.transform.TransformPoint(relativePosition);

        relativePosition = Vector3.zero;
    }
    void SpawnDefault()
    {
        Player.Instance.transform.position = Vector3.zero;
        Debug.LogError("No Game Level Start ID");

        relativePosition = Vector3.zero;
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void LoadLevel(string levelID)
    {
        levelEndNotFound = false;
        if (!Game_LevelEndIdentifier.Instance)
        {
            Debug.LogError("No Game Level End ID");
            levelEndNotFound = true;
            //StartCoroutine(LoadLevelAsync(levelID));
            SceneManager.LoadScene(levelID);
            return;
        }
        relativePosition = Game_LevelEndIdentifier.Instance.endPosition.InverseTransformPoint(Player.Instance.transform.position);

        //StartCoroutine(LoadLevelAsync(levelID));
        SceneManager.LoadScene(levelID);
    }
}