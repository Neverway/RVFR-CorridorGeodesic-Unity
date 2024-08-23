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

[RequireComponent(typeof(SignalReceiver))]
public class Elevator : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public bool isPowered;


    //=-----------------=
    // Private Variables
    //=-----------------=
    

    //=-----------------=
    // Reference Variables
    //=-----------------=
    private SignalReceiver signalReceiver;
    [SerializeField] private Animator animator;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        signalReceiver = GetComponent<SignalReceiver>();
        animator.keepAnimatorStateOnDisable = true;
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void OpenElevator()
    {
        if (isPowered) return;
        animator.Play("Elevator_Open");
        isPowered = true;
    }
    
    public void CloseElevator()
    {
        if (!isPowered) return;
        animator.Play("Elevator_Close");
        isPowered = false;
    }
    
    public void ToggleElevator()
    {
        if (isPowered)
        {
            CloseElevator();
        }
        else
        {
            OpenElevator();
        }
    }

    public void DescendElevator()
    {
        if (!isPowered) return;
        animator.Play("Elevator_Descending");
    }
}
