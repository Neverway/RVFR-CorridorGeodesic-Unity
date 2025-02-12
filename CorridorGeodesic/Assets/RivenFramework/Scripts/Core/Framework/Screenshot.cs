//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Capture the current view in-game and save it as a png 
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework
{
public class Screenshot : MonoBehaviour
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

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(TakeScreenShot());
        }
    }

    IEnumerator TakeScreenShot()
    {
        yield return new WaitForEndOfFrame();
        var file = $"{Application.persistentDataPath}/{Application.identifier}-{Application.version}-{System.DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)")}.png";
        DevConsole.Log($"Screenshot saved '{file}'", "CAM");
        ScreenCapture.CaptureScreenshot(file);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
}
