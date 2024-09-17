//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Volume_LevelStreaming : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private WorldLoader worldLoader;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private new void OnTriggerEnter2D(Collider2D _other)
    {
        worldLoader = FindObjectOfType<WorldLoader>();
        if (_other.GetComponent<Pawn>() || _other.CompareTag("PhysProp"))
        {
            SceneManager.MoveGameObjectToScene(_other.gameObject, SceneManager.GetSceneByName(worldLoader.streamingWorldID));
        }
    }
    
    private new void OnTriggerEnter(Collider _other)
    {
        worldLoader = FindObjectOfType<WorldLoader>();
        if (_other.GetComponent<Pawn>() || _other.CompareTag("PhysProp"))
        {
            _other.transform.SetParent(null);

            Game_LevelHelpers.StoreObjectRelativePosition(_other.gameObject.GetInstanceID(), _other.transform);

            SceneManager.MoveGameObjectToScene(_other.gameObject, SceneManager.GetSceneByName(worldLoader.streamingWorldID));
        }
    }
    
    private new void OnTriggerExit2D(Collider2D _other)
    {
        worldLoader = FindObjectOfType<WorldLoader>();
        if (_other.GetComponent<Pawn>() || _other.CompareTag("PhysProp"))
        {
            SceneManager.MoveGameObjectToScene(_other.gameObject, SceneManager.GetActiveScene());
        }
    }
    
    private new void OnTriggerExit(Collider _other)
    {
        worldLoader = FindObjectOfType<WorldLoader>();
        if (_other.GetComponent<Pawn>() || _other.CompareTag("PhysProp"))
        {
            SceneManager.MoveGameObjectToScene(_other.gameObject, SceneManager.GetActiveScene());

            RotationPositionBinding binding = Game_LevelHelpers.GetObjectWorldStartPosition(_other.gameObject.GetInstanceID());

            _other.transform.position = binding.position;
            _other.transform.rotation = binding.rotation;
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
