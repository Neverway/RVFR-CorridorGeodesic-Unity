//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: This is based off of the tutorial series by Plai
// Source: https://www.youtube.com/watch?v=LqnPeqoJRFY
//
//=============================================================================


using System;
using UnityEngine;

public class Pawn_WallRun : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private Transform orientation;

    [Header("Wall Running")] 
    [SerializeField] private float wallDistance = 0.6f;
    [SerializeField] private float minimumJumpHeight = 1.5f;
    [SerializeField] private float wallRunGravity = 5;
    [SerializeField] private float wallJumpForce = 5;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool wallLeft, wallRight;
    private RaycastHit leftWallHit, rightWallHit;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Rigidbody rigidbody;
    public InputActions.FirstPersonShooterActions fpsActions;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        fpsActions = new InputActions().FirstPersonShooter;
        fpsActions.Enable();
    }

    private void Update()
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
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }
    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    private void StartWallRun()
    {
        rigidbody.useGravity = false;
        rigidbody.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
        if (fpsActions.Jump.WasPressedThisFrame())
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y);
                rigidbody.AddForce(wallRunJumpDirection * (wallJumpForce * 100), ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y);
                rigidbody.AddForce(wallRunJumpDirection * (wallJumpForce * 100), ForceMode.Force);
            }
        }
    }

    private void StopWallRun()
    {
        rigidbody.useGravity = enabled;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
