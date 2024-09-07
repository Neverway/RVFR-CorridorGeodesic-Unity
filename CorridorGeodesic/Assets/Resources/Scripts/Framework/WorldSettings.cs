//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldSettings : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public GameMode gameModeOverride;
    public bool startAsSpectator;
    public bool disableWorldKillVolume;
    public bool disableWorldKillY;
    public int worldKillVolumeDistance=32767;
    public int worldKillYDistance = -100;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        InvokeRepeating(nameof(CheckKillVolume), 0, 2);
    }

    private void Update()
    {
        if (!gameInstance.localPlayerCharacter)
        {
            SpawnPlayerCharacter();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (!disableWorldKillVolume) Gizmos.DrawCube(new Vector3(0,0,0), new Vector3(worldKillVolumeDistance*0.5f,worldKillVolumeDistance*0.5f,worldKillVolumeDistance*0.5f));
        if (!disableWorldKillY) Gizmos.DrawCube(new Vector3(0,worldKillYDistance,0), new Vector3(1000,1,1000));
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void CheckKillVolume()
    {
        if (disableWorldKillVolume && disableWorldKillY) return;
        foreach (var pawn in FindObjectsOfType<Pawn>())
        {
            if (!disableWorldKillVolume)
            {
                var distanceToEntity = Vector3.Distance(pawn.transform.position,  new Vector3(0,0,0));
                if (distanceToEntity >= worldKillVolumeDistance || distanceToEntity <= (worldKillVolumeDistance * -1))
                {
                    Destroy(pawn.gameObject);
                }
            }
            if (!disableWorldKillY)
            {
                if (pawn.transform.position.y <= worldKillYDistance)
                {
                    Destroy(pawn.gameObject);
                }
            }
        }
        foreach (var actor in FindObjectsOfType<ActorData>())
        {
            if (!actor.CompareTag("PhysProp")) continue;
            
            if (!disableWorldKillVolume)
            {
                var distanceToEntity = Vector3.Distance(actor.transform.position,  new Vector3(0,0,0));
                if (distanceToEntity >= worldKillVolumeDistance || distanceToEntity <= (worldKillVolumeDistance * -1))
                {
                    Destroy(actor.gameObject);
                }
            }
            if (!disableWorldKillY)
            {
                if (actor.transform.position.y <= worldKillYDistance)
                {
                    Destroy(actor.gameObject);
                }
            }
        }
    }

    private static Transform GetPlayerStartPoint(bool _usePlayerStart = true)
    {
        if (!_usePlayerStart) return null;
        var allPossibleStartPoints = FindObjectsOfType<PlayerStart>();
        var allValidStartPoints = allPossibleStartPoints.Where(_possibleStartPoint => _possibleStartPoint.playerStartFilter == "").ToList();

        if (allValidStartPoints.Count == 0) return null;
        var random = Random.Range(0, allValidStartPoints.Count);
        return allValidStartPoints[random].transform;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SpawnPlayerCharacter()
    {
        if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
        
        // Perform a check first to see if there is already a local player character in the scene
        //Debug.Log("LocalPlayerCharacter is empty in the gameInstance! Has the player spawned yet? Checking...");
        foreach (var pawn in FindObjectsOfType<Pawn>())
        {
            if (pawn.isPossessed)
            {
                gameInstance.localPlayerCharacter = pawn;
                //Debug.Log("A possessed actor was found, assigning them to be the LocalPlayerCharacter.");
                return;
            }
        }
        //Debug.Log("No possessed actors found. Checking actor controllers to see if any of them are player driven...");
        foreach (var pawn in FindObjectsOfType<Pawn>())
        {
            if (gameInstance.PlayerControllerClasses.Contains(pawn.currentController))
            {
                gameInstance.localPlayerCharacter = pawn;
                //Debug.Log("A valid controller actor was found, assigning them to be the LocalPlayerCharacter.");
                return;
            }
        }
        //Debug.Log("No player found. Let's spawn a new one!");
        
        var startPoint = GetPlayerStartPoint();
        
        // Determine the game mode to use - either the override or the default.
        var gameMode = gameModeOverride ? gameModeOverride : gameInstance.defaultGamemode;
        // Choose the class type based on whether starting as a spectator.
        var classToInstantiate = startAsSpectator ? gameMode.spectatorClass : gameMode.defaultPawnClass;
        // Determine the spawn position and rotation - use default if startPoint is null.
        Vector3 spawnPosition = startPoint ? startPoint.position : Vector3.zero;
        Quaternion spawnRotation = startPoint ? startPoint.rotation : Quaternion.identity;
        // Instantiate the player character.
        gameInstance.localPlayerCharacter = Instantiate(classToInstantiate, spawnPosition, spawnRotation).GetComponent<Pawn>();

    }
}
