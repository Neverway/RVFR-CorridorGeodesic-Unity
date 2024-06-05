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
        print(path);
        gameInstance = GetComponent<GameInstance>();
        if (File.Exists(path)) LoadSettings();
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
        var json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<ApplicationSettingsData>(json);
        currentSettingsData = data;
    }
    
    public void ApplySettings()
    {
        // Resolution
        Screen.SetResolution(Mathf.RoundToInt(currentSettingsData.targetResolution.x), Mathf.RoundToInt(currentSettingsData.targetResolution.x), Screen.fullScreen);
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
        // Effects Quality
        // Texture Quality
        
        // Anti-Aliasing
        // Motion Blur
        // Ambient Occlusion
        // Bloom
        
        SaveSettings();
    }
}
