//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAim : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private GameObject turretMount;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private CameraManager cameraManager;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start ()
    {
        cameraManager = FindObjectOfType<CameraManager> ();
    }

    private void Update()
    {
        if (!cameraManager)
        {
            cameraManager = FindObjectOfType<CameraManager> ();
            return;
        }

        if (cameraManager.GetActiveRenderingCamera ())
        {
            transform.LookAt (cameraManager.GetActiveRenderingCamera ().transform.position, turretMount.transform.forward);
        }

    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}