//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public PawnController currentController;

    public PlayerState currentState;

    public string team;
    
    public bool isPossessed;
    public bool isPaused;
    public bool wasPaused; // used to restore a paused state when all pawns isPaused state is modified by the game instance
    public bool isInvulnerable;
    public bool isDead;

    public event Action OnEntityHurt;
    public event Action OnEntityHeal;
    public event Action OnEntityDeath;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
    }

    private void Update()
    {
        CheckCameraState();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void CheckCameraState()
    {
        if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
        if (gameInstance.localPlayerCharacter == this && GetComponentInChildren<Camera>(true)) GetComponentInChildren<Camera>(true).gameObject.SetActive(true);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void Hurt()
    {
        // Damages the pawn by a specified amount
        if (isInvulnerable || isDead) return;
    }

    public void Heal()
    {
        // Heals the pawn by a specified amount
        if (isInvulnerable || isDead) return;
    }

    public void Kill()
    {
        // Instantly sets the pawns health to zero, firing it's onDeath event
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
}
