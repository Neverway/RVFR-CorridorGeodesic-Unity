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


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;
    private WorldLoader worldLoader;
    private LevelLoader levelLoader; // Added for loading the overworld levels from Cartographer
    [SerializeField] private Button Bttn_MainGame, Bttn_Extras, Bttn_Ranking, Bttn_Settings, Bttn_Quit;
    [SerializeField] private GameObject UI_Extras, UI_Ranking, UI_Settings;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        worldLoader = FindObjectOfType<WorldLoader>();
        levelLoader = FindObjectOfType<LevelLoader>();
        Bttn_MainGame.onClick.AddListener(delegate { OnClick("MainGame"); });
        Bttn_Extras.onClick.AddListener(delegate { OnClick("Extras"); });
        Bttn_Ranking.onClick.AddListener(delegate { OnClick("Ranking"); });
        Bttn_Settings.onClick.AddListener(delegate { OnClick("Settings"); });
        Bttn_Quit.onClick.AddListener(delegate { OnClick("Quit"); });
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
            case "MainGame":
                if (!worldLoader) worldLoader = FindObjectOfType<WorldLoader>();
                if (!levelLoader) levelLoader = FindObjectOfType<LevelLoader>();
                worldLoader.Load("Overworld");
                // levelLoader.Load("", true); // Replace with function to load save file level
                break;
            case "Extras":
                if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                GameInstance.AddWidget(UI_Extras);
                break;
            case "Ranking":
                if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                GameInstance.AddWidget(UI_Ranking);
                break;
            case "Settings":
                if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                GameInstance.AddWidget(UI_Settings);
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
