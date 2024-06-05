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
    public Vector2 targetResolution = new Vector2(1920, 1080);
    [Tooltip("0-Fullscreen, 1-Fullscreen Windowed, 2-Windowed, 3-Windowed Maximized")]
    [Range(0, 3)] public int windowMode = 0;
    [Tooltip("Also referred to as target framerate")]
    [Range(-1, 300)] public int fpslimit = 60;
    [Tooltip("Also referred to as fps counter")]
    public bool showFramecounter;
    
    // Quality
    [Tooltip("0-25%, 1-50%, 2-75%, 3-100%, 4-200%")]
    [Range(0, 4)] public int resolutionScale = 3;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int shadowQuality;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int effectsQuality;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int textureQuality;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int postprocessingQuality;
    
    // Effects
    [Tooltip("0-Off")]
    [Range(0, 4)] public int antialiasing;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int motionBlur;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int ambientOcclusion;
    [Tooltip("0-Lowest, 4-Highest")]
    [Range(0, 4)] public int bloom;
}
