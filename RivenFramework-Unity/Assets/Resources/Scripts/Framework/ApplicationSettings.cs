//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class ApplicationSettings : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public ApplicationSettingsData defaultSettingsData;
    public ApplicationSettingsData currentSettingsData;


    //=-----------------=
    // Private Variables
    //=-----------------=
    public bool debugApply;
    private string path;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        path = $"{Application.persistentDataPath}/settings.json";
        gameInstance = GetComponent<GameInstance>();
        LoadSettings();
        ApplySettings();
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


    //=-----------------=
    // External Functions
    //=-----------------=
    public void ResetSettings()
    {
        currentSettingsData = defaultSettingsData;
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
            currentSettingsData = defaultSettingsData;
        }
    }
    
    public void ApplySettings()
    {
        // Resolution
        //Screen.SetResolution(Mathf.RoundToInt(currentSettingsData.targetResolution.x), Mathf.RoundToInt(currentSettingsData.targetResolution.x), Screen.fullScreen);
        // Window Mode
        switch (currentSettingsData.windowMode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 3:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }
        // Vsync
        switch (currentSettingsData.enableVysnc)
        {
            case true:
                QualitySettings.vSyncCount = 1;
                break;
            case false:
                QualitySettings.vSyncCount = 0;
                break;
        }
        // FPS limit
        Application.targetFrameRate = currentSettingsData.fpslimit;
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
                ScalableBufferManager.ResizeBuffers(2f, 2f);
                break;
        }
        // Shadow Quality
        switch (currentSettingsData.shadowQuality)
        {
            case 0:
                QualitySettings.shadows = ShadowQuality.Disable; // Real-time shadows (off)
                QualitySettings.shadowResolution = ShadowResolution.Low;
                QualitySettings.shadowCascades = 0;
                QualitySettings.shadowDistance = 10;
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
                QualitySettings.globalTextureMipmapLimit = 3;
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                break;
            case 1:
                QualitySettings.globalTextureMipmapLimit = 3;
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
                QualitySettings.antiAliasing = 8;
                break;
        }
        // Motion Blur
        switch (currentSettingsData.motionBlur)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
        // Ambient Occlusion
        switch (currentSettingsData.ambientOcclusion)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
        // Bloom
        switch (currentSettingsData.bloom)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
        
        SaveSettings();
    }
}
