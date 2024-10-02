//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasForceGetActiveCamera : MonoBehaviour
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
    private Canvas canvas;
    private CameraManager cameraManager;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (canvas == null)
        {
            cameraManager = FindObjectOfType<CameraManager>(); 
            if (canvas == null)
            {
                Debug.LogWarning("Could not find " + nameof(CameraManager) + " to update canvas.worldCamera");
                return;
            }
        }

        canvas.worldCamera = cameraManager.GetActiveRenderingCamera();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
