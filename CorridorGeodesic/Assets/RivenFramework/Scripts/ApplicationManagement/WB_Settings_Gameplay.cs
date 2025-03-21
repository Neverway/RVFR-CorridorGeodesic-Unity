//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Settings sub-widget for changing gameplay options
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using Neverway.Framework;

namespace Neverway.Framework.ApplicationManagement
{
    public class WB_Settings_Gameplay : MonoBehaviour
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

        // View
        [SerializeField] private Toggle invertHorizontalView;
        [SerializeField] private Toggle invertVerticalView;
        [SerializeField] private Slider horizontalLookSpeed;
        [SerializeField] private Slider verticalLookSpeed;
        [SerializeField] private Slider joystickLookSensitivity;
        [SerializeField] private Slider mouseLookSensitivity;

        [SerializeField] private Slider cameraFov;

        // Display
        [SerializeField] private Slider brightness;
        [SerializeField] private Toggle speedrunMode;
        [SerializeField] private Slider colorBlindIntensity;
        [SerializeField] private Button_Selector colorBlindFilter;
        [SerializeField] private Toggle dyslexicFriendlyFont;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            applicationSettings = FindObjectOfType<ApplicationSettings>();
            InitButtonValues();
            InitEventListeners(); // This is where the values of the applicationSettings are updated via the buttons and stuff :3
        }

        private void Update()
        {
            applicationSettings.bufferedSettingsData.colorBlindFilter = colorBlindFilter.currentIndex;
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=
        public void InitButtonValues()
        {
            applicationSettings.bufferedSettingsData = new ApplicationSettingsData(applicationSettings.currentSettingsData);
            invertHorizontalView.isOn = applicationSettings.bufferedSettingsData.invertHorizontalView;
            invertVerticalView.isOn = applicationSettings.bufferedSettingsData.invertVerticalView;
            horizontalLookSpeed.value = applicationSettings.bufferedSettingsData.horizontalLookSpeed * 10;
            verticalLookSpeed.value = applicationSettings.bufferedSettingsData.verticalLookSpeed * 10;
            joystickLookSensitivity.value = applicationSettings.bufferedSettingsData.joystickLookSensitivity * 10;
            mouseLookSensitivity.value = applicationSettings.bufferedSettingsData.mouseLookSensitivity * 10;
            cameraFov.value = applicationSettings.bufferedSettingsData.cameraFov;

            brightness.value = applicationSettings.bufferedSettingsData.brightness * 100;
            speedrunMode.isOn = applicationSettings.bufferedSettingsData.speedrunMode;
            colorBlindIntensity.value = applicationSettings.bufferedSettingsData.colorBlindIntensity * 100;
            dyslexicFriendlyFont.isOn = applicationSettings.bufferedSettingsData.dyslexicFriendlyFont;
            colorBlindFilter.currentIndex = applicationSettings.bufferedSettingsData.colorBlindFilter;
        }

        private void InitEventListeners()
        {
            invertHorizontalView.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.invertHorizontalView = invertHorizontalView.isOn;
            });
            invertVerticalView.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.invertVerticalView = invertVerticalView.isOn;
            });
            horizontalLookSpeed.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.horizontalLookSpeed = (horizontalLookSpeed.value / 10);
            });
            verticalLookSpeed.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.verticalLookSpeed = (verticalLookSpeed.value / 10);
            });
            joystickLookSensitivity.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.joystickLookSensitivity =
                    (joystickLookSensitivity.value / 10);
            });
            mouseLookSensitivity.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.mouseLookSensitivity = (mouseLookSensitivity.value / 10);
            });
            cameraFov.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.cameraFov = Mathf.RoundToInt(cameraFov.value);
            });

            brightness.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.brightness = brightness.value / 100;
            });
            speedrunMode.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.speedrunMode = speedrunMode.isOn;
            });
            colorBlindIntensity.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.colorBlindIntensity = (colorBlindIntensity.value / 100);
            });
            dyslexicFriendlyFont.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.dyslexicFriendlyFont = dyslexicFriendlyFont.isOn;
            });
        }


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}