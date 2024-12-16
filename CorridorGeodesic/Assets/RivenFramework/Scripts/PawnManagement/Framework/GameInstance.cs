//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: A persistent script that stores values of variables globally for
//  all players to access
// Notes:
//
//=============================================================================

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Neverway.Framework.PawnManagement;

namespace Neverway.Framework.PawnManagement
{
    public class GameInstance : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public GameMode defaultGamemode;
        //[IsDomainReloaded] 
        public static GameInstance Instance;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=
        [Tooltip("0-Title, 1-Loading, 2-Pause, 3-LevelEditor, 4-HUD, 5-Inventory, 6-Framecounter")]
        public List<GameObject> UserInterfaceWidgets;

        public List<PawnController> PlayerControllerClasses;
        [HideInInspector] public Pawn localPlayerCharacter;

        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Awake()
        {
            Instance = this;
        }

        private void LateUpdate()
        {
            // If no player was found, enable a random pawns camera
            if (localPlayerCharacter == null)
            {
                var emergencyFallback = FindObjectOfType<Pawn>();
                if (emergencyFallback)
                {
                    localPlayerCharacter = emergencyFallback;
                    emergencyFallback.GetComponentInChildren<Camera>(true).gameObject.SetActive(true);
                }
            }
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=

        //=-----------------=
        // Core External Functions
        //=-----------------=
        public void CreateNewPlayerCharacter(GameMode _gamemode)
        {
            // NEED FUNCTION TO FIND VALID PLAYER START POINT
            var startpoint = GetPlayerStartPoint().transform;
            localPlayerCharacter = Instantiate(_gamemode.defaultPawnClass, startpoint.position, startpoint.rotation)
                .GetComponent<Pawn>();
            localPlayerCharacter.isPossessed = false;
            localPlayerCharacter.name = localPlayerCharacter.name.Replace("(Clone)", "").Trim();
        }

        public void CreateNewPlayerCharacter(GameMode _gamemode, bool _isLocalPlayer)
        {
            // NEED FUNCTION TO FIND VALID PLAYER START POINT
            var startpoint = GetPlayerStartPoint().transform;
            localPlayerCharacter = Instantiate(_gamemode.defaultPawnClass, startpoint.position, startpoint.rotation)
                .GetComponent<Pawn>();
            localPlayerCharacter.isPossessed = _isLocalPlayer;
            localPlayerCharacter.name = localPlayerCharacter.name.Replace("(Clone)", "").Trim();
        }

        public void CreateNewPlayerCharacter(GameMode _gamemode, bool _isLocalPlayer, bool _usePlayerStart)
        {
            // NEED FUNCTION TO FIND VALID PLAYER START POINT
            if (_usePlayerStart && FindObjectOfType<PlayerStart>())
            {
                var startpoint = GetPlayerStartPoint().transform;
                localPlayerCharacter = Instantiate(_gamemode.defaultPawnClass, startpoint.position, startpoint.rotation)
                    .GetComponent<Pawn>();
            }
            else
            {
                localPlayerCharacter =
                    Instantiate(_gamemode.defaultPawnClass, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0))
                        .GetComponent<Pawn>();
            }

            localPlayerCharacter.isPossessed = _isLocalPlayer;
            localPlayerCharacter.name = localPlayerCharacter.name.Replace("(Clone)", "").Trim();
        }

        private static Transform GetPlayerStartPoint(bool _usePlayerStart = true)
        {
            if (!_usePlayerStart) return null;
            var allPossibleStartPoints = FindObjectsOfType<PlayerStart>();
            var allValidStartPoints = allPossibleStartPoints
                .Where(_possibleStartPoint => _possibleStartPoint.playerStartFilter == "").ToList();

            if (allValidStartPoints.Count == 0) return null;
            var random = Random.Range(0, allValidStartPoints.Count);
            return allValidStartPoints[random].transform;

        }

        public string GetCurrentGamemode()
        {
            return localPlayerCharacter.currentController.ToString();
        }

        public static void AddWidget(GameObject _widgetBlueprint)
        {
            var canvas = GameObject.FindWithTag("UserInterface");
            var newWidget = Instantiate(_widgetBlueprint, canvas.transform, false);
            newWidget.transform.localScale = new Vector3(1, 1, 1);

            // Remove (Clone) from the name since we reference the widgets by name 
            newWidget.name = newWidget.name.Replace("(Clone)", "").Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_widgetName">The name of the root object of the ui element</param>
        /// <returns></returns>
        public static GameObject GetWidget(string _widgetName)
        {
            var canvas = GameObject.FindWithTag("UserInterface");
            for (var i = 0; i < canvas.transform.childCount; i++)
            {
                var widget = canvas.transform.GetChild(i).gameObject;
                if (widget.name == _widgetName) return widget;
            }

            return null;
        }

        public void SetAllPawnsIsPaused(bool _isPaused, bool _saveWasPausedState = true)
        {
            foreach (var pawn in FindObjectsOfType<Pawn>())
            {
                if (_saveWasPausedState && !_isPaused)
                {
                    pawn.isPaused = pawn.wasPaused;
                }
                else
                {
                    pawn.wasPaused = pawn.isPaused;
                    pawn.isPaused = _isPaused;
                }
            }
        }


        //=-----------------=
        // User External Functions
        //=-----------------=
        // 0
        public void UI_ShowTitle()
        {
            AddWidget(UserInterfaceWidgets[0]);
        }

        // 1
        public void UI_ShowLoading()
        {
            AddWidget(UserInterfaceWidgets[1]);
        }

        // 2
        public void UI_ShowPause()
        {
            if (!GetWidget("WB_Pause"))
            {
                AddWidget(UserInterfaceWidgets[2]);
                SetAllPawnsIsPaused(true);
            }
            else
            {
                var _settingsWidget = GetWidget("WB_Settings");
                if (_settingsWidget)
                {
                    // TODO fix this cyclical reference
                    //_settingsWidget.GetComponent<WB_Settings>().RemoveSubwidgets();
                    Destroy(_settingsWidget);
                }

                Destroy(GetWidget("WB_Pause"));
                SetAllPawnsIsPaused(false);
            }
        }

        // 3
        public void UI_ShowLevelEditor()
        {
            AddWidget(UserInterfaceWidgets[3]);
        }

        // 4 (If you are wondering, this function requires a check for the existing menu due to it being re-created in the
        // testing mode in the level editor scene)
        public void UI_ShowHUD()
        {
            if (!GetWidget("WB_HUD"))
            {
                AddWidget(UserInterfaceWidgets[4]);
            }
            else
            {
                Destroy(GetWidget("WB_HUD"));
            }
        }

        // 5 
        public void UI_ShowInventory()
        {
            if (!GetWidget("WB_Inventory"))
            {
                AddWidget(UserInterfaceWidgets[5]);
                SetAllPawnsIsPaused(true);
            }
            else
            {
                Destroy(GetWidget("WB_Inventory"));
                SetAllPawnsIsPaused(false);
            }
        }

        // 6
        public void UI_ShowFramecounter()
        {
            AddWidget(UserInterfaceWidgets[6]);
        }

        // 7
        public void UI_ShowDeathScreen()
        {
            AddWidget(UserInterfaceWidgets[7]);
        }

        // 8
        public void UI_ShowFeedbackMenu(bool enabled)
        {
            if (enabled && !GetWidget("WB_FeedbackMenu"))
            {
                AddWidget(UserInterfaceWidgets[8]);
                SetAllPawnsIsPaused(true);
            }
            else if (!enabled)
            {
                Destroy(GetWidget("WB_FeedbackMenu"));
                SetAllPawnsIsPaused(false);
            }
        }

        //=----Reload Static Fields----=
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitializeStaticFields()
        {
            Instance = null;
        }
    }
}
