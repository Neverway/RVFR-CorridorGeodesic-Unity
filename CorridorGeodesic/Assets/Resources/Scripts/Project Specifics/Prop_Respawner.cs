//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose: Spawns a prefab object on a timer, and spawns it again if it is destroyed.
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop_Respawner : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=

    [SerializeField] GameObject prefabToSpawn;
    private GameObject spawnedObject;
    [LogicComponentHandle] public LogicComponent respawnProp;
    private bool previousPowered = false;

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private float spawnDelay;
    private Coroutine spawnWorker;
    [Tooltip("Wait for a DestroySpawnedObject call before spawning the first object.")]
    [SerializeField] private bool waitForRespawn = false;
    [Tooltip("If false, the spawner will always set waitForRespawn when the spawned object is destroyed.")]
    [SerializeField] private bool autoRespawn = true;


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Update()
    {
        if (waitForRespawn == false)
        {
            if (spawnedObject == null && spawnWorker == null)
            {
                spawnWorker = StartCoroutine(SpawnWorker());
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private IEnumerator SpawnWorker()
    {
        yield return new WaitForSeconds(spawnDelay);
        spawnedObject = Instantiate(prefabToSpawn, transform.position, transform.rotation);
        spawnWorker = null;
        if (autoRespawn == false)
        {
            waitForRespawn = true;
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=

    /// <summary>
    /// Destroy the spawned object so that it will respawn.
    /// </summary>
    public void DestroySpawnedObject()
    {
        StartCoroutine(DestroyWorker());
    }

    /// <summary>
    /// Destroy the object after moving it to trigger OnTriggerExit for any objects it's inside of.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyWorker()
    {
        waitForRespawn = false;
        if (spawnedObject == null)
        {
            yield break;
        }

        //Move the spawne dobject far away, triggering OnTriggerExit for anything it may have been inside of
        spawnedObject.transform.position = transform.position + Vector3.one * 10000f;
        //wait one frame, then destroy object
        yield return null;
        Destroy(spawnedObject);
    }

    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        if (respawnProp.isPowered && !previousPowered)
        {
            DestroySpawnedObject();

            if (spawnWorker == null)
            {
                spawnWorker = StartCoroutine(SpawnWorker());
            }
        }
        previousPowered = respawnProp.isPowered;

    }
}