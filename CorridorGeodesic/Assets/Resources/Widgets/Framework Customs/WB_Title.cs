//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Button buttonMainGame, buttonExtras, buttonRanking, buttonSettings, buttonQuit, buttonCredits;
    [SerializeField] private GameObject extrasWidget, rankingWidget, settingsWidget, creditsWidget;


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
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
