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
    }
    
    public override void PawnUpdate(Pawn _pawn)
    {
        if (fpsActions.Pause.WasPressedThisFrame()) gameInstance.UI_ShowPause();

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
        
        
        moveDirection = _pawn.transform.forward * fpsActions.Move.ReadValue<Vector2>().y + _pawn.transform.right * fpsActions.Move.ReadValue<Vector2>().x;
        ControlDrag(_pawn);

        var multiplier = 0.01f;
        var applicationSettings = FindObjectOfType<ApplicationSettings>();
        yRotation += fpsActions.LookAxis.ReadValue<Vector2>().x*(20*applicationSettings.currentSettingsData.verticalLookSpeed)*multiplier;
        xRotation -= fpsActions.LookAxis.ReadValue<Vector2>().y*(20*applicationSettings.currentSettingsData.horizontalLookSpeed)*multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    }

    public override void PawnFixedUpdate(Pawn _pawn)
    {
        MovePlayer(_pawn);
        viewCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        _pawn.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void MovePlayer(Pawn _pawn)
    {
        rigidbody.AddForce(moveDirection.normalized * (_pawn.currentState.movementSpeed * _pawn.currentState.movementMultiplier), ForceMode.Acceleration);
    }

    private void ControlDrag(Pawn _pawn)
    {
        rigidbody.drag = _pawn.currentState.drag;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
