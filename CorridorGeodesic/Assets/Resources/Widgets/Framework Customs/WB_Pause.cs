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
using UnityEngine.UI;
using Neverway.Framework;

public class WB_Pause : MonoBehaviour
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
    [SerializeField] private Button buttonResume, buttonSettings, buttonTitle, buttonQuit, buttonRestart;
    [SerializeField] private GameObject settingsWidget;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        worldLoader = FindObjectOfType<WorldLoader>();
        buttonResume.onClick.AddListener(delegate { OnClick("buttonResume"); });
        buttonSettings.onClick.AddListener(delegate { OnClick("buttonSettings"); });
        buttonTitle.onClick.AddListener(delegate { OnClick("buttonTitle"); });
        buttonQuit.onClick.AddListener(delegate { OnClick("buttonQuit"); });
        buttonRestart.onClick.AddListener(delegate { OnClick("buttonRestart"); });
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void OnClick(string _button)
    {
        switch (_button)
        {
            case "buttonResume":
                if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                gameInstance.UI_ShowPause();
                break;
            case "buttonSettings":
                if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                GameInstance.AddWidget(settingsWidget);
                GameInstance.GetWidget("WB_Settings").GetComponent<WB_Settings>().Init();
                break;
            case "buttonTitle":
                worldLoader.LoadWorld("_Title");
                break;
            case "buttonQuit":
                Application.Quit();
                break;
            case "buttonRestart":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
