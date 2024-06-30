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
    [SerializeField] private Slider cameraFov;
    // Display
    //[SerializeField] private Slider colorBlindIntensity;
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
        applicationSettings.currentSettingsData.colorBlindFilter = colorBlindFilter.currentIndex;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void InitButtonValues()
    {
        cameraFov.value = applicationSettings.currentSettingsData.cameraFov;
        dyslexicFriendlyFont.isOn = applicationSettings.currentSettingsData.dyslexicFriendlyFont;
        colorBlindFilter.currentIndex = applicationSettings.currentSettingsData.colorBlindFilter;
    }

    private void InitEventListeners()
    {        
        cameraFov.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.cameraFov = Mathf.RoundToInt(cameraFov.value); });
        dyslexicFriendlyFont.onValueChanged.AddListener(delegate { applicationSettings.currentSettingsData.dyslexicFriendlyFont = dyslexicFriendlyFont.isOn; });
    }    


    //=-----------------=
    // External Functions
    //=-----------------=
}
