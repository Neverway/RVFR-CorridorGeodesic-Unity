//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Neverway.Framework;
using Neverway.Framework.ApplicationManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WB_FirstTimeSetup : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("A list of the screens to toggle for the setup menu")]
    [SerializeField] public GameObject[] setupScreens;
    [Tooltip("A list of game objects to toggle in correlation with the current screen")]
    [SerializeField] public GameObject[] screenObjects;
    [Header("Language")]
    [SerializeField] private Toggle dyslexicFriendlyFont;
    [Header("Graphics")]
    [SerializeField] public Button_Selector qualityPreset;
    [SerializeField] public Image qualityPreview;
    [SerializeField] public Sprite[] qualityPreviews;
    [Header("Audio")]
    [SerializeField] private Slider masterVolume;
    [Header("Brightness")]
    [SerializeField] public Slider brightness;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private int currentScreen = 0;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private ApplicationSettings applicationSettings;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        applicationSettings = FindObjectOfType<ApplicationSettings>();
        InitButtonValues();
        InitEventListeners();
    }

    private void Update()
    {
        for (int i = 0; i < setupScreens.Length; i++)
        {
            if (i == currentScreen)
            {
                setupScreens[i].SetActive(true);
                if (screenObjects[i]) screenObjects[i].SetActive(true);
            }
            else
            {
                setupScreens[i].SetActive(false);
                if (screenObjects[i]) screenObjects[i].SetActive(false);
            }
        }
        
        //applicationSettings.currentSettingsData.qualityPreset = qualityPreset.currentIndex;
        applicationSettings.ApplySettings();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator FinishFirstTimeSetup()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    private void InitButtonValues()
    {
        dyslexicFriendlyFont.isOn = applicationSettings.currentSettingsData.dyslexicFriendlyFont;
        qualityPreset.currentIndex = applicationSettings.currentSettingsData.qualityPreset;
        masterVolume.value = applicationSettings.currentSettingsData.masterVolume;
        brightness.value = applicationSettings.currentSettingsData.brightness * 100;
    }

    private void InitEventListeners()
    {
        dyslexicFriendlyFont.onValueChanged.AddListener(delegate
        {
            applicationSettings.currentSettingsData.dyslexicFriendlyFont = dyslexicFriendlyFont.isOn;
        });
        qualityPreset.onValueChanged.AddListener(delegate
        {
            qualityPreview.sprite = qualityPreviews[qualityPreset.currentIndex];
        });
        masterVolume.onValueChanged.AddListener(delegate
        {
            applicationSettings.currentSettingsData.masterVolume = Mathf.RoundToInt(masterVolume.value);
        });
        brightness.onValueChanged.AddListener(delegate
        {
            applicationSettings.currentSettingsData.brightness = brightness.value / 100;
        });
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void NextScreen()
    {
        applicationSettings.ApplySettings();
        currentScreen++;
        // We have reached the end of the first time setup, give the player a second to read the final message
        //  then move on to the next level (which is normally the title or splash screen)
        if (currentScreen == setupScreens.Length-1)
        {
            StartCoroutine(FinishFirstTimeSetup());
        }
    }
}
