//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using Neverway.Framework.PawnManagement;

namespace Neverway.Framework.ApplicationManagement
{
    public class WB_Settings : MonoBehaviour
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
        private ApplicationSettings applicationSettings;

        [SerializeField] private Button buttonBack,
            buttonApply,
            buttonReset,
            buttonGraphics,
            buttonAudio,
            buttonControls,
            buttonGameplay;

        [SerializeField] private GameObject graphicsWidget, audioWidget, controlsWidget, gameplayWidget;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            gameInstance = FindObjectOfType<GameInstance>();
            applicationSettings = FindObjectOfType<ApplicationSettings>();
            applicationSettings.LoadSettings();
            buttonBack.onClick.AddListener(delegate { OnClick("buttonBack"); });
            buttonApply.onClick.AddListener(delegate { OnClick("buttonApply"); });
            buttonReset.onClick.AddListener(delegate { OnClick("buttonReset"); });
            buttonGraphics.onClick.AddListener(delegate { OnClick("buttonGraphics"); });
            buttonAudio.onClick.AddListener(delegate { OnClick("buttonAudio"); });
            buttonControls.onClick.AddListener(delegate { OnClick("buttonControls"); });
            buttonGameplay.onClick.AddListener(delegate { OnClick("buttonGameplay"); });
            Init();
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=
        private void OnClick(string button)
        {
            switch (button)
            {
                case "buttonBack":
                    //if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                    //gameInstance.UI_ShowTitle();
                    RemoveSubwidgets();
                    Destroy(gameObject);
                    break;
                case "buttonApply":
                    applicationSettings.ApplySettings();
                    break;
                case "buttonReset":
                    applicationSettings.ResetSettings();
                    break;
                case "buttonGraphics":
                    if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                    RemoveSubwidgets();
                    GameInstance.AddWidget(graphicsWidget);
                    break;
                case "buttonAudio":
                    if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                    RemoveSubwidgets();
                    GameInstance.AddWidget(audioWidget);
                    break;
                case "buttonControls":
                    if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                    RemoveSubwidgets();
                    GameInstance.AddWidget(controlsWidget);
                    break;
                case "buttonGameplay":
                    if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                    RemoveSubwidgets();
                    GameInstance.AddWidget(gameplayWidget);
                    break;
            }
        }


        //=-----------------=
        // External Functions
        //=-----------------=
        [Tooltip("Call this to add the first setting sub-widget")]
        public void Init()
        {
            RemoveSubwidgets();
            GameInstance.AddWidget(graphicsWidget);
        }

        public void RemoveSubwidgets()
        {
            Destroy(GameInstance.GetWidget("WB_Settings_Graphics"));
            Destroy(GameInstance.GetWidget("WB_Settings_Audio"));
            Destroy(GameInstance.GetWidget("WB_Settings_Controls"));
            Destroy(GameInstance.GetWidget("WB_Settings_Gameplay"));
        }
    }
}