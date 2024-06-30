//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Defines an interactable object in the game.
// Notes: This script should be attached to any object that can be interacted with by the player.
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.Events;

public class Logic_Interactable : MonoBehaviour
{
    // Channels for interacting with the object
    public string signalChannel; // Channel to listen for an activation or deactivation signal on

    public bool isPowered; // Flag indicating if the object is currently active

    public UnityEvent OnInteract, OnActivated, OnDeactivated;

    public void Update()
    {
        if (signalChannel == "") OnDeactivated.Invoke();
        // Initialize current active state
        if (isPowered) OnActivated.Invoke();
        else OnDeactivated.Invoke();
    }
}