//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Get the active view camera and assign it to a world-space canvas
// Notes: This is depreciated, now there is only one "view camera" and the new 
//  camera manager just assigns it to follow a handle on the possessed pawn
//
//=============================================================================

namespace Neverway.Framework
{/*
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
            if (cameraManager == null)
            {
                cameraManager = FindObjectOfType<CameraManager>();
                if (cameraManager == null)
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
    }*/
}