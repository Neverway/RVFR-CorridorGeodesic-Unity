//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
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
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
    }

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
}