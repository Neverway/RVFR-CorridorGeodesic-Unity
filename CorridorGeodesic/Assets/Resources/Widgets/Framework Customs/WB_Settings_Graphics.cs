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
using Neverway.Framework.ApplicationManagement;

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
    [SerializeField] private Toggle enableVsync;
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
        PopulateTargetResolutionDropdown();
        targetResolution.value = applicationSettings.currentSettingsData.targetResolution;
        windowMode.currentIndex = applicationSettings.currentSettingsData.windowMode;
        enableVsync.isOn = applicationSettings.currentSettingsData.enableVysnc;
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
    
    void PopulateTargetResolutionDropdown()
    {
        targetResolution.ClearOptions();
        //int currentResolutionIndex = 0;

        // Create a list to hold the resolution strings
        var options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < applicationSettings.resolutions.Length; i++)
        {
            var aspect = GetAspectRatio(applicationSettings.resolutions[i].width, applicationSettings.resolutions[i].height);
            // Don't allow not perfect aspect ratios
            /*if (aspect == "")
            {
                continue;
            }*/
            string resolution = $"{applicationSettings.resolutions[i].width}x{applicationSettings.resolutions[i].height} [{aspect}]";
            options.Add(new TMP_Dropdown.OptionData(resolution));

            // Check if this resolution is the current one
            if (applicationSettings.resolutions[i].width == Screen.width && applicationSettings.resolutions[i].height == Screen.height)
            {
                //currentResolutionIndex = i;
            }
        }

        // Set the dropdown options
        targetResolution.AddOptions(options);
        //targetResolution.value = currentResolutionIndex;
        targetResolution.RefreshShownValue();
    }
    
    string GetAspectRatio(int width, int height)
    {
        float aspectRatio = (float)width / height;

        if (Mathf.Approximately(aspectRatio, 16f / 9f))
        {
            return "16:9";
        }
        if (Mathf.Approximately(aspectRatio, 4f / 3f))
        {
            return "4:3";
        }
        if (Mathf.Approximately(aspectRatio, 21f / 9f))
        {
            return "21:9";
        }
        if (Mathf.Approximately(aspectRatio, 16f / 10f))
        {
            return "16:10";
        }
        if (Mathf.Approximately(aspectRatio, 3f / 2f))
        {
            return "3:2";
        }
        // Return an empty string for non-perfect aspect ratios
        return "";
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
