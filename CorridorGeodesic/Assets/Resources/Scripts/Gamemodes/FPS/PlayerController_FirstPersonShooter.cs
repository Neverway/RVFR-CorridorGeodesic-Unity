//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: This player controller is based off of the tutorial series by Plai
// Source: https://www.youtube.com/watch?v=LqnPeqoJRFY
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[CreateAssetMenu(fileName="PlayerController_FirstPersonShooter", menuName="Neverway/ScriptableObjects/Pawns & Gamemodes/Controllers/PlayerController_FirstPersonShooter")]
public class PlayerController_FirstPersonShooter : PawnController
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector3 moveDirection;
    private Vector3 slopMoveDirection;
    private float yRotation;
    private float xRotation;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;
    public InputActions.FirstPersonShooterActions fpsActions;
    private Rigidbody rigidbody;
    private Camera viewCamera;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public override void PawnAwake(Pawn _pawn)
    {
        gameInstance = FindObjectOfType<GameInstance>();
        fpsActions = new InputActions().FirstPersonShooter;
        fpsActions.Enable();

        rigidbody = _pawn.GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;

        viewCamera = _pawn.GetComponentInChildren<Camera>();
        rigidbody.mass = _pawn.currentState.gravityMultiplier;
    }
    
    public override void PawnUpdate(Pawn _pawn)
    {
        if (fpsActions.Pause.WasPressedThisFrame()) gameInstance.UI_ShowPause();

        // Lock mouse when unpaused, unlock when paused
        if (_pawn.isPaused)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Item usage
        if (fpsActions.Primary.WasPressedThisFrame())
        {
            if (_pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false))
            {
                _pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false).UsePrimary();
            }
        }
        if (fpsActions.Secondary.WasPressedThisFrame())
        {
            if (_pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false))
            {
                _pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false).UseSecondary();
            }
        }
        
        UpdateMovement(_pawn);
        UpdateRotation(_pawn);
        UpdateJumping(_pawn);
        
        // Calculate Slope Movement
        //Debug.Log(slopMoveDirection);
        slopMoveDirection = Vector3.ProjectOnPlane(moveDirection, _pawn.slopeHit.normal);
    }

    public override void PawnFixedUpdate(Pawn _pawn)
    {
        MovePlayer(_pawn);
        if (_pawn.GetComponent<Pawn_WallRun>())
        {
            viewCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, _pawn.GetComponent<Pawn_WallRun>().tilt);
        }
        else
        {
            viewCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
        _pawn.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void UpdateMovement(Pawn _pawn)
    {
        // Pawn movement
        moveDirection = _pawn.transform.forward * fpsActions.Move.ReadValue<Vector2>().y + _pawn.transform.right * fpsActions.Move.ReadValue<Vector2>().x;
        ControlDrag(_pawn);
        ControlSprinting(_pawn);
    }

    private void UpdateRotation(Pawn _pawn)
    {
        // Pawn looking
        var multiplier = 0.01f;
        var applicationSettings = FindObjectOfType<ApplicationSettings>();
        yRotation += fpsActions.LookAxis.ReadValue<Vector2>().x*(20*applicationSettings.currentSettingsData.horizontalLookSpeed)*multiplier;
        xRotation -= fpsActions.LookAxis.ReadValue<Vector2>().y*(20*applicationSettings.currentSettingsData.verticalLookSpeed)*multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    private void UpdateJumping(Pawn _pawn)
    {
        //Debug.Log(_pawn.IsGrounded3D());
        if (fpsActions.Jump.WasPressedThisFrame() && _pawn.IsGrounded3D())
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
            rigidbody.AddForce(Vector3.up * _pawn.currentState.jumpForce, ForceMode.Impulse);
        }
    }
    
    private void MovePlayer(Pawn _pawn)
    {
        if (_pawn.IsGrounded3D() && !_pawn.IsGroundSloped3D())
        {
            rigidbody.AddForce(
                moveDirection.normalized * (_pawn.currentState.movementSpeed * _pawn.currentState.movementMultiplier),
                ForceMode.Acceleration);
        }
        else if (_pawn.IsGrounded3D() && _pawn.IsGroundSloped3D())
        {
            rigidbody.AddForce(
                slopMoveDirection.normalized * (_pawn.currentState.movementSpeed * _pawn.currentState.movementMultiplier),
                ForceMode.Acceleration);
        }
        else
        {
            rigidbody.AddForce(
                moveDirection.normalized * (_pawn.currentState.movementSpeed * (_pawn.currentState.movementMultiplier * _pawn.currentState.airMovementMultiplier)),
                ForceMode.Acceleration);
        }
    }

    private void ControlDrag(Pawn _pawn)
    {
        if (_pawn.IsGrounded3D())
        {
            rigidbody.drag = _pawn.currentState.groundDrag;
        }
        else
        {
            rigidbody.drag = _pawn.currentState.airDrag;
        }
    }

    private void ControlSprinting(Pawn _pawn)
    {
        if (fpsActions.Action.IsPressed() && _pawn.IsGrounded3D())
        {
            _pawn.currentState.movementSpeed = Mathf.Lerp(_pawn.currentState.movementSpeed,
                _pawn.defaultState.movementSpeed * _pawn.currentState.sprintSpeedMultiplier,
                _pawn.currentState.sprintAcceleration * Time.deltaTime);
        }
        else 
        {
            _pawn.currentState.movementSpeed = Mathf.Lerp(_pawn.currentState.movementSpeed,
                _pawn.defaultState.movementSpeed,
                _pawn.currentState.sprintAcceleration * Time.deltaTime);
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
