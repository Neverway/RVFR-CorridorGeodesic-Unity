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
    // Used to delay the appearance of the first time setup screen until after the game instance has decided that we are actually going to need it
    private bool initialized; 


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
        StartCoroutine(WaitForGameInstance());
    }

    private void Update()
    {
        if (!initialized) return;
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
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator WaitForGameInstance()
    {
        yield return new WaitForSeconds(0.5f);
        initialized = true;
    }
    
    private IEnumerator FinishFirstTimeSetup()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        applicationSettings.ApplySettings();
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
            applicationSettings.bufferedSettingsData.dyslexicFriendlyFont = dyslexicFriendlyFont.isOn;
            applicationSettings.ApplySettings();
        });
        qualityPreset.onValueChanged.AddListener(delegate
        {
            applicationSettings.bufferedSettingsData.qualityPreset = qualityPreset.currentIndex;
            qualityPreview.sprite = qualityPreviews[qualityPreset.currentIndex];
            applicationSettings.SetQualityPreset(qualityPreset.currentIndex);
            applicationSettings.ApplySettings();
        });
        masterVolume.onValueChanged.AddListener(delegate
        {
            applicationSettings.bufferedSettingsData.masterVolume = Mathf.RoundToInt(masterVolume.value);
            applicationSettings.ApplySettings();
        });
        brightness.onValueChanged.AddListener(delegate
        {
            applicationSettings.bufferedSettingsData.brightness = brightness.value / 100;
            applicationSettings.ApplySettings();
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
