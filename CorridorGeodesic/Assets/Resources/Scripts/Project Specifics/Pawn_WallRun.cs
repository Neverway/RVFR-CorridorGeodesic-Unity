//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: This is based off of the tutorial series by Plai
// Source: https://www.youtube.com/watch?v=LqnPeqoJRFY
//
//=============================================================================


using System;
using UnityEngine;
using Neverway.Framework.ApplicationManagement;

public class Pawn_WallRun : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private Transform orientation;

    [Header("Wall Running")] 
    [SerializeField] private bool allowWallRun;
    [SerializeField] private float wallDistance = 0.6f;
    [SerializeField] private float minimumJumpHeight = 1.5f;
    [SerializeField] private float wallRunGravity = 5;
    [SerializeField] private float wallJumpForce = 5;

    [Header("Camera")] 
    [SerializeField] private float wallRunAdditionalFov = 20;
    [SerializeField] private float wallRunFovTime = 20;
    [SerializeField] private float cameraTilt;
    [SerializeField] private float cameraTiltTime;

    public float tilt { get; private set; }


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool wallLeft, wallRight; 
    private RaycastHit leftWallHit, rightWallHit;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Rigidbody objectRigidbody;
    private InputActions.FirstPersonShooterActions fpsActions;
    private ApplicationSettings applicationSettings;
    [SerializeField] private Camera viewCamera;
    [SerializeField] private LayerMask layerMask;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        fpsActions = new InputActions().FirstPersonShooter;
        fpsActions.Enable();
        
        applicationSettings = FindObjectOfType<ApplicationSettings>();
    }

    private void Update()
    {
        // Wall kick
        if (fpsActions.Jump.WasPressedThisFrame() && CanWallRun())
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                objectRigidbody.velocity = new Vector3(objectRigidbody.velocity.x, objectRigidbody.velocity.y);
                objectRigidbody.AddForce(wallRunJumpDirection * (wallJumpForce * 10), ForceMode.Impulse);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                objectRigidbody.velocity = new Vector3(objectRigidbody.velocity.x, objectRigidbody.velocity.y);
                objectRigidbody.AddForce(wallRunJumpDirection * (wallJumpForce * 10), ForceMode.Impulse);
            }
        }
    }

    private void FixedUpdate()
    {
        CheckWall();
        if (CanWallRun())
        {
            if (wallLeft)
            {
                StartWallRun();
            }
            else if (wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight, layerMask);
    }
    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance, layerMask);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance, layerMask);
    }

    private void StartWallRun()
    {
        if (allowWallRun)
        {
            objectRigidbody.useGravity = false;
            objectRigidbody.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
        }
        
        // Camera effects
        viewCamera.fieldOfView = Mathf.Lerp(viewCamera.fieldOfView,
            applicationSettings.currentSettingsData.cameraFov + wallRunAdditionalFov, wallRunFovTime * Time.deltaTime);

        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -cameraTilt, cameraTiltTime * Time.deltaTime);
        }
        if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, cameraTilt, cameraTiltTime * Time.deltaTime);
        }
    }

    private void StopWallRun()
    {
        if (allowWallRun)
        {
            objectRigidbody.useGravity = enabled;
        }
        
        // Camera effects
        viewCamera.fieldOfView = Mathf.Lerp(viewCamera.fieldOfView,
            applicationSettings.currentSettingsData.cameraFov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, cameraTiltTime * Time.deltaTime);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
