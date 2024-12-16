//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using Neverway.Framework;

public class Pawn_Turret_Aim : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private GameObject turretMount;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float degreesPerSecond = 70f;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    private CameraManager cameraManager;
    public float forwardness;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start ()
    {
        cameraManager = FindObjectOfType<CameraManager> ();
    }

    private void Update ()
    {
        if (!cameraManager)
        {
            cameraManager = FindObjectOfType<CameraManager> ();
            return;
        }

        Camera cam = cameraManager.GetActiveRenderingCamera ();
        if (cam == null)
        {
            return;
        }

        forwardness = Vector3.Dot (turretMount.transform.forward, cam.transform.position - transform.position);

        if (forwardness > 0)
        {
            aimTarget.LookAt (cam.transform.position, turretMount.transform.forward);
        }

        else
        {
            aimTarget.LookAt (turretMount.transform.position + turretMount.transform.forward, turretMount.transform.forward);
        }

        float diff = Quaternion.Angle(aimTarget.rotation, transform.rotation);

        float t = (degreesPerSecond*Time.deltaTime) / diff;
        if (t > 1) t = 1;

        transform.rotation = Quaternion.Lerp (transform.rotation, aimTarget.rotation, t);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}