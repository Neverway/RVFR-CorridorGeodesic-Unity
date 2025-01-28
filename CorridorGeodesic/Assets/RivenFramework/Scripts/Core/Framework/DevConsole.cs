//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework
{
public class DevConsole
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public static void Log(string _message, string _header = "", bool _enableUnityLogging = true)
    {
        if (_enableUnityLogging) Debug.Log($"[{_header}] {_message}");
    }
    public static void LogWarning(string _message, string _header = "", bool _enableUnityLogging = true)
    {
        if (_enableUnityLogging) Debug.LogWarning($"[{_header}] {_message}");
    }
    public static void LogError(string _message, string _header = "", bool _enableUnityLogging = true)
    {
        if (_enableUnityLogging) Debug.LogError($"[{_header}] {_message}");
    }
    public static void LogSuccess(string _message, string _header = "", bool _enableUnityLogging = true)
    {
        if (_enableUnityLogging) Debug.Log($"[{_header}] {_message}");
    }
}
}
