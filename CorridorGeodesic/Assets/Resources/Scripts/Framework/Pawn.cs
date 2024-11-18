//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.Customs;

namespace Neverway.Framework
{
    public class Pawn : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public PawnController currentController;
        public CharacterData defaultState;
        public CharacterState currentState;

        public bool isPossessed;
        public bool isPaused;

        public bool
            wasPaused; // used to restore a paused state when all pawns isPaused state is modified by the game instance

        public bool isInvulnerable;
        public bool isDead;
        public bool isNearInteractable; // Imported from old system
        public bool destroyOnDeath; // Should this object be deleted once it dies
        public float destroyOnDeathDelay; // How long, in seconds, should we wait before deleting the pawn after death

        private List<ContactPoint>
            contactPoints = new List<ContactPoint>(); // Used to perform advanced collision checks (like step-up checks)

        public event Action OnPawnHurt;
        public event Action OnPawnHeal;
        public event Action OnPawnDeath;



        //=-----------------=
        // Private Variables
        //=-----------------=

        private Coroutine pawnHurtRoutine;
        private Coroutine pawnAutoHealRoutine;

        //=-----------------=
        // Reference Variables
        //=-----------------=
        private GameInstance gameInstance;
        public Quaternion faceDirection; // Imported from old system
        public RaycastHit slopeHit;
        public Pawn_AttachmentPoint physObjectAttachmentPoint;
        [SerializeField] private LayerMask groundDetectionLayerMask;

        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Awake()
        {
            PassCharacterDataToCurrentState();
            currentController.PawnAwake(this);
        }

        private void Update()
        {
            if (gameInstance == null)
            {
                gameInstance = FindObjectOfType<GameInstance>();
                if (gameInstance == null)
                {
                    Debug.LogWarning("Pawn could not find any active GameInstance");
                    return;
                }
            }

            // Quick slapped together check to figure out if a pawn is a player (since volumes look for isPossesed to determin player)
            isPossessed = gameInstance.PlayerControllerClasses.Contains(currentController);

            CheckCameraState();
            if (isDead) return;
            currentController.PawnUpdate(this);

            if (currentState.autoRegenHealth && pawnHurtRoutine == null && pawnAutoHealRoutine == null &&
                currentState.health < defaultState.health)
            {
                pawnAutoHealRoutine = StartCoroutine(PawnAutoHealRoutine());
            }
        }

        private void FixedUpdate()
        {
            if (isDead) return;
            currentController.PawnFixedUpdate(this);
            contactPoints.Clear(); //Deletes all ContactPoints collected from the last physics frame
        }

        void OnCollisionEnter(Collision col)
        {
            contactPoints.AddRange(col.contacts);
        }

        void OnCollisionStay(Collision col)
        {
            contactPoints.AddRange(col.contacts);
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=
        private void CheckCameraState()
        {
            var viewCamera = GetComponentInChildren<Camera>(true);
            if (IsPlayerControlled() && viewCamera)
            {
                viewCamera.gameObject.SetActive(true);
            }
            else if (viewCamera)
            {
                viewCamera.gameObject.SetActive(false);
            }
        }

        private IEnumerator InvulnerabilityCooldown()
        {
            isInvulnerable = true;
            yield return new WaitForSeconds(currentState.invulnerabilityTime);
            isInvulnerable = false;
        }

        // ------------------------------------------------------------------
        // MODIFY ME TO MATCH YOUR CharacterState & CharacterData CLASSES!!!
        // ------------------------------------------------------------------
        private void PassCharacterDataToCurrentState()
        {
            currentState.characterName = defaultState.actorName;
            currentState.health = defaultState.health;
            currentState.invulnerabilityTime = defaultState.invulnerabilityTime;
            currentState.movementSpeed = defaultState.movementSpeed;
            currentState.team = defaultState.team;
            currentState.animationController = defaultState.animationController;
            currentState.characterSounds = defaultState.characterSounds;
            currentState.groundCheckOffset = defaultState.groundCheckOffset;
            currentState.groundCheckRadius = defaultState.groundCheckRadius;
            currentState.groundMask = defaultState.groundMask;
            // Add project specific variables below this line!
            currentState.groundDrag = defaultState.groundDrag;
            currentState.airDrag = defaultState.airDrag;
            currentState.maxHorizontalMovementSpeed = defaultState.maxHorizontalMovementSpeed;
            currentState.maxHorizontalAirSpeed = defaultState.maxHorizontalAirSpeed;
            currentState.movementMultiplier = defaultState.movementMultiplier;
            currentState.airMovementMultiplier = defaultState.airMovementMultiplier;
            currentState.gravityMultiplier = defaultState.gravityMultiplier;
            currentState.jumpForce = defaultState.jumpForce;
            currentState.steepSlopeAngle = defaultState.steepSlopeAngle;
            currentState.sprintSpeedMultiplier = defaultState.sprintSpeedMultiplier;
            currentState.sprintAcceleration = defaultState.sprintAcceleration;
            currentState.autoRegenHealth = defaultState.autoRegenHealth;
            currentState.healthRegenPerSecond = defaultState.healthRegenPerSecond;
            currentState.healthRegenDelay = defaultState.healthRegenDelay;
            currentState.fallDamage = defaultState.fallDamage;
            currentState.minFallDamage = defaultState.minFallDamage;
            currentState.fallDamageVelocity = defaultState.fallDamageVelocity;
            currentState.minFallDamageVelocity = defaultState.minFallDamageVelocity;
        }
        // ------------------------------------------------------------------


        //=-----------------=
        // External Functions
        //=-----------------=
        public bool IsGrounded3D()
        {
            //return Physics.SphereCast(transform.position, currentState.groundCheckRadius, Vector3.down, out RaycastHit hit,
            //    1.1f, currentState.groundMask);

            //return hit.distance < 1.5f;

            return Physics.CheckSphere(transform.position - currentState.groundCheckOffset,
                currentState.groundCheckRadius,
                currentState.groundMask,
                QueryTriggerInteraction.Ignore);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position - currentState.groundCheckOffset, currentState.groundCheckRadius);
        }

        public bool IsGroundSloped3D()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, currentState.groundCheckOffset.y + 0.5f,
                    currentState.groundMask, QueryTriggerInteraction.Ignore))
            {
                return slopeHit.normal != Vector3.up;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the angle of the slope is greater than steepSlopeAngle.
        /// </summary>
        /// <returns></returns>
        public bool IsGroundSteep3D()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, currentState.groundCheckOffset.y + 0.5f,
                    groundDetectionLayerMask, QueryTriggerInteraction.Ignore))
            {
                float angle = Vector3.Angle(slopeHit.normal, Vector3.up);
                return angle > currentState.steepSlopeAngle;
            }

            return false;
        }

        public bool IsPlayerControlled()
        {
            if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
            return gameInstance.PlayerControllerClasses.Contains(currentController);
        }

        public void Move(Vector3 _movement, string _mode)
        {
            if (_mode == "translate")
                transform.Translate(_movement * (
                    currentState.movementSpeed * Time.deltaTime));
        }

        public void Move(Vector3 _movement, string _mode, float _movementSpeed)
        {
            if (_mode == "translate") transform.Translate(_movement * (_movementSpeed * Time.deltaTime));
        }
        
        public void ModifyHealth(float _value)
        {
            if (isInvulnerable) return;
            StartCoroutine(InvulnerabilityCooldown());
            switch (_value)
            {
                case > 0:
                    OnPawnHeal?.Invoke();
                    isDead = false;
                    if (currentState.characterSounds.heal)
                        GetComponent<AudioSource_PitchVarienceModulator>().PlaySound(currentState.characterSounds.heal);
                    break;
                case < 0:
                    if (isDead) return;
                    StartHealthRegen();
                    try
                    {
                        OnPawnHurt?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    if (currentState.characterSounds.hurt)
                        GetComponent<AudioSource_PitchVarienceModulator>().PlaySound(currentState.characterSounds.hurt);
                    break;
            }

            if (currentState.health + _value <= 0)
            {
                if (isDead) return;
                GetComponent<AudioSource_PitchVarienceModulator>().PlaySound(currentState.characterSounds.death);
                OnPawnDeath?.Invoke();
                isDead = true;
                if (destroyOnDeath)
                {
                    Destroy(gameObject, destroyOnDeathDelay);
                }
            }

            if (currentState.health + _value > defaultState.health) currentState.health = defaultState.health;
            else if (currentState.health + _value < 0) currentState.health = 0;
            else currentState.health += _value;
        }

        private void StartHealthRegen()
        {
            if (!currentState.autoRegenHealth)
            {
                return;
            }

            if (pawnHurtRoutine != null)
            {
                StopCoroutine(pawnHurtRoutine);
            }

            if (pawnAutoHealRoutine != null)
            {
                StopCoroutine(pawnAutoHealRoutine);
            }

            pawnHurtRoutine = StartCoroutine(PawnHurtRoutine());
        }

        public void Kill()
        {
            // Instantly sets the pawns health to zero, firing its onDeath event
            ModifyHealth(-999999);
        }

        public void GetPawnController()
        {
            // Returns the type of controller that is possessing this pawn
            // This can be used to do things like checking if a pawn is possessed by a player
        }

        public void SetPawnController()
        {
            // Sets the type of controller that is possessing this pawn
        }

        public void SetPawnDefaultState(CharacterData _playerState)
        {
            // Sets the type of character
            defaultState = _playerState;
            PassCharacterDataToCurrentState();
        }

        private IEnumerator PawnHurtRoutine()
        {
            yield return new WaitForSeconds(currentState.healthRegenDelay);
            if (pawnAutoHealRoutine != null)
            {
                StopCoroutine(pawnAutoHealRoutine);
            }

            pawnAutoHealRoutine = StartCoroutine(PawnAutoHealRoutine());
            pawnHurtRoutine = null;
        }

        private IEnumerator PawnAutoHealRoutine()
        {
            while (currentState.health > 0 && currentState.health < defaultState.health)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log("Auto-healing");
                ModifyHealth(currentState.healthRegenPerSecond);
            }

            pawnAutoHealRoutine = null;
        }

    }
}
