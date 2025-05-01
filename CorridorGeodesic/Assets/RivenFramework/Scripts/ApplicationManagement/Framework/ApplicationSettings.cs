//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Sets and loads the parameters for the application settings from a file
// Notes: This is only for the application settings like graphics, audio, gameplay, etc.
//  This file should not be used to store any game-specific information
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.PostProcessing;
using Neverway.Framework.PawnManagement;
using UnityEngine.SceneManagement;


namespace Neverway.Framework.ApplicationManagement
{
    public class ApplicationSettings : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [Tooltip(
            "If you have changed the application data structure, update this number so that the game knows to make a new config file for the new version")]
        public int configVersion = 1;
        [Tooltip("The default values for the settings (pulled from the constructor in ApplicationSettingsData, overridden here)")]
        [SerializeField] private ApplicationSettingsData defaultSettingsData;
        public ApplicationSettingsData_Quality retroQuality, lowQuality, mediumQuality, highQuality, fantasticQuality;
        [Tooltip("The current values for the settings")]
        public ApplicationSettingsData currentSettingsData;
        [Tooltip("The unapplied values for the settings, current settings gets set to these values right before applying")]
        public ApplicationSettingsData bufferedSettingsData;

        [Tooltip("A list of which folders contain textures that are affected by the dynamic texture filters")]
        [SerializeField]
        private List<string> dynamicallyFilteredTexturePaths =
            new List<string> { "Materials/Textures/DynamicallyFiltered" };

        public bool debugForceEnableFirstTimeSetup;


        //=-----------------=
        // Private Variables
        //=-----------------=
        private string configurationFilePath;
        public Resolution[] resolutions;


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private GameInstance gameInstance;
        public AudioMixer audioMixer;
        public GameObject cameraPrefab;
        public PostProcessProfile postProcessProfile;

        private FMOD.Studio.Bus masterBus;
        private FMOD.Studio.Bus sfxBus;
        private FMOD.Studio.Bus musicBus;
        private FMOD.Studio.Bus ambienceBus;
        private FMOD.Studio.Bus voicesBus;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            // Get the default file path to save the application settings config
            configurationFilePath = $"{UnityEngine.Application.persistentDataPath}/settings.json";
            InitializeReferenceVariables();
            InitialConfigurationSetup();
            
            // Todo: this seems like a silly fix for updating the fps visibility and texture filtering ~Liz
            InvokeRepeating(nameof(CheckFPSCounterVisibility), 0, 1);
            InvokeRepeating(nameof(CheckDynamicTextureFiltering), 0, 1);
        }
        

        //=-----------------=
        // Internal Functions
        //=-----------------=
        private void InitializeReferenceVariables()
        {
            gameInstance = GetComponent<GameInstance>();

            // Pretty sure this is Soulex's FMOD stuff (RAH I HATE FMOD RAH!!!! ~Liz)
            masterBus = RuntimeManager.GetBus("bus:/Master");
            sfxBus = RuntimeManager.GetBus("bus:/Master/SFX");
            musicBus = RuntimeManager.GetBus("bus:/Master/Music");
            musicBus = RuntimeManager.GetBus("bus:/Master/Ambience");
            voicesBus = RuntimeManager.GetBus("bus:/Master/Voices");
        }

        private void InitialConfigurationSetup()
        {
            DevConsole.Log("Initializing application settings...", "App Config");
            // If we have a config file...
            if (File.Exists(configurationFilePath) && !debugForceEnableFirstTimeSetup)
            {
                var json = File.ReadAllText(configurationFilePath);
                var data = JsonUtility.FromJson<ApplicationSettingsData>(json);
                // Ensure the config version has not changed...
                if (data.configurationFileCompatibilityVersion == configVersion)
                {
                    // Everything checks out, let's apply those settings!
                    bufferedSettingsData = data;
                    GetCurrentResolutionFromList();
                    ApplySettings();
                    
                    // If we are on the firstTimeSetup scene...
                    if (SceneManager.GetActiveScene().buildIndex == 0)
                    {
                        // Skip to the next scene
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                    DevConsole.LogSuccess("Successfully loaded config file", "App Config");
                    return;
                }
            }
            
            // Force create a new empty configuration file
            bufferedSettingsData = new ApplicationSettingsData(defaultSettingsData);
            bufferedSettingsData.configurationFileCompatibilityVersion = configVersion;
            GetCurrentResolutionFromList();
            ApplySettings();
            DevConsole.Log("No valid config file found, default config created", "App Config");
            
            // The config was not valid...
            // If we are not on the firstTimeSetup scene...
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                return;
            }
            DevConsole.Log("Continuing with first-time setup", "App Config");
            // Set the resolution to the highest supported by the display
            bufferedSettingsData.targetResolution = resolutions.Length-1;
            DevConsole.Log($"Setting the display resolution to {resolutions[resolutions.Length-1].width}x{resolutions[resolutions.Length-1].height}", "App Config");
            ApplySettings();
        }

        private void GetCurrentResolutionFromList()
        {
            resolutions = Screen.resolutions;
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                // Check if this resolution is the current one
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            // Set the dropdown options
            currentSettingsData.targetResolution = currentResolutionIndex;
        }
        
        // Todo: give this function a better name ~Liz
        private void UpdateActiveWindowButtons()
        {
            if (FindObjectOfType<WB_Settings_Graphics>())
            {
                FindObjectOfType<WB_Settings_Graphics>().InitButtonValues();
            }

            if (FindObjectOfType<WB_Settings_Audio>())
            {
                FindObjectOfType<WB_Settings_Audio>().InitButtonValues();
            }

            if (FindObjectOfType<WB_Settings_Gameplay>())
            {
                FindObjectOfType<WB_Settings_Gameplay>().InitButtonValues();
            }
        }

        private void CheckFPSCounterVisibility()
        {
            switch (currentSettingsData.showFramecounter)
            {
                case true:
                    if (!GameInstance.GetWidget("WB_Framecounter"))
                    {
                        gameInstance.UI_ShowFramecounter();
                    }

                    break;
                case false:
                    if (GameInstance.GetWidget("WB_Framecounter"))
                    {
                        Destroy(GameInstance.GetWidget("WB_Framecounter"));
                    }

                    break;
            }
        }

        private void CheckDynamicTextureFiltering()
        {
            foreach (var texturePath in dynamicallyFilteredTexturePaths)
            {
                switch (currentSettingsData.quality.textureQuality)
                {
                    case 0:
                        foreach (var texture in Resources.LoadAll<Texture>(texturePath))
                        {
                            texture.filterMode = FilterMode.Point;
                        }
                        break;
                    case 1:
                        foreach (var texture in Resources.LoadAll<Texture>(texturePath))
                        {
                            texture.filterMode = FilterMode.Point;
                        }
                        break;
                    case 2:
                        foreach (var texture in Resources.LoadAll<Texture>(texturePath))
                        {
                            texture.filterMode = FilterMode.Bilinear;
                        }
                        break;
                    case 3:
                        foreach (var texture in Resources.LoadAll<Texture>(texturePath))
                        {
                            texture.filterMode = FilterMode.Bilinear;
                        }
                        break;
                    case 4:
                        foreach (var texture in Resources.LoadAll<Texture>(texturePath))
                        {
                            texture.filterMode = FilterMode.Trilinear;
                        }
                        break;
                }
            }
        }

        private IEnumerator SetLocalization(int _localeID)
        {
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        }


        //=-----------------=
        // External Functions
        //=-----------------=
        public void ResetSettings()
        {
            bufferedSettingsData = new ApplicationSettingsData(defaultSettingsData);
            ApplySettings();
        }

        public void SaveSettings()
        {
            var json = JsonUtility.ToJson(currentSettingsData, true);
            File.WriteAllText(configurationFilePath, json); // Save JSON data to file
        }

        /*private void LoadSettings()
        {
            if (File.Exists(configurationFilePath))
            {
                var json = File.ReadAllText(configurationFilePath);
                var data = JsonUtility.FromJson<ApplicationSettingsData>(json);
                // Ensure the configuration version has not changed
                if (data.configurationFileCompatibilityVersion == defaultSettingsData.configurationFileCompatibilityVersion)
                {
                    currentSettingsData = data;
                }
                // Config was not valid, need to generate a new one
                else
                {
                    currentSettingsData = new ApplicationSettingsData(defaultSettingsData);
                }
            }
            // Config was not valid, need to generate a new one
            else
            {
                currentSettingsData = new ApplicationSettingsData(defaultSettingsData);
            }
        }*/

        public void ApplySettings()
        {
            currentSettingsData = new ApplicationSettingsData(bufferedSettingsData);
            
            // Resolution
            Screen.SetResolution(resolutions[currentSettingsData.targetResolution].width,
                resolutions[currentSettingsData.targetResolution].height, GetFullscreenMode());
            // Vsync
            switch (currentSettingsData.enableVysnc)
            {
                case true:
                    QualitySettings.vSyncCount = 2;
                    break;
                case false:
                    QualitySettings.vSyncCount = 0;
                    break;
            }

            // FPS limit
            if (!currentSettingsData.enableVysnc)
            {
                UnityEngine.Application.targetFrameRate = currentSettingsData.fpsLimit;
            }
            else
            {
                UnityEngine.Application.targetFrameRate = 0;
            }

            // Framecounter
            switch (currentSettingsData.showFramecounter)
            {
                case true:
                    if (!GameInstance.GetWidget("WB_Framecounter"))
                    {
                        gameInstance.UI_ShowFramecounter();
                    }

                    break;
                case false:
                    if (GameInstance.GetWidget("WB_Framecounter"))
                    {
                        Destroy(GameInstance.GetWidget("WB_Framecounter"));
                    }

                    break;
            }

            // Resolution Scale
            switch (currentSettingsData.quality.resolutionScale)
            {
                case 0:
                    ScalableBufferManager.ResizeBuffers(0.25f, 0.25f);
                    break;
                case 1:
                    ScalableBufferManager.ResizeBuffers(0.5f, 0.5f);
                    break;
                case 2:
                    ScalableBufferManager.ResizeBuffers(0.75f, 0.75f);
                    break;
                case 3:
                    ScalableBufferManager.ResizeBuffers(1f, 1f);
                    break;
                case 4:
                    ScalableBufferManager.ResizeBuffers(1f, 1f);
                    break;
            }

            // Shadow Quality
            switch (currentSettingsData.quality.shadowQuality)
            {
                case 0:
                    QualitySettings.shadows = ShadowQuality.Disable; // Real-time shadows (off)
                    QualitySettings.shadowResolution = ShadowResolution.Low;
                    QualitySettings.shadowCascades = 0;
                    QualitySettings.shadowDistance = 5;
                    break;
                case 1:
                    QualitySettings.shadows = ShadowQuality.HardOnly; // Real-time shadows (hard-shadows only)
                    QualitySettings.shadowResolution = ShadowResolution.Low;
                    QualitySettings.shadowCascades = 2;
                    QualitySettings.shadowDistance = 15;
                    break;
                case 2:
                    QualitySettings.shadows = ShadowQuality.All; // Real-time shadows (all)
                    QualitySettings.shadowResolution = ShadowResolution.Medium;
                    QualitySettings.shadowCascades = 2;
                    QualitySettings.shadowDistance = 20;
                    break;
                case 3:
                    QualitySettings.shadows = ShadowQuality.All; // Real-time shadows (all)
                    QualitySettings.shadowResolution = ShadowResolution.High;
                    QualitySettings.shadowCascades = 4;
                    QualitySettings.shadowDistance = 40;
                    break;
                case 4:
                    QualitySettings.shadows = ShadowQuality.All; // Real-time shadows (all)
                    QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                    QualitySettings.shadowCascades = 4;
                    QualitySettings.shadowDistance = 150;
                    break;
            }

            // Effects Quality
            switch (currentSettingsData.quality.effectsQuality)
            {
                case 0:
                    QualitySettings.softParticles = false;
                    QualitySettings.particleRaycastBudget = 4;
                    break;
                case 1:
                    QualitySettings.softParticles = false;
                    QualitySettings.particleRaycastBudget = 16;
                    break;
                case 2:
                    QualitySettings.softParticles = true;
                    QualitySettings.particleRaycastBudget = 64;
                    break;
                case 3:
                    QualitySettings.softParticles = true;
                    QualitySettings.particleRaycastBudget = 256;
                    break;
                case 4:
                    QualitySettings.softParticles = true;
                    QualitySettings.particleRaycastBudget = 4096;
                    break;
            }

            // Texture Quality
            switch (currentSettingsData.quality.textureQuality)
            {
                case 0:
                    QualitySettings.globalTextureMipmapLimit = 12;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    break;
                case 1:
                    QualitySettings.globalTextureMipmapLimit = 4;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    break;
                case 2:
                    QualitySettings.globalTextureMipmapLimit = 2;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    break;
                case 3:
                    QualitySettings.globalTextureMipmapLimit = 1;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    break;
                case 4:
                    QualitySettings.globalTextureMipmapLimit = 0;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    break;
            }


            // Anti-Aliasing
            switch (currentSettingsData.antialiasing)
            {
                case 0:
                    QualitySettings.antiAliasing = 0;
                    break;
                case 1:
                    QualitySettings.antiAliasing = 2;
                    break;
                case 2:
                    QualitySettings.antiAliasing = 4;
                    break;
                case 3:
                    QualitySettings.antiAliasing = 8;
                    break;
                case 4:
                    QualitySettings.antiAliasing = 0;
                    cameraPrefab.GetComponent<PostProcessLayer>().antialiasingMode =
                        PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                    // Apply to any active cameras (that support it)
                    foreach (var camera in FindObjectsOfType<Camera>())
                    {
                        if (camera.GetComponent<PostProcessLayer>())
                            camera.GetComponent<PostProcessLayer>().antialiasingMode =
                                PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                    }

                    break;
                case 5:
                    QualitySettings.antiAliasing = 0;
                    cameraPrefab.GetComponent<PostProcessLayer>().antialiasingMode =
                        PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                    // Apply to any active cameras (that support it)
                    foreach (var camera in FindObjectsOfType<Camera>())
                    {
                        if (camera.GetComponent<PostProcessLayer>())
                            camera.GetComponent<PostProcessLayer>().antialiasingMode =
                                PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                    }

                    break;
                case 6:
                    QualitySettings.antiAliasing = 0;
                    cameraPrefab.GetComponent<PostProcessLayer>().antialiasingMode =
                        PostProcessLayer.Antialiasing.TemporalAntialiasing;
                    // Apply to any active cameras (that support it)
                    foreach (var camera in FindObjectsOfType<Camera>())
                    {
                        if (camera.GetComponent<PostProcessLayer>())
                            camera.GetComponent<PostProcessLayer>().antialiasingMode =
                                PostProcessLayer.Antialiasing.TemporalAntialiasing;
                    }

                    break;
            }

            Bloom bloom = postProcessProfile.GetSetting<Bloom>();
            MotionBlur motionBlur = postProcessProfile.GetSetting<MotionBlur>();
            AmbientOcclusion ambientOcclusion = postProcessProfile.GetSetting<AmbientOcclusion>();
            ColorGrading colorGrading = postProcessProfile.GetSetting<ColorGrading>();

            // Motion Blur
            switch (currentSettingsData.motionBlur)
            {
                case 0:
                    motionBlur.active = false;
                    break;
                case 1:
                    motionBlur.active = true;
                    motionBlur.shutterAngle.value = 100f;
                    break;
                case 2:
                    motionBlur.active = true;
                    motionBlur.shutterAngle.value = 170f;
                    break;
                case 3:
                    motionBlur.active = true;
                    motionBlur.shutterAngle.value = 200f;
                    break;
                case 4:
                    motionBlur.active = true;
                    motionBlur.shutterAngle.value = 270f;
                    break;
            }

            // Ambient Occlusion
            switch (currentSettingsData.ambientOcclusion)
            {
                case 0:
                    ambientOcclusion.active = false;
                    break;
                case 1:
                    ambientOcclusion.active = true;
                    ambientOcclusion.intensity.value = 0.25f;
                    break;
                case 2:
                    ambientOcclusion.active = true;
                    ambientOcclusion.intensity.value = 0.5f;
                    break;
                case 3:
                    ambientOcclusion.active = true;
                    ambientOcclusion.intensity.value = 0.75f;
                    break;
                case 4:
                    ambientOcclusion.active = true;
                    ambientOcclusion.intensity.value = 1f;
                    break;
            }

            // Bloom
            switch (currentSettingsData.bloom)
            {
                case 0:
                    bloom.active = false;
                    break;
                case 1:
                    bloom.active = true;
                    bloom.intensity.value = 0.25f;
                    break;
                case 2:
                    bloom.active = true;
                    bloom.intensity.value = 0.5f;
                    break;
                case 3:
                    bloom.active = true;
                    bloom.intensity.value = 0.75f;
                    break;
                case 4:
                    bloom.active = true;
                    bloom.intensity.value = 1.5f;
                    break;
            }

            // AUDIO SETTINGS
            audioMixer.SetFloat("inputMicrophone", ConvertVolumeToPercentage(currentSettingsData.masterVolume));
            audioMixer.SetFloat("master", ConvertVolumeToPercentage(currentSettingsData.masterVolume));
            audioMixer.SetFloat("music", ConvertVolumeToPercentage(currentSettingsData.musicVolume));
            audioMixer.SetFloat("soundEffects", ConvertVolumeToPercentage(currentSettingsData.soundVolume));
            audioMixer.SetFloat("voiceChat", ConvertVolumeToPercentage(currentSettingsData.voiceVolume));
            audioMixer.SetFloat("characterChatter", ConvertVolumeToPercentage(currentSettingsData.chatterVolume));
            audioMixer.SetFloat("ambient", ConvertVolumeToPercentage(currentSettingsData.ambientVolume));
            audioMixer.SetFloat("menus", ConvertVolumeToPercentage(currentSettingsData.menuVolume));

            masterBus.setVolume(LinearVolumeFromSliderValue(currentSettingsData.masterVolume));
            sfxBus.setVolume(LinearVolumeFromSliderValue(currentSettingsData.soundVolume));
            ambienceBus.setVolume(LinearVolumeFromSliderValue(currentSettingsData.ambientVolume));
            musicBus.setVolume(LinearVolumeFromSliderValue(currentSettingsData.musicVolume));
            voicesBus.setVolume(LinearVolumeFromSliderValue(currentSettingsData.voiceVolume));

            // GAMEPLAY SETTINGS
            // Camera FOV
            cameraPrefab.GetComponent<Camera>().fieldOfView = currentSettingsData.cameraFov;
            // Apply to any active cameras
            foreach (var camera in FindObjectsOfType<Camera>())
            {
                if (camera.GetComponent<Camera_NoChangeFOV>()) return;
                camera.fieldOfView = currentSettingsData.cameraFov;
            }

            postProcessProfile.GetSetting<AutoExposure>().keyValue.value = currentSettingsData.brightness;

            // ColorBlind Filter
            var colorBlindIntensityValue =
                currentSettingsData
                    .colorBlindIntensity; // @Liz I SUCK AT MATH AND EVERY EQUATION I TRIED CAUSES NEGATIVE COLOR VALUES (That's bad!)
            switch (currentSettingsData.colorBlindFilter
                   )
            {
                case 0:
                    colorGrading.active = true;
                    colorGrading.mixerRedOutRedIn.value = 100;
                    colorGrading.mixerGreenOutRedIn.value = 0;
                    colorGrading.mixerBlueOutRedIn.value = 0;

                    colorGrading.mixerRedOutGreenIn.value = 0;
                    colorGrading.mixerGreenOutGreenIn.value = 100;
                    colorGrading.mixerBlueOutGreenIn.value = 0;

                    colorGrading.mixerBlueOutRedIn.value = 0;
                    colorGrading.mixerGreenOutBlueIn.value = 0;
                    colorGrading.mixerBlueOutBlueIn.value = 100;
                    break;
                case 1:
                    // Protanopia
                    colorGrading.active = true;
                    colorGrading.mixerRedOutRedIn.value =
                        56 * colorBlindIntensityValue;
                    colorGrading.mixerGreenOutRedIn.value =
                        44 * colorBlindIntensityValue;
                    colorGrading.mixerBlueOutRedIn.value =
                        0 * colorBlindIntensityValue;

                    colorGrading.mixerRedOutGreenIn.value =
                        55 * colorBlindIntensityValue;
                    colorGrading.mixerGreenOutGreenIn.value =
                        45 * colorBlindIntensityValue;
                    colorGrading.mixerBlueOutGreenIn.value =
                        0 * colorBlindIntensityValue;

                    colorGrading.mixerBlueOutRedIn.value =
                        0 * colorBlindIntensityValue;
                    colorGrading.mixerGreenOutBlueIn.value =
                        24 * colorBlindIntensityValue;
                    colorGrading.mixerBlueOutBlueIn.value =
                        76 * colorBlindIntensityValue;
                    break;
                case 2:
                    // Deuteranopia
                    colorGrading.active = true;
                    colorGrading.mixerRedOutRedIn.value =
                        80 * colorBlindIntensityValue;
                    colorGrading.mixerGreenOutRedIn.value =
                        20 * colorBlindIntensityValue;
                    colorGrading.mixerBlueOutRedIn.value =
                        0 * colorBlindIntensityValue;

                    colorGrading.mixerRedOutGreenIn.value =
                        25 * colorBlindIntensityValue;
                    colorGrading.mixerGreenOutGreenIn.value =
                        75 * colorBlindIntensityValue;
                    colorGrading.mixerBlueOutGreenIn.value =
                        0 * colorBlindIntensityValue;

                    colorGrading.mixerBlueOutRedIn.value =
                        0 * colorBlindIntensityValue;
                    colorGrading.mixerGreenOutBlueIn.value =
                        14 * colorBlindIntensityValue;
                    colorGrading.mixerBlueOutBlueIn.value =
                        86 * colorBlindIntensityValue;
                    break;
                case 3:
                    // Tritanopia
                    colorGrading.active = true;
                    colorGrading.mixerRedOutRedIn.value =
                        95 * colorBlindIntensityValue;
                    colorGrading.mixerGreenOutRedIn.value =
                        5 * colorBlindIntensityValue;
                    colorGrading.mixerBlueOutRedIn.value =
                        0 * colorBlindIntensityValue;

                    colorGrading.mixerRedOutGreenIn.value =
                        0 * colorBlindIntensityValue;
                    colorGrading.mixerGreenOutGreenIn.value =
                        43 * colorBlindIntensityValue;
                    colorGrading.mixerBlueOutGreenIn.value =
                        57 * colorBlindIntensityValue;

                    colorGrading.mixerBlueOutRedIn.value =
                        0 * colorBlindIntensityValue;
                    colorGrading.mixerGreenOutBlueIn.value =
                        47 * colorBlindIntensityValue;
                    colorGrading.mixerBlueOutBlueIn.value =
                        53 * colorBlindIntensityValue;
                    break;
            }

            // Dyslexia Assist
            if (currentSettingsData.dyslexicFriendlyFont)
            {
                GetComponent<ApplicationFontSetter>().currentFont = 1;
            }
            else
            {
                GetComponent<ApplicationFontSetter>().currentFont = 0;
            }

            // Language
            StartCoroutine(SetLocalization(currentSettingsData.localeID));



            SaveSettings();
            UpdateActiveWindowButtons();
        }

        public void SetQualityPreset(int _qualityPreset)
        {
            switch (_qualityPreset)
            {
                case 0:
                    bufferedSettingsData.quality.resolutionScale = retroQuality.resolutionScale;
                    bufferedSettingsData.quality.shadowQuality = retroQuality.shadowQuality;
                    bufferedSettingsData.quality.effectsQuality = retroQuality.effectsQuality;
                    bufferedSettingsData.quality.textureQuality = retroQuality.textureQuality;
                    bufferedSettingsData.quality.postprocessingQuality = retroQuality.postprocessingQuality;
                    break;
                case 1:
                    bufferedSettingsData.quality.resolutionScale = lowQuality.resolutionScale;
                    bufferedSettingsData.quality.shadowQuality = lowQuality.shadowQuality;
                    bufferedSettingsData.quality.effectsQuality = lowQuality.effectsQuality;
                    bufferedSettingsData.quality.textureQuality = lowQuality.textureQuality;
                    bufferedSettingsData.quality.postprocessingQuality = lowQuality.postprocessingQuality;
                    break;
                case 2:
                    bufferedSettingsData.quality.resolutionScale = mediumQuality.resolutionScale;
                    bufferedSettingsData.quality.shadowQuality = mediumQuality.shadowQuality;
                    bufferedSettingsData.quality.effectsQuality = mediumQuality.effectsQuality;
                    bufferedSettingsData.quality.textureQuality = mediumQuality.textureQuality;
                    bufferedSettingsData.quality.postprocessingQuality = mediumQuality.postprocessingQuality;
                    break;
                case 3:
                    bufferedSettingsData.quality.resolutionScale = highQuality.resolutionScale;
                    bufferedSettingsData.quality.shadowQuality = highQuality.shadowQuality;
                    bufferedSettingsData.quality.effectsQuality = highQuality.effectsQuality;
                    bufferedSettingsData.quality.textureQuality = highQuality.textureQuality;
                    bufferedSettingsData.quality.postprocessingQuality = highQuality.postprocessingQuality;
                    break;
                case 4:
                    bufferedSettingsData.quality.resolutionScale = fantasticQuality.resolutionScale;
                    bufferedSettingsData.quality.shadowQuality = fantasticQuality.shadowQuality;
                    bufferedSettingsData.quality.effectsQuality = fantasticQuality.effectsQuality;
                    bufferedSettingsData.quality.textureQuality = fantasticQuality.textureQuality;
                    bufferedSettingsData.quality.postprocessingQuality = fantasticQuality.postprocessingQuality;
                    break;
            }
        }

        public FullScreenMode GetFullscreenMode()
        {
            // Window Mode
            switch (currentSettingsData.windowMode)
            {
                case 0:
                    return FullScreenMode.ExclusiveFullScreen;
                case 1:
                    return FullScreenMode.FullScreenWindow;
                case 2:
                    return FullScreenMode.Windowed;
                case 3:
                    return FullScreenMode.MaximizedWindow;
                default:
                    return FullScreenMode.ExclusiveFullScreen;
            }
        }

        public static float ConvertVolumeToPercentage(int _volumeSliderValue)
        {
            // Calculate the mapped value using the formula: mappedValue = -80 + 0.8 * volumeSliderValue
            float mappedValue = -80 + 0.8f * _volumeSliderValue;

            // Ensure the mapped value stays within the range of -80 to 0
            if (mappedValue < -80)
            {
                mappedValue = -80;
            }
            else if (mappedValue > 0)
            {
                mappedValue = 0;
            }

            return mappedValue;
        }

        public static float LinearVolumeFromSliderValue(int sliderValue)
        {
            float normalizedValue = sliderValue / 100f;
            return normalizedValue;
            //float adjustedValue = Mathf.Log10(normalizedValue) * 20;

            //return sliderValue == 0 ? -80 : adjustedValue;
        }
    }
}