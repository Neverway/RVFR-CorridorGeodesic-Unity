//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Neverway.Framework.ApplicationManagement;

namespace Neverway.Framework
{
    public class WB_Settings_Audio : MonoBehaviour
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

        // Audio Devices
        [SerializeField] private TMP_Dropdown outputDevice;
        [SerializeField] private TMP_Dropdown inputDevice;
        [SerializeField] private Slider inputVolume;
        [SerializeField] private Slider inputPreview;

        // Audio Mixer
        [SerializeField] private Slider masterVolume;
        [SerializeField] private Slider musicVolume;
        [SerializeField] private Slider soundVolume;
        [SerializeField] private Slider voiceVolume;
        [SerializeField] private Slider chatterVolume;
        [SerializeField] private Slider ambientVolume;
        [SerializeField] private Slider menuVolume;

        // Audio Accessibility
        [SerializeField] private Toggle visualizeSoundEffects;
        [SerializeField] public Button_Selector closedCaptioning;
        [SerializeField] private Slider minVolume;
        [SerializeField] private Slider maxVolume;
        [SerializeField] private Slider minFrequency;


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
            applicationSettings.bufferedSettingsData.closedCaptioning = closedCaptioning.currentIndex;
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=
        public void InitButtonValues()
        {
            applicationSettings.bufferedSettingsData = new ApplicationSettingsData(applicationSettings.currentSettingsData);
            outputDevice.value = applicationSettings.bufferedSettingsData.outputDevice;
            inputDevice.value = applicationSettings.bufferedSettingsData.inputDevice;
            inputVolume.value = applicationSettings.bufferedSettingsData.inputVolume;

            masterVolume.value = applicationSettings.bufferedSettingsData.masterVolume;
            musicVolume.value = applicationSettings.bufferedSettingsData.musicVolume;
            soundVolume.value = applicationSettings.bufferedSettingsData.soundVolume;
            voiceVolume.value = applicationSettings.bufferedSettingsData.voiceVolume;
            chatterVolume.value = applicationSettings.bufferedSettingsData.chatterVolume;
            ambientVolume.value = applicationSettings.bufferedSettingsData.ambientVolume;
            menuVolume.value = applicationSettings.bufferedSettingsData.menuVolume;

            visualizeSoundEffects.isOn = applicationSettings.bufferedSettingsData.visualizeSoundEffects;
            closedCaptioning.currentIndex = applicationSettings.bufferedSettingsData.closedCaptioning;
            minVolume.value = applicationSettings.bufferedSettingsData.minVolume;
            maxVolume.value = applicationSettings.bufferedSettingsData.maxVolume;
            minFrequency.value = applicationSettings.bufferedSettingsData.minFrequency;
        }

        private void InitEventListeners()
        {
            outputDevice.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.outputDevice = outputDevice.value;
            });
            inputDevice.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.inputDevice = inputDevice.value;
            });
            inputVolume.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.inputVolume = Mathf.RoundToInt(inputVolume.value);
            });

            masterVolume.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.masterVolume = Mathf.RoundToInt(masterVolume.value);
            });
            musicVolume.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.musicVolume = Mathf.RoundToInt(musicVolume.value);
            });
            soundVolume.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.soundVolume = Mathf.RoundToInt(soundVolume.value);
            });
            voiceVolume.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.voiceVolume = Mathf.RoundToInt(voiceVolume.value);
            });
            chatterVolume.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.chatterVolume = Mathf.RoundToInt(chatterVolume.value);
            });
            ambientVolume.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.ambientVolume = Mathf.RoundToInt(ambientVolume.value);
            });
            menuVolume.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.menuVolume = Mathf.RoundToInt(menuVolume.value);
            });

            visualizeSoundEffects.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.visualizeSoundEffects = visualizeSoundEffects.isOn;
            });
            minVolume.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.minVolume = Mathf.RoundToInt(minVolume.value);
            });
            maxVolume.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.maxVolume = Mathf.RoundToInt(maxVolume.value);
            });
            minFrequency.onValueChanged.AddListener(delegate
            {
                applicationSettings.bufferedSettingsData.minFrequency = Mathf.RoundToInt(minFrequency.value);
            });
        }


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}