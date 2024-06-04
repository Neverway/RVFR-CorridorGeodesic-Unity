//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class ApplicationSettings : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Vector2 targetResolution = new Vector2(1920, 1080);
    [Range(0, 3)] public int windowMode = 0;
    [Range(-1, 300)] public int fpslimit = 60;
    
    [Range(0, 4)] public int resolutionScale = 3;


    //=-----------------=
    // Private Variables
    //=-----------------=
    public bool debugApply;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
         
    }

    private void Update()
    {
        if (debugApply)
        {
            debugApply = false;
            ApplyGraphicSettings();
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void ApplyGraphicSettings()
    {
        // Resolution
        Screen.SetResolution(Mathf.RoundToInt(targetResolution.x), Mathf.RoundToInt(targetResolution.x), Screen.fullScreen);
        // Window Mode
        switch (windowMode)
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
        Application.targetFrameRate = fpslimit;
        
        // Resolution Scale
        switch (resolutionScale)
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
    }
}
