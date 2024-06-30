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


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        canvas.worldCamera = FindObjectOfType<CameraManager>().GetActiveRenderingCamera();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
