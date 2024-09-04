//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Creates one target object, if that object is destroyed, create another
// Notes:
//
//=============================================================================

using System.Collections;
using UnityEngine;

public class Prop_Respawner : MonoBehaviour
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