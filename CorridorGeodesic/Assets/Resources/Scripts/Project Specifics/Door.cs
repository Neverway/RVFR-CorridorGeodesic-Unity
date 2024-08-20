//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public bool isPowered;


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private Animator animator;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        animator.keepAnimatorStateOnDisable = true;
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void OpenDoor()
    {
        if (isPowered) return;
        animator.SetBool("IsPowered", true);
        isPowered = true;
    }
    
    public void CloseDoor()
    {
        if (!isPowered) return;
        animator.SetBool("IsPowered", false);
        isPowered = false;
    }
    
    public void ToggleDoor()
    {
        if (isPowered)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
}
