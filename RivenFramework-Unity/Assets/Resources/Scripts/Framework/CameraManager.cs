//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public List<Camera> cameras;


    //=-----------------=
    // Private Variables
    //=-----------------=


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
        cameras.Clear();
        foreach (var camera in FindObjectsOfType<Camera>())
        {
            cameras.Add(camera);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public Camera GetActiveRenderingCamera()
    {
        Camera[] cameras = Camera.allCameras;
        foreach (Camera cam in cameras)
        {
            if (cam.isActiveAndEnabled && cam.targetTexture == null)
            {
                return cam;
            }
        }
        return null;
    }
}
