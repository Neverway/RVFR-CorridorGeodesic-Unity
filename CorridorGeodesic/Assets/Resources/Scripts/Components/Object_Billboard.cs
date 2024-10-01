//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Always make a gameObject face the camera 
// Notes: (usually used for 2D objects like sprites to make them appear 3D)
//
//=============================================================================

using UnityEngine;

/// <summary>
/// Always make a gameObject face the camera 
/// </summary>
public class Object_Billboard : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("How many seconds between updating this object to face the camera")]
    [SerializeField] private float updateRate = 0.25f;


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
        InvokeRepeating(nameof(UpdateBillboard), 0, updateRate);
    }

    private void UpdateBillboard()
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