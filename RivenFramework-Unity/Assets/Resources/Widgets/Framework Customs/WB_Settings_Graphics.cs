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

public class WB_Settings_Graphics : MonoBehaviour
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
    // Display
    [SerializeField] private TMP_Dropdown targetResolution;
    [SerializeField] private Button_Selector windowMode;
    [SerializeField] private Slider fpslimit;
    [SerializeField] private Toggle showFramecounter;
    
    // Quality
    [SerializeField] public Button_Selector resolutionScale;
    [SerializeField] public Button_Selector shadowQuality;
    [SerializeField] public Button_Selector effectsQuality;
    [SerializeField] public Button_Selector textureQuality;
    [SerializeField] public Button_Selector postprocessingQuality;
    
    // Effects
    [SerializeField] public Button_Selector antialiasing;
    [SerializeField] public Button_Selector motionBlur;
    [SerializeField] public Button_Selector ambientOcclusion;
    [SerializeField] public Button_Selector bloom;


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
        applicationSettings.currentSettingsData.windowMode = windowMode.currentIndex;
        
        applicationSettings.currentSettingsData.resolutionScale = resolutionScale.currentIndex;
        applicationSettings.currentSettingsData.shadowQuality = shadowQuality.currentIndex;
        applicationSettings.currentSettingsData.effectsQuality = effectsQuality.currentIndex;
        applicationSettings.currentSettingsData.textureQuality = textureQuality.currentIndex;
        applicationSettings.currentSettingsData.postprocessingQuality = postprocessingQuality.currentIndex;
        
        applicationSettings.currentSettingsData.antialiasing = antialiasing.currentIndex;
        applicationSettings.currentSettingsData.motionBlur = motionBlur.currentIndex;
        applicationSettings.currentSettingsData.ambientOcclusion = ambientOcclusion.currentIndex;
        applicationSettings.currentSettingsData.bloom = bloom.currentIndex;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void InitButtonValues()
    {
        targetResolution.value = applicationSettings.currentSettingsData.targetResolution;
        windowMode.currentIndex = applicationSettings.currentSettingsData.windowMode;
        fpslimit.value = applicationSettings.currentSettingsData.fpslimit;
        showFramecounter.isOn = applicationSettings.currentSettingsData.showFramecounter;

        resolutionScale.currentIndex = applicationSettings.currentSettingsData.resolutionScale;
        shadowQuality.currentIndex = applicationSettings.currentSettingsData.shadowQuality;
        effectsQuality.currentIndex = applicationSettings.currentSettingsData.effectsQuality;
        textureQuality.currentIndex = applicationSettings.currentSettingsData.textureQuality;
        postprocessingQuality.currentIndex = applicationSettings.currentSettingsData.postprocessingQuality;
        
        antialiasing.currentIndex = applicationSettings.currentSettingsData.antialiasing;
        motionBlur.currentIndex = applicationSettings.currentSettingsData.motionBlur;
        ambientOcclusion.currentIndex = applicationSettings.currentSettingsData.ambientOcclusion;
        bloom.currentIndex = applicationSettings.currentSettingsData.bloom;
    }

    private void InitEventListeners()
    {
        targetResolution.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.targetResolution = targetResolution.value; });
        fpslimit.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.fpslimit = Mathf.RoundToInt(fpslimit.value); });
        showFramecounter.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.showFramecounter = showFramecounter.isOn; });
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
