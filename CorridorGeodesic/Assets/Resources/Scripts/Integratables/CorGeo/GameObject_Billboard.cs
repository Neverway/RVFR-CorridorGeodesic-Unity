//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Always make a gameobject face the camera 
// Notes: (usually used for 2D objects like sprites to make them appear 3D)
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObject_Billboard : MonoBehaviour
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
    private CameraManager cameraManager;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
        if (!cameraManager)
        {
            cameraManager = FindObjectOfType<CameraManager>();
            return;
        }

        if (cameraManager.GetActiveRenderingCamera())
        {
            transform.LookAt(cameraManager.GetActiveRenderingCamera().transform.position, cameraManager.GetActiveRenderingCamera().transform.up);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}