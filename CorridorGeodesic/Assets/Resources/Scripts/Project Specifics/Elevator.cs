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

[RequireComponent(typeof(NEW_LogicProcessor))]
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
    private NEW_LogicProcessor logicProcessor;
    [SerializeField] private Animator animator;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        logicProcessor = GetComponent<NEW_LogicProcessor>();
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
