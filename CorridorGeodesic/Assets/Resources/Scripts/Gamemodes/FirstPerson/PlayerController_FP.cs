//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: This player controller is based off of the tutorial series by Plai,
//  The Source Engine style step-up method is by Cobertos
// Plai Source: https://www.youtube.com/watch?v=LqnPeqoJRFY
// Cobertos Source: https://cobertos.com/blog/post/how-to-climb-stairs-unity3d
//
//=============================================================================

using System.Diagnostics;
using UnityEngine;
using Neverway.Framework;
using Neverway.Framework.ApplicationManagement;
using Neverway.Framework.LogicSystem;
using Neverway.Framework.PawnManagement;
using Debug = UnityEngine.Debug;
using GameInstance = Neverway.Framework.PawnManagement.GameInstance;

namespace Neverway
{/*
    [CreateAssetMenu(fileName = "PlayerController_FirstPerson",
        menuName = "Neverway/ScriptableObjects/Pawns & Gamemodes/Controllers/PlayerController_FirstPerson")]
    public class PlayerController_FirstPerson : PawnController
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [Tooltip("How hard a physics prop is tossed when the player throws it")]
        [SerializeField] public float throwForce = 150;
        [Tooltip("How many seconds after the player has stepped off a ledge can they still jump for")]
        [SerializeField] public float coyoteTime = 0.2f;
        [SerializeField] public float jumpInputBuffer = 0.2f;
        [Tooltip("Debug parameter to allow the player to spam jump to fly around a level")]
        [SerializeField] public bool allowJumpCheat = true;

        //=-----------------=
        // Private Variables
        //=-----------------=
        private Vector3 moveDirection;
        private Vector3 slopMoveDirection;

        // Input buffering for jumping
        private float timeLastCouldJump = -1f;
        private float timeLastInputJump = -1f;
        private float timeLastJumped = -1f;
        private bool ignoreGroundCheckToAvoidDoubleJump = false;
        private bool doJumpCheat = false;

        private bool isCrouching;

        [HideInInspector] public float yRotation;
        [HideInInspector] public float xRotation;


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private GameInstance gameInstance;
        private Rigidbody rigidbody;
        private Camera viewCamera;
        private InputActions.FirstPersonActions fpActions;
        private CapsuleCollider collider1;
        private CapsuleCollider collider2;

        [Tooltip("When the player presses the interact action this volume prefab is created and used similar to a raycast for detecting interactions")]
        [SerializeField] private GameObject interactionVolume;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        public override void PawnAwake(Pawn _pawn)
        {
            // Get references
            gameInstance = FindObjectOfType<GameInstance>();
            rigidbody = _pawn.GetComponent<Rigidbody>();
            viewCamera = _pawn.GetComponentInChildren<Camera>();

            // Setup inputs
            fpActions = new InputActions().FirstPerson;
            fpActions.Enable();

            // Assign initial values
            rigidbody.mass = _pawn.currentState.gravityMultiplier;
            rigidbody.freezeRotation = true;

            // Subscribe to events
            _pawn.OnPawnDeath += () => { OnDeath(_pawn); };

            // Turn off jump cheat
            doJumpCheat = false;
        }

        public override void PawnUpdate(Pawn _pawn)
        {
            // Check for pause input and set cursor locking accordingly
            UpdatePauseMenu(_pawn);
            if (_pawn.isPaused) return;
            UpdateInteractionUsage(_pawn);

            // Debug Respawn
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                _pawn.ModifyHealth(-9999);
            }

            UpdateMovement(_pawn);
            UpdateRotation(_pawn);
            UpdateJumping(_pawn);
            UpdateCrouching(_pawn);

            // Calculate Slope Movement
            slopMoveDirection = Vector3.ProjectOnPlane(moveDirection, _pawn.slopeHit.normal);
        }

        public override void PawnFixedUpdate(Pawn _pawn)
        {
            if (_pawn.isDead)
            {
                return;
            }

            MovePlayer(_pawn);

            // Set wall-running view tilt
            if (_pawn.GetComponent<Pawn_WallRun>())
            {
                viewCamera.transform.localRotation =
                    Quaternion.Euler(xRotation, 0, _pawn.GetComponent<Pawn_WallRun>().tilt);
            }
            else
            {
                viewCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            }
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=
        private void UpdatePauseMenu(Pawn _pawn)
        {
            // Pause Game
            if (fpActions.Pause.WasPressedThisFrame()) gameInstance.UI_ShowPause();

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
        }

        private void UpdateInteractionUsage(Pawn _pawn)
        {
            // Item usage
            if (!_pawn.physObjectAttachmentPoint) return;
            if (fpActions.Secondary.IsPressed())
            {
                if (_pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false))
                {
                    _pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false).UseSecondary();
                }
            }

            if (fpActions.Secondary.WasReleasedThisFrame())
            {
                if (_pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false))
                {
                    _pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false).ReleaseSecondary();
                }
            }
            /*
            if (fpActions.ClearRift.WasPressedThisFrame())
            {
                if (_pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false))
                {
                    _pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false).UseSpecial();
                }
            }

            if (!_pawn.physObjectAttachmentPoint.heldObject)
            {
                if (fpActions.Primary.WasPressedThisFrame())
                {
                    if (_pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false))
                    {
                        _pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(false).UsePrimary();
                    }
                }
            }
            // Throw object
            else
            {
                if (fpActions.Primary.WasPressedThisFrame())
                {
                    _pawn.physObjectAttachmentPoint.heldObject.GetComponent<Rigidbody>()
                        .AddForce(viewCamera.transform.forward * throwForce, ForceMode.Impulse);
                    _pawn.physObjectAttachmentPoint.heldObject.GetComponent<Object_Grabbable>().ToggleHeld();
                }
            }

            // Interact
            if (fpActions.Interact.WasPressedThisFrame())
            {
                if (_pawn.physObjectAttachmentPoint.heldObject != null
                    && _pawn.physObjectAttachmentPoint.heldObject.TryGetComponent<Object_Grabbable>(out var grabbable)
                   )
                {
                    grabbable.Drop();
                }
                else //If player wasn't holding anything, then spawn an interactionVolume in front of the camera for a frame.
                {
                    var interaction = Instantiate(interactionVolume, viewCamera.transform);
                    interaction.transform.GetChild(0).GetComponent<Volume_TriggerInteraction>().targetPawn = _pawn;
                    Destroy(interaction, 0.1f);
                }
            }
        }

        private void UpdateMovement(Pawn _pawn)
        {
            // Pawn movement
            moveDirection = _pawn.transform.forward * fpActions.Move.ReadValue<Vector2>().y +
                            _pawn.transform.right * fpActions.Move.ReadValue<Vector2>().x;
            ControlDrag(_pawn);
            ControlSprinting(_pawn);
        }

        private void UpdateRotation(Pawn _pawn)
        {
            var applicationSettings = FindObjectOfType<ApplicationSettings>();
            // Separate multipliers for mouse and joystick
            float mouseMultiplier = 0.01f;
            float joystickMultiplier = 0.2f;
            //float mouseMultiplier = applicationSettings.currentSettingsData.mouseLookSensitivity;
            //float joystickMultiplier = applicationSettings.currentSettingsData.joystickLookSensitivity;

            // Determine the input method (mouse or joystick)
            // ReSharper disable once ReplaceWithSingleAssignment.False
            bool isUsingMouse = false;
            if (fpActions.LookAxis.IsInProgress())
            {
                if (fpActions.LookAxis.activeControl.device.name == "Mouse")
                {
                    isUsingMouse = true;
                }
            }

            // Apply the appropriate multiplier
            var multiplier = isUsingMouse ? mouseMultiplier : joystickMultiplier;
            yRotation += fpActions.LookAxis.ReadValue<Vector2>().x *
                         (20 * applicationSettings.currentSettingsData.horizontalLookSpeed) * multiplier;
            xRotation -= fpActions.LookAxis.ReadValue<Vector2>().y *
                         (20 * applicationSettings.currentSettingsData.verticalLookSpeed) * multiplier;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            _pawn.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        private void UpdateJumping(Pawn _pawn)
        {
            if (allowJumpCheat && Input.GetKeyDown(KeyCode.J) &&
                Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) &&
                Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                doJumpCheat = !doJumpCheat;
                if (doJumpCheat)
                {
                    try
                    {
                        Stopwatch timer = FindObjectOfType<Stopwatch>();
                        timer.InvalidateTimer();
                    }
                    catch
                    {
                    }
                }
            }

            if (fpActions.Jump.WasPressedThisFrame())
                timeLastInputJump = Time.time;

            if (!ignoreGroundCheckToAvoidDoubleJump && _pawn.IsGrounded3D() && !_pawn.IsGroundSteep3D())
                timeLastCouldJump = Time.time;

            bool jumpInputValid = Time.time <= (timeLastInputJump + jumpInputBuffer);
            bool jumpConditionValid = Time.time <= (timeLastCouldJump + coyoteTime);

            //start mchecking for ground again if moving down or stationary (0.1 instead of 0 for some degree of error)
            ignoreGroundCheckToAvoidDoubleJump = ignoreGroundCheckToAvoidDoubleJump && rigidbody.velocity.y > 0.1f;

            if (Time.time <=
                timeLastJumped +
                (coyoteTime * 2f)) //If enough time passed and the flag is STILL not set false, just set it false
            {
                ignoreGroundCheckToAvoidDoubleJump = false;
            }

            if (doJumpCheat)
                jumpConditionValid = true;

            //Debug.Log(_pawn.IsGrounded3D());
            if (jumpInputValid && jumpConditionValid)
            {
                timeLastCouldJump = -1f;
                timeLastInputJump = -1f;
                timeLastJumped = Time.time;
                ignoreGroundCheckToAvoidDoubleJump = true;

                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
                rigidbody.AddForce(Vector3.up * _pawn.currentState.jumpForce, ForceMode.Impulse);

                EndOfDemoStatsTracker.instance.AddJumpCount();
            }
        }

        private void UpdateCrouching(Pawn _pawn)
        {
            // ensure we have references to our colliders


            // if we are holding crouch and we aren't crouching
            if (fpActions.Crouch.IsPressed() && !isCrouching)
            {
                // set crouching
                isCrouching = true;
                // scale the collider down

                // adjust the collider offset

            }
            // if we are not holding crouch and we are crouching
            if (!fpActions.Crouch.IsPressed() && isCrouching)
            {
                // set not crouching
                isCrouching = false;
                // scale the collider up

                // adjust the collider offset back

            }
        }

        private void MovePlayer(Pawn _pawn)
        {
            var currentVelocity = rigidbody.velocity;
            var desiredGroundVelocity = moveDirection * (_pawn.currentState.movementSpeed * _pawn.currentState.movementMultiplier);
            var desiredSlopeVelocity = slopMoveDirection * (_pawn.currentState.movementSpeed * _pawn.currentState.movementMultiplier);
            var desiredAirVelocity = moveDirection * (_pawn.currentState.movementSpeed * (_pawn.currentState.movementMultiplier * _pawn.currentState.airMovementMultiplier));
            var groundAccelerationRate = 0.1f;
            var slopeAccelerationRate = 0.25f;
            var airAccelerationRate = 0.2f;
            //Debug.Log($"C{currentVelocity}");
            //Debug.Log($"T{desiredGroundVelocity}");

            // Ground Movement
            if (_pawn.IsGrounded3D() && !_pawn.IsGroundSloped3D())
            {
                // if current is less than target and target is positive, or current is greater than target and target is negative
                if (currentVelocity.x < desiredGroundVelocity.x && desiredGroundVelocity.x > 0f || currentVelocity.x > desiredGroundVelocity.x && desiredGroundVelocity.x < 0f )
                {
                    rigidbody.velocity += new Vector3(desiredGroundVelocity.x*groundAccelerationRate, 0, 0);
                }
                if (currentVelocity.z < desiredGroundVelocity.z && desiredGroundVelocity.z > 0f || currentVelocity.z > desiredGroundVelocity.z && desiredGroundVelocity.z < 0f )
                {
                    rigidbody.velocity += new Vector3(0, 0, desiredGroundVelocity.z*groundAccelerationRate);
                }
            }
            // Slope Movement
            else if (_pawn.IsGrounded3D() && _pawn.IsGroundSloped3D())
            {
                // if current is less than target and target is positive, or current is greater than target and target is negative
                if (currentVelocity.x < desiredSlopeVelocity.x && desiredSlopeVelocity.x > 0f || currentVelocity.x > desiredSlopeVelocity.x && desiredSlopeVelocity.x < 0f )
                {
                    rigidbody.velocity += new Vector3(desiredSlopeVelocity.x*slopeAccelerationRate, 0, 0);
                }
                if (currentVelocity.z < desiredSlopeVelocity.z && desiredSlopeVelocity.z > 0f || currentVelocity.z > desiredSlopeVelocity.z && desiredSlopeVelocity.z < 0f )
                {
                    rigidbody.velocity += new Vector3(0, 0, desiredSlopeVelocity.z*slopeAccelerationRate);
                }
            }
            // Air Movement
            else
            {
                // if current is less than target and target is positive, or current is greater than target and target is negative
                if (currentVelocity.x < desiredAirVelocity.x && desiredAirVelocity.x > 0f || currentVelocity.x > desiredAirVelocity.x && desiredAirVelocity.x < 0f )
                {
                    rigidbody.velocity += new Vector3(desiredAirVelocity.x*airAccelerationRate, 0, 0);
                }
                if (currentVelocity.z < desiredAirVelocity.z && desiredAirVelocity.z > 0f || currentVelocity.z > desiredAirVelocity.z && desiredAirVelocity.z < 0f )
                {
                    rigidbody.velocity += new Vector3(0, 0, desiredAirVelocity.z*airAccelerationRate);
                }
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
            if (fpActions.ClearRift.IsPressed() && _pawn.IsGrounded3D())
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

        private void OnDeath(Pawn _pawn)
        {
            // Drop held props
            if (_pawn.physObjectAttachmentPoint.heldObject)
            {
                if (_pawn.physObjectAttachmentPoint.heldObject.GetComponent<Object_Grabbable>())
                {
                    _pawn.physObjectAttachmentPoint.heldObject.GetComponent<Object_Grabbable>().ToggleHeld();
                }
            }

            // Call the function on the georipper to remove all rifts
            // TODO this currently assumes the player is ONLY ever holding the georipper
            // An if statement should be added to check if there were any rifts or something
            _pawn.transform.GetComponentInChildren<Item_Geodesic_Utility>(true).UseSpecial();

            // Remove the HUD
            Destroy(GameInstance.GetWidget("WB_HUD"));
            // Add the respawn HUD
            gameInstance.UI_ShowDeathScreen();

            // Play the death animation
            _pawn.GetComponent<Animator>().Play("Death");
        }


        //=-----------------=
        // External Functions
        //=-----------------=

    }
           */
}