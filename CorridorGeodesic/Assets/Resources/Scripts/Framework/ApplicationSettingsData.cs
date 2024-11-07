//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ApplicationSettingsData
{
    // Graphics
    // Display
    [Tooltip("X-Width, Y-Height")]
    public int targetResolution;
    [Tooltip("0-Fullscreen, 1-Fullscreen Windowed, 2-Windowed, 3-Windowed Maximized")]
    [Range(0, 3)] public int windowMode;
    [Tooltip("Vertical sync")]
    public bool enableVysnc;
    [Tooltip("Also referred to as target framerate")]
    [Range(-1, 300)] public int fpslimit;
    [Tooltip("Also referred to as fps counter")]
    public bool showFramecounter;
    
    // Quality
    [Tooltip("0-25%, 1-50%, 2-75%, 3-100%, 4-200%")]
    [Range(0, 4)] public int resolutionScale;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int shadowQuality;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int effectsQuality;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int textureQuality;

    [Tooltip("0-Lowest, 4-Highest")] [Range(0, 4)]
    public int postprocessingQuality;
    
    // Effects
    [Tooltip("0-Off")]
    [Range(0, 4)] public int antialiasing;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int motionBlur;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int ambientOcclusion;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int bloom;
    
    // Audio
    // Audio Devices
    [Tooltip("0-System Default")]
    public int outputDevice; 
    [Tooltip("0-System Default")]
    public int inputDevice;
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
    [Tooltip("0-Off, 1-Dialogue, 2-Voice Chat, 3-All")]
    [Range(0, 4)] public int closedCaptioning;
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
    public float colorBlindIntensity;
    [Range(0,3)] public int colorBlindFilter;
    public bool dyslexicFriendlyFont;
    public bool reduceStrobing;
    public bool screenReader;

    public int localeID;

    // Default constructor
    public ApplicationSettingsData()
    {
        targetResolution = 0;
        windowMode = 0;
        enableVysnc = false;
        fpslimit = 60;
        showFramecounter = false;

        resolutionScale = 3;
        shadowQuality = 2;
        effectsQuality = 2;
        textureQuality = 5;
        postprocessingQuality = 2;

        antialiasing = 0;
        motionBlur = 1;
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
        targetResolution = other.targetResolution;
        windowMode = other.windowMode;
        enableVysnc = other.enableVysnc;
        fpslimit = other.fpslimit;
        showFramecounter = other.showFramecounter;
        
        resolutionScale = other.resolutionScale;
        shadowQuality = other.shadowQuality;
        effectsQuality = other.effectsQuality;
        textureQuality = other.textureQuality;
        postprocessingQuality = other.postprocessingQuality;
        
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
        colorBlindIntensity = other.colorBlindIntensity;
        colorBlindFilter = other.colorBlindFilter;
        dyslexicFriendlyFont = other.dyslexicFriendlyFont;
        reduceStrobing = other.reduceStrobing;
        screenReader = other.screenReader;

        localeID = other.localeID;
    }
}
