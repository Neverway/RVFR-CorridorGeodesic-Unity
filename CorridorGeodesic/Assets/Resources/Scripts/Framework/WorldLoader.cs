//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldLoader : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] public float delayBeforeWorldChange = 0.25f;
    [SerializeField] public float minimumRequiredLoadTime = 1f;
    [SerializeField] private string loadingWorldID = "_Travel";
    public static event Action OnWorldLoaded;
    public bool isLoading;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private string targetWorldID;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Image loadingBar;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator Load()
    {
        isLoading = true;
        yield return new WaitForSeconds(delayBeforeWorldChange);
        SceneManager.LoadScene(loadingWorldID);
	    
        // The following should execute on the loading screen scene
        var loadingBarObject = GameObject.FindWithTag("LoadingBar");
        if (loadingBarObject) loadingBar = loadingBarObject.GetComponent<Image>();
	    
        yield return new WaitForSeconds(minimumRequiredLoadTime);
        StartCoroutine(LoadAsyncOperation());
    }
    
    private IEnumerator ForceLoad(float _loadDelay)
    {
        yield return new WaitForSeconds(_loadDelay);
        SceneManager.LoadScene(targetWorldID);
    }

    private IEnumerator LoadAsyncOperation()
    {
        // Create an async operation (Will automatically switch to target scene once it's finished loading)
        var targetLevel = SceneManager.LoadSceneAsync(targetWorldID);
	    
        while (targetLevel.progress < 1)
        {
            // Set loading bar to reflect async progress
            if (loadingBar) loadingBar.fillAmount = targetLevel.progress;
            yield return new WaitForEndOfFrame();
        }

        isLoading = false;
        // Scene has finished loading, trigger the SceneLoaded event
        if (OnWorldLoaded != null)
        {
            OnWorldLoaded.Invoke();
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void LoadWorld(string _worldID)
    {
        targetWorldID = _worldID;
        if (!DoesSceneExist(_worldID))
        {
            Debug.LogWarning(_worldID + " Is not a valid level! Loading fallback scene...");
            targetWorldID = "_Error";
            Destroy(GameInstance.GetWidget("WB_Loading"));
        }
        StartCoroutine(Load());
    }
    
    public void ForceLoadWorld(string _targetSceneID, float _delay)
    {
        targetWorldID = _targetSceneID;
        if (!DoesSceneExist(_targetSceneID))
        {
            Debug.LogWarning(_targetSceneID + " Is not a valid level! Loading fallback scene...");
            targetWorldID = "_Error";
            Destroy(GameInstance.GetWidget("WB_Loading"));
        }
        StartCoroutine(ForceLoad(_delay));
    }
    
    // This code was expertly copied from @Yagero on github.com
    // https://gist.github.com/yagero/2cd50a12fcc928a6446539119741a343
    // (Seriously though, this function is a life saver, so thanks!)
    public static bool DoesSceneExist(string _targetSceneID)
    {
        if (string.IsNullOrEmpty(_targetSceneID))
            return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var lastSlash = scenePath.LastIndexOf("/");
            var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (string.Compare(_targetSceneID, sceneName, true) == 0)
                return true;
        }

        return false;
    }
}
