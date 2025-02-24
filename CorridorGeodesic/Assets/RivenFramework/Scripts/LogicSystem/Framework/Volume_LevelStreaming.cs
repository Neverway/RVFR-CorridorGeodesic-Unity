//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: 
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.SceneManagement;
using Neverway.Framework.PawnManagement;

namespace Neverway.Framework.LogicSystem
{
    public class Volume_LevelStreaming : Volume
    {
        //=-----------------=
        // Public Variables
        //=-----------------=


        //=-----------------=
        // Private Variables
        //=-----------------=
        [SerializeField] private Vector3 exitOffset;
        [SerializeField] private bool debugDrawExitZone;
        private bool initializedExitZone;


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private WorldLoader worldLoader;
        private GameObject streamContainer;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            worldLoader = FindObjectOfType<WorldLoader>();
            streamContainer = transform.GetChild(0).gameObject;
            streamContainer.GetComponent<Volume_LevelStreamContainer>().exitOffset = exitOffset;
            streamContainer.GetComponent<Volume_LevelStreamContainer>().parentStreamVolume = gameObject;
            streamContainer.transform.SetParent(null);
            worldLoader = FindObjectOfType<WorldLoader>();
        }

        private void Update()
        {
            if (initializedExitZone) return;
            if (SceneManager.GetSceneByName(worldLoader.streamingWorldID).IsValid())
            {
                streamContainer.GetComponent<Volume_LevelStreamContainer>().initializedExitZone = true;
                initializedExitZone = true;
                SceneManager.MoveGameObjectToScene(streamContainer.gameObject, SceneManager.GetSceneByName(worldLoader.streamingWorldID));
                streamContainer.transform.SetParent(null);
            }
        }

        private void OnDrawGizmos()
        {
            if (!debugDrawExitZone) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position+exitOffset, transform.localScale);
        }

        private new void OnTriggerEnter2D(Collider2D _other)
        {
            worldLoader = FindObjectOfType<WorldLoader>();
            if (_other.GetComponent<Pawn>() || _other.CompareTag("PhysProp"))
            {
                SceneManager.MoveGameObjectToScene(_other.gameObject,
                    SceneManager.GetSceneByName(worldLoader.streamingWorldID));
            }
        }

        private new void OnTriggerStay(Collider _other)
        {
            if (!initializedExitZone) return;
            if (_other.GetComponent<Pawn>() || _other.CompareTag("PhysProp"))
            {
                if (_other.transform.parent == streamContainer.transform) return;
                _other.transform.SetParent(null);
                if (SceneManager.GetSceneByName(worldLoader.streamingWorldID).IsValid())
                {
                    SceneManager.MoveGameObjectToScene(_other.gameObject, SceneManager.GetSceneByName(worldLoader.streamingWorldID));
                    _other.transform.SetParent(streamContainer.transform);
                }
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
                _other.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(_other.gameObject, SceneManager.GetActiveScene());

                //RotationPositionBinding binding = Game_LevelHelpers.GetObjectWorldStartPosition(_other.gameObject.GetInstanceID());

                //_other.transform.position = binding.position;
                //_other.transform.rotation = binding.rotation;
            }
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}