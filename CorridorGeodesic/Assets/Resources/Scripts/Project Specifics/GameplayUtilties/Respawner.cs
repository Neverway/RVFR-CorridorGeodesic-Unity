//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose: Spawns a prefab object on a timer, and spawns it again if it is destroyed.
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=

    [SerializeField] GameObject prefabToSpawn;
    private GameObject spawnedObject;

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private float spawnDelay;
    private Coroutine spawnWorker;


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Update()
    {
        if (spawnedObject == null && spawnWorker == null)
        {
            spawnWorker = StartCoroutine(SpawnWorker());
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private IEnumerator SpawnWorker ()
    {
        yield return new WaitForSeconds(spawnDelay);
        spawnedObject = Instantiate (prefabToSpawn, transform.position, transform.rotation);
        spawnWorker = null;
    }

    //=-----------------=
    // External Functions
    //=-----------------=

    /// <summary>
    /// Destroy the spawned object so that it will respawn.
    /// </summary>
    public void DestroySpawnedObject ()
    {
        StartCoroutine (DestroyWorker ());
    }

    /// <summary>
    /// Destroy the object after moving it to trigger OnTriggerExit for any objects it's inside of.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyWorker ()
    {
        //Move the spawne dobject far away, triggering OnTriggerExit for anything it may have been inside of
        spawnedObject.transform.position = new Vector3 (0, 1000, 0);
        //wait one frame, then destroy object
        yield return null;
        Destroy (spawnedObject);
    }

}