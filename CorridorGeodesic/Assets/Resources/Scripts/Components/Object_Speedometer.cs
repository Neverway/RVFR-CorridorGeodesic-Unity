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


    //=-----------------=
    // Private Variables
    //=-----------------=
    
    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    private TMP_Text velocityText;
    [SerializeField] private Rigidbody entityRigidbody;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        velocityText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        velocityText.text = "Velocity: " + entityRigidbody.velocity.magnitude.ToString("F2") + " m/s";
    }
    
    
    //=-----------------=
    // Internal Functions
    //=-----------------=
    
    
    //=-----------------=
    // External Functions
    //=-----------------=
}

