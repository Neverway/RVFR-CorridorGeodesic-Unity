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
public class Door : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    //public bool signalReceiver.isPowered;


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private Animator animator;
    private bool isOpen;


    //=-----------------=
    // Reference Variables
    //=-----------------='
    private SignalReceiver signalReceiver;
    [SerializeField] private Material poweredMaterial, unpoweredMaterial;
    [SerializeField] private GameObject indicatorMesh;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        signalReceiver = GetComponent<SignalReceiver>();
        animator.keepAnimatorStateOnDisable = true;
    }

    private void Update()
    {
        if (signalReceiver.isPowered)
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
        if (!signalReceiver.isPowered) return;
        if (isOpen) return;
        animator.SetBool("IsPowered", true);
        isOpen = true;
    }
    
    public void CloseDoor()
    {
        if (!signalReceiver.isPowered) return;
        if (!isOpen) return;
        animator.SetBool("IsPowered", false);
        isOpen = false;
    }
    
    public void ToggleDoor()
    {
        if (!signalReceiver.isPowered) return;
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    /*public void SetPowered(bool _isPowered)
    {
        signalReceiver.isPowered = _isPowered;
    }*/
}
