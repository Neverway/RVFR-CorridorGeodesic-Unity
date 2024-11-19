//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Disables cameras on unpossessed pawns
// Notes: This is depreciated, now there is only one "view camera" and the new 
//  camera manager just assigns it to follow a handle on the possessed pawn
//
//=============================================================================

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Neverway.Framework
{
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
            InvokeRepeating(nameof(UpdateCameras), 0, 0.5f);
        }

        private void UpdateCameras()
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
            foreach (Camera cam in cameras)
            {
                if (cam.IsUnityNull())
                {
                    cameras.Remove(cam);
                    return null;
                }

                if (cam.isActiveAndEnabled && cam.targetTexture == null)
                {
                    return cam;
                }
            }

            return null;
        }
    }
}