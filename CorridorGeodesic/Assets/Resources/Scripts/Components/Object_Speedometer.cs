//======== Neverway 2023 Project Script | Written by Arthur Aka Liz ===========
// 
// Type: 
// Purpose: 
// Applied to: 
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Text_Speedometer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private bool UseLocalPlayer;


    //=-----------------=
    // Private Variables
    //=-----------------=
    
    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    private TMP_Text velocityText;
    [SerializeField] private Rigidbody entityRigidbody;
    private GameInstance gameInstance;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        velocityText = GetComponent<TMP_Text>();
        gameInstance = FindObjectOfType<GameInstance>();
    }

    private void Update()
    {
        if (UseLocalPlayer && !entityRigidbody)
        {
            gameInstance = FindObjectOfType<GameInstance>();
            if (gameInstance.localPlayerCharacter.GetComponent<Rigidbody>()) entityRigidbody = gameInstance.localPlayerCharacter.GetComponent<Rigidbody>();
        }
        if (!entityRigidbody) return;
        velocityText.text = "Velocity: " + entityRigidbody.velocity.magnitude.ToString("F2") + " m/s";
    }
    
    
    //=-----------------=
    // Internal Functions
    //=-----------------=
    
    
    //=-----------------=
    // External Functions
    //=-----------------=
}

