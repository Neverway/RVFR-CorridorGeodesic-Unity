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
    private bool isOpen;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Material poweredMaterial, unpoweredMaterial;
    [SerializeField] private GameObject indicatorMesh;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        animator.keepAnimatorStateOnDisable = true;
    }

    private void Update()
    {
        if (isPowered)
        {
            indicatorMesh.GetComponent<MeshRenderer>().material = poweredMaterial;
        }
        else
        {
            indicatorMesh.GetComponent<MeshRenderer>().material = unpoweredMaterial;
        }
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void OpenDoor()
    {
        if (!isPowered) return;
        if (isOpen) return;
        animator.SetBool("IsPowered", true);
        isOpen = true;
    }
    
    public void CloseDoor()
    {
        if (!isPowered) return;
        if (!isOpen) return;
        animator.SetBool("IsPowered", false);
        isOpen = false;
    }
    
    public void ToggleDoor()
    {
        if (!isPowered) return;
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void SetPowered(bool _isPowered)
    {
        isPowered = _isPowered;
    }
}
