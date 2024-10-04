using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LemonBuddyDestroyedTracker : MonoBehaviour
{
    public bool clearBuddiesOnAwake = false;

    public void Awake()
    {
        if (clearBuddiesOnAwake)
            scenesBuddyWasDestroyedIn = new List<int>();
    }

    //static fields and methods

    public static List<int> scenesBuddyWasDestroyedIn = new List<int>();
    public void OnBuddyDestroyed()
    {
        int newSceneIndex = gameObject.scene.buildIndex;
        if (!scenesBuddyWasDestroyedIn.Contains(newSceneIndex))
        {
            scenesBuddyWasDestroyedIn.Add(newSceneIndex);
            EndOfDemoStatsTracker.instance.UpdateBuddies(scenesBuddyWasDestroyedIn.Count);
        }
    }
}
