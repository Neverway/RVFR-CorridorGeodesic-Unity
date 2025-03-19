//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Data structure class for what's stored in the application settings file
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neverway.Framework.ApplicationManagement
{
    [Serializable]
    public class ApplicationSettingsData
    {
        // File
        [Tooltip("Change this value when you want to force players not to use an older config")]
        public int configurationFileCompatibilityVersion;
        
        // Graphics
        // Display
        [Tooltip("X-Width, Y-Height")] 
        public int targetResolution;

        [Tooltip("0-Fullscreen, 1-Fullscreen Windowed, 2-Windowed, 3-Windowed Maximized")] [Range(0, 3)]
        public int windowMode;

        [Tooltip("Vertical sync")] public bool enableVysnc;

        [Tooltip("Also referred to as target framerate")] [Range(-1, 300)]
        public int fpsLimit;

        [Tooltip("Also referred to as fps counter")]
        public bool showFramecounter;

        // Quality
        [Range(0, 4)]
        public int qualityPreset;
        public ApplicationSettingsData_Quality quality;

        // Effects
        [Tooltip("0-Off")] [Range(0, 4)] public int antialiasing;

        [Tooltip("0-Lowest, 4-Highest")] [Range(0, 4)]
        public int motionBlur;

        [Tooltip("0-Lowest, 4-Highest")] [Range(0, 4)]
        public int ambientOcclusion;

        [Tooltip("0-Lowest, 4-Highest")] [Range(0, 4)]
        public int bloom;

        // Audio
        // Audio Devices
        [Tooltip("0-System Default")] public int outputDevice;
        [Tooltip("0-System Default")] public int inputDevice;
        [Range(0, 100)] public int inputVolume;

        // Audio Mixer
        [Range(0, 100)] public int masterVolume;
        [Range(0, 100)] public int musicVolume;
        [Range(0, 100)] public int soundVolume;
        [Range(0, 100)] public int voiceVolume;
        [Range(0, 100)] public int chatterVolume;
        [Range(0, 100)] public int ambientVolume;
        [Range(0, 100)] public int menuVolume;

        // Audio Accessibility
        public bool visualizeSoundEffects;

        [Tooltip("0-Off, 1-Dialogue, 2-Voice Chat, 3-All")] [Range(0, 4)]
        public int closedCaptioning;

        public int minVolume;
        public int maxVolume;
        public int minFrequency;

        // Gameplay
        // View
        public bool invertHorizontalView;
        public bool invertVerticalView;
        public float horizontalLookSpeed;
        public float verticalLookSpeed;
        public float joystickLookSensitivity;
        public float mouseLookSensitivity;
        public int cameraFov;

        // Communication
        public bool hideTextChat;
        public bool enablePushToTalk;

        // General Accessibility
        public float brightness;
        public bool speedrunMode;
        public float colorBlindIntensity;
        [Range(0, 3)] public int colorBlindFilter;
        public bool dyslexicFriendlyFont;
        public bool reduceStrobing;
        public bool screenReader;

        public int localeID;

        // Default constructor
        public ApplicationSettingsData()
        {
            configurationFileCompatibilityVersion = 0;
            
            targetResolution = 0;
            windowMode = 0;
            enableVysnc = false;
            fpsLimit = 60;
            showFramecounter = false;

            qualityPreset = 3;
            quality = new ApplicationSettingsData_Quality();

            antialiasing = 0;
            motionBlur = 0;
            ambientOcclusion = 2;
            bloom = 2;

            outputDevice = 0;
            inputDevice = 0;
            inputVolume = 100;

            masterVolume = 100;
            musicVolume = 100;
            soundVolume = 100;
            voiceVolume = 100;
            chatterVolume = 100;
            ambientVolume = 100;
            menuVolume = 100;

            visualizeSoundEffects = false;
            closedCaptioning = 0;
            minVolume = 0;
            maxVolume = 140;
            minFrequency = 17;

            invertHorizontalView = false;
            invertVerticalView = false;
            horizontalLookSpeed = 1;
            verticalLookSpeed = 0.75f;
            joystickLookSensitivity = 0.7f;
            mouseLookSensitivity = 0.7f;
            cameraFov = 90;

            hideTextChat = false;
            enablePushToTalk = false;

            brightness = 1;
            speedrunMode = false;
            colorBlindIntensity = 1;
            colorBlindFilter = 0;
            dyslexicFriendlyFont = false;
            reduceStrobing = false;
            screenReader = false;

            localeID = 0;
        }

        // Clone constructor
        public ApplicationSettingsData(ApplicationSettingsData other)
        {
            configurationFileCompatibilityVersion = other.configurationFileCompatibilityVersion;
            
            targetResolution = other.targetResolution;
            windowMode = other.windowMode;
            enableVysnc = other.enableVysnc;
            fpsLimit = other.fpsLimit;
            showFramecounter = other.showFramecounter;

            qualityPreset = other.qualityPreset;
            quality = other.quality;

            antialiasing = other.antialiasing;
            motionBlur = other.motionBlur;
            ambientOcclusion = other.ambientOcclusion;
            bloom = other.bloom;

            outputDevice = other.outputDevice;
            inputDevice = other.inputDevice;
            inputVolume = other.inputVolume;

            masterVolume = other.masterVolume;
            musicVolume = other.musicVolume;
            soundVolume = other.soundVolume;
            voiceVolume = other.voiceVolume;
            chatterVolume = other.chatterVolume;
            ambientVolume = other.ambientVolume;
            menuVolume = other.menuVolume;

            visualizeSoundEffects = other.visualizeSoundEffects;
            closedCaptioning = other.closedCaptioning;
            minVolume = other.minVolume;
            maxVolume = other.maxVolume;
            minFrequency = other.minFrequency;

            invertHorizontalView = other.invertHorizontalView;
            invertVerticalView = other.invertVerticalView;
            horizontalLookSpeed = other.horizontalLookSpeed;
            verticalLookSpeed = other.verticalLookSpeed;
            joystickLookSensitivity = other.joystickLookSensitivity;
            mouseLookSensitivity = other.mouseLookSensitivity;
            cameraFov = other.cameraFov;

            hideTextChat = other.hideTextChat;
            enablePushToTalk = other.enablePushToTalk;

            brightness = other.brightness;
            speedrunMode = other.speedrunMode;
            colorBlindIntensity = other.colorBlindIntensity;
            colorBlindFilter = other.colorBlindFilter;
            dyslexicFriendlyFont = other.dyslexicFriendlyFont;
            reduceStrobing = other.reduceStrobing;
            screenReader = other.screenReader;

            localeID = other.localeID;
        }
    }

    [Serializable]
    public class ApplicationSettingsData_Quality
    {
        [Tooltip("0-25%, 1-50%, 2-75%, 3-100%, 4-200%")] [Range(0, 4)]
        public int resolutionScale;

        [Tooltip("0-Lowest, 4-Highest")] [Range(0, 4)]
        public int shadowQuality;

        [Tooltip("0-Lowest, 4-Highest")] [Range(0, 4)]
        public int effectsQuality;

        [Tooltip("0-Lowest, 4-Highest")] [Range(0, 4)]
        public int textureQuality;

        [Tooltip("0-Lowest, 4-Highest")] [Range(0, 4)]
        public int postprocessingQuality;
        
        // Default Constructor
        public ApplicationSettingsData_Quality()
        {
            resolutionScale = 3;
            shadowQuality = 2;
            effectsQuality = 2;
            textureQuality = 4;
            postprocessingQuality = 2;
        }
        
        // Clone Constructor
        public ApplicationSettingsData_Quality(ApplicationSettingsData_Quality other)
        {
            resolutionScale = other.resolutionScale;
            shadowQuality = other.shadowQuality;
            effectsQuality = other.effectsQuality;
            textureQuality = other.textureQuality;
            postprocessingQuality = other.postprocessingQuality;
        }
    }
}

