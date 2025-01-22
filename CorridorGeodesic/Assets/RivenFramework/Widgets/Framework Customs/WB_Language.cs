//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using Neverway.Framework.ApplicationManagement;

namespace Neverway.Framework
{
    public class WB_Language : MonoBehaviour
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
        private ApplicationSettings applicationSettings;
        [SerializeField] private Button buttonBack;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            applicationSettings = FindObjectOfType<ApplicationSettings>();
            if (buttonBack) buttonBack.onClick.AddListener(() => { Destroy(gameObject); });
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
        public void SetLocaleID(int _localeID)
        {
            applicationSettings.bufferedSettingsData.localeID = _localeID;
            applicationSettings.ApplySettings();
        }
    }
}