//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        applicationSettings.currentSettingsData.closedCaptioning = closedCaptioning.currentIndex;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void InitButtonValues()
    {
        outputDevice.value = applicationSettings.currentSettingsData.outputDevice;
        inputDevice.value = applicationSettings.currentSettingsData.inputDevice;
        inputVolume.value = applicationSettings.currentSettingsData.inputVolume;
        
        masterVolume.value = applicationSettings.currentSettingsData.masterVolume;
        musicVolume.value = applicationSettings.currentSettingsData.musicVolume;
        soundVolume.value = applicationSettings.currentSettingsData.soundVolume;
        voiceVolume.value = applicationSettings.currentSettingsData.voiceVolume;
        chatterVolume.value = applicationSettings.currentSettingsData.chatterVolume;
        ambientVolume.value = applicationSettings.currentSettingsData.ambientVolume;
        menuVolume.value = applicationSettings.currentSettingsData.menuVolume;
        
        visualizeSoundEffects.isOn = applicationSettings.currentSettingsData.visualizeSoundEffects;
        closedCaptioning.currentIndex = applicationSettings.currentSettingsData.closedCaptioning;
        minVolume.value = applicationSettings.currentSettingsData.minVolume;
        maxVolume.value = applicationSettings.currentSettingsData.maxVolume;
        minFrequency.value = applicationSettings.currentSettingsData.minFrequency;
    }

    private void InitEventListeners()
    {
        outputDevice.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.outputDevice = outputDevice.value; });
        inputDevice.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.inputDevice = inputDevice.value; });
        inputVolume.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.inputVolume = Mathf.RoundToInt(inputVolume.value); });
        
        masterVolume.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.masterVolume = Mathf.RoundToInt(masterVolume.value); });
        musicVolume.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.musicVolume = Mathf.RoundToInt(musicVolume.value); });
        soundVolume.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.soundVolume = Mathf.RoundToInt(soundVolume.value); });
        voiceVolume.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.voiceVolume = Mathf.RoundToInt(voiceVolume.value); });
        chatterVolume.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.chatterVolume = Mathf.RoundToInt(chatterVolume.value); });
        ambientVolume.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.ambientVolume = Mathf.RoundToInt(ambientVolume.value); });
        menuVolume.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.menuVolume = Mathf.RoundToInt(menuVolume.value); });
        
        visualizeSoundEffects.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.visualizeSoundEffects = visualizeSoundEffects.isOn; });
        minVolume.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.minVolume = Mathf.RoundToInt(minVolume.value); });
        maxVolume.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.maxVolume = Mathf.RoundToInt(maxVolume.value); });
        minFrequency.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.minFrequency = Mathf.RoundToInt(minFrequency.value); });
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
