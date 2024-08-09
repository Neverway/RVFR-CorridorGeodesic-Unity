//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Animator animator;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    

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
