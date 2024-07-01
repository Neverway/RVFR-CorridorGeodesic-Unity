//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using UnityEngine;

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
    public bool wasPaused; // used to restore a paused state when all pawns isPaused state is modified by the game instance
    public bool isInvulnerable;
    public bool isDead;
    public bool isNearInteractable; // Imported from old system
    public bool destroyOnDeath; // Should this object be deleted once it dies
    public float destroyOnDeathDelay; // How long, in seconds, should we wait before deleting the pawn after death

    public event Action OnPawnHurt;
    public event Action OnPawnHeal;
    public event Action OnPawnDeath;
    


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;
    public Quaternion faceDirection; // Imported from old system
    public RaycastHit slopeHit;


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
        gameInstance = FindObjectOfType<GameInstance>();
        CheckCameraState();
        if (isDead) return;
        currentController.PawnUpdate(this);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        currentController.PawnFixedUpdate(this);
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
        yield return new WaitForSeconds(1);
        isInvulnerable = false;
    }
    
    // ------------------------------------------------------------------
    // MODIFY ME TO MATCH YOUR CharacterState & CharacterData CLASSES!!!
    // ------------------------------------------------------------------
    private void PassCharacterDataToCurrentState()
    {
        currentState.characterName = defaultState.actorName;
        currentState.health = defaultState.health;
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
        currentState.movementMultiplier = defaultState.movementMultiplier;
        currentState.airMovementMultiplier = defaultState.airMovementMultiplier;
        currentState.gravityMultiplier = defaultState.gravityMultiplier;
        currentState.jumpForce = defaultState.jumpForce;
        currentState.sprintSpeedMultiplier = defaultState.sprintSpeedMultiplier;
        currentState.sprintAcceleration = defaultState.sprintAcceleration;
    }
    // ------------------------------------------------------------------


    //=-----------------=
    // External Functions
    //=-----------------=
    public bool IsGrounded3D()
    {
        return Physics.CheckSphere(transform.position - currentState.groundCheckOffset, currentState.groundCheckRadius, currentState.groundMask);
    }
    
    public bool IsGroundSloped3D()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, currentState.groundCheckOffset.y + 0.5f))
        {
            return slopeHit.normal != Vector3.up;
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
        if (_mode == "translate") transform.Translate(_movement * (
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
                if (currentState.characterSounds.heal) GetComponent<AudioSource_PitchVarienceModulator>().PlaySound(currentState.characterSounds.heal);
                break;
            case < 0:
                if (isDead) return;
                OnPawnHurt?.Invoke();
                if (currentState.characterSounds.hurt) GetComponent<AudioSource_PitchVarienceModulator>().PlaySound(currentState.characterSounds.hurt);
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

        if (currentState.health + _value > currentState.health) currentState.health = defaultState.health;
        else if (currentState.health + _value < 0) currentState.health = 0;
        else currentState.health += _value;
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
}
