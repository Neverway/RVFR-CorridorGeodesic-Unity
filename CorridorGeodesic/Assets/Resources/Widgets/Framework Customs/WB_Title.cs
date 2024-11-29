//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using Neverway.Framework.Cartographer;

namespace Neverway.Framework
{
    public class WB_Title : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public string targetLevelID;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private GameInstance gameInstance;
        private WorldLoader worldLoader;
        private LevelManager levelLoader; // Added for loading the overworld levels from Cartographer

        [SerializeField] private Button buttonMainGame,
            buttonExtras,
            buttonRanking,
            buttonSettings,
            buttonQuit,
            buttonCredits,
            buttonLanguage;

        [SerializeField] private GameObject extrasWidget, rankingWidget, settingsWidget, creditsWidget, languageWidget;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            gameInstance = FindObjectOfType<GameInstance>();
            worldLoader = FindObjectOfType<WorldLoader>();
            levelLoader = FindObjectOfType<LevelManager>();
            buttonMainGame.onClick.AddListener(delegate { OnClick("buttonMainGame"); });
            buttonExtras.onClick.AddListener(delegate { OnClick("buttonExtras"); });
            buttonRanking.onClick.AddListener(delegate { OnClick("buttonRanking"); });
            buttonSettings.onClick.AddListener(delegate { OnClick("buttonSettings"); });
            buttonQuit.onClick.AddListener(delegate { OnClick("buttonQuit"); });
            buttonCredits.onClick.AddListener(delegate { OnClick("buttonCredits"); });
            buttonLanguage.onClick.AddListener(delegate { OnClick("buttonLanguage"); });
        }

        private void Update()
        {

        }

        //=-----------------=
        // Internal Functions
        //=-----------------=
        private void OnClick(string button)
        {
            switch (button)
            {
                case "buttonMainGame":
                    if (!worldLoader) worldLoader = FindObjectOfType<WorldLoader>();
                    if (!levelLoader) levelLoader = FindObjectOfType<LevelManager>();
                    worldLoader.LoadWorld(targetLevelID);
                    // levelLoader.Load("", true); // Replace with function to load save file level
                    break;
                case "buttonExtras":
                    if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                    GameInstance.AddWidget(extrasWidget);
                    break;
                case "buttonRanking":
                    if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                    GameInstance.AddWidget(rankingWidget);
                    break;
                case "buttonSettings":
                    if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                    GameInstance.AddWidget(settingsWidget); // Create the settings widget
                    //Destroy(gameObject); // Remove the current widget
                    //GameInstance.GetWidget("WB_Settings").GetComponent<WB_Settings>().Init();
                    break;
                case "buttonQuit":
                    Application.Quit();
                    break;
                case "buttonCredits":
                    if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                    GameInstance.AddWidget(creditsWidget);
                    break;
                case "buttonLanguage":
                    if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                    GameInstance.AddWidget(languageWidget);
                    break;
            }
        }


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}