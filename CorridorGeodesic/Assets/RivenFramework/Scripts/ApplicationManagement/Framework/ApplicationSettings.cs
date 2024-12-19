//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Sets and loads the parameters for the application settings from a file
// Notes: This is only for the application settings like graphics, audio, gameplay, etc.
//  This file should not be used to store any game-specific information
//
//=============================================================================

using System.Collections;
using FMODUnity;
using System.IO;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.PostProcessing;
using Neverway.Framework.PawnManagement;
using UnityEngine.ResourceManagement;


namespace Neverway.Framework.ApplicationManagement
{
    public class ApplicationSettings : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [ReadOnly] [SerializeField] private ApplicationSettingsData defaultSettingsData;
        public ApplicationSettingsData currentSettingsData;
        public Resolution[] resolutions;


        //=-----------------=
        // Private Variables
        //=-----------------=
        public bool debugApply;
        private string path;


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
            path = $"{UnityEngine.Application.persistentDataPath}/settings.json";
            gameInstance = GetComponent<GameInstance>();

            masterBus = RuntimeManager.GetBus("bus:/Master");
            sfxBus = RuntimeManager.GetBus("bus:/Master/SFX");
            musicBus = RuntimeManager.GetBus("bus:/Master/Music");
            musicBus = RuntimeManager.GetBus("bus:/Master/Ambience");
            voicesBus = RuntimeManager.GetBus("bus:/Master/Voices");

            LoadSettings();
            GetCurrentResolutionFromList();
            ApplySettings();
            InvokeRepeating(nameof(CheckFPSCounterVisibility), 0, 1);
            InvokeRepeating(nameof(CheckDynamicTextureFiltering), 0, 1);
        }

        private void Update()
        {
            if (debugApply)
            {
                debugApply = false;
                ApplySettings();
            }
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=
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
            //switch (currentSettingsData.textureQuality)
            //{
            //    case 0:
            //        foreach (var texture in Resources.LoadAll<Texture>("Materials/Textures/DynamicallyFiltered"))
            //        {
            //            texture.filterMode = FilterMode.Point;
            //        }
            //        break;
            //    case 1:
            //        foreach (var texture in Resources.LoadAll<Texture>("Materials/Textures/DynamicallyFiltered"))
            //        {
            //            texture.filterMode = FilterMode.Point;
            //        }
            //        break;
            //    case 2:
            //        foreach (var texture in Resources.LoadAll<Texture>("Materials/Textures/DynamicallyFiltered"))
            //        {
            //            texture.filterMode = FilterMode.Bilinear;
            //        }
            //        break;
            //    case 3:
            //        foreach (var texture in Resources.LoadAll<Texture>("Materials/Textures/DynamicallyFiltered"))
            //        {
            //            texture.filterMode = FilterMode.Bilinear;
            //        }
            //        break;
            //    case 4:
            //        foreach (var texture in Resources.LoadAll<Texture>("Materials/Textures/DynamicallyFiltered"))
            //        {
            //            texture.filterMode = FilterMode.Trilinear;
            //        }
            //        break;
            //}
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
            currentSettingsData = new ApplicationSettingsData(defaultSettingsData);
            ApplySettings();
        }

        public void SaveSettings()
        {
            var json = JsonUtility.ToJson(currentSettingsData, true);
            File.WriteAllText(path, json); // Save JSON data to file
        }

        public void LoadSettings()
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<ApplicationSettingsData>(json);
                currentSettingsData = data;
            }
            else
            {
                currentSettingsData = new ApplicationSettingsData(defaultSettingsData);
            }
        }

        public void ApplySettings()
        {
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
                UnityEngine.Application.targetFrameRate = currentSettingsData.fpslimit;
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
            switch (currentSettingsData.resolutionScale)
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
            switch (currentSettingsData.shadowQuality)
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
            switch (currentSettingsData.effectsQuality)
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
            switch (currentSettingsData.textureQuality)
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

        FullScreenMode GetFullscreenMode()
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

        void GetCurrentResolutionFromList()
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