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
    public bool disableWorldKillVolume;
    public int worldKillVolumeDistance=32767;


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
    }

    private void Update()
    {
        if (!gameInstance.localPlayerCharacter) SpawnPlayerCharacter();
        CheckKillVolume();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void CheckKillVolume()
    {
        if (disableWorldKillVolume) return;
        foreach (var pawn in FindObjectsOfType<Pawn>())
        {
            var distanceToEntity = Vector3.Distance(pawn.transform.position,  new Vector3(0,0,0));
            if (distanceToEntity >= worldKillVolumeDistance || distanceToEntity <= (worldKillVolumeDistance * -1))
            {
                Destroy(pawn.gameObject);
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
        
        var startPoint = GetPlayerStartPoint();
        
        // Check for gamemode override
        if (gameModeOverride)
        {
            if (startPoint)
            {
                gameInstance.localPlayerCharacter = Instantiate(gameModeOverride.defaultPawnClass, startPoint.position, startPoint.rotation).GetComponent<Pawn>();
            }
            else
            {
                gameInstance.localPlayerCharacter = Instantiate(gameModeOverride.defaultPawnClass, new Vector3(), new Quaternion()).GetComponent<Pawn>();
            }
        }
        else
        {
            if (startPoint)
            {
                gameInstance.localPlayerCharacter = Instantiate(gameInstance.defaultGamemode.defaultPawnClass, startPoint.position, startPoint.rotation).GetComponent<Pawn>();
            }
            else
            {
                gameInstance.localPlayerCharacter = Instantiate(gameInstance.defaultGamemode.defaultPawnClass, new Vector3(), new Quaternion()).GetComponent<Pawn>();
            }
        }
    }
}
