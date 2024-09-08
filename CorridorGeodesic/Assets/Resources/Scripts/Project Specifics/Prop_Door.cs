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
using UnityEngine.Events;

[RequireComponent(typeof(NEW_LogicProcessor))]
public class Prop_Door : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public NEW_LogicProcessor inputSignal;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------='
    private NEW_LogicProcessor logicProcessor;
    [Header("References")]
    [SerializeField] private Animator animator;
    //[SerializeField] private Material poweredMaterial, unpoweredMaterial;
    //[SerializeField] private GameObject indicatorMesh;
    [HideInInspector] [SerializeField] private UnityEvent onPowered;
    [HideInInspector] [SerializeField] private UnityEvent onUnpowered;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        logicProcessor = GetComponent<NEW_LogicProcessor>();
        //animator.keepAnimatorStateOnDisable = true;
    }

    private void Update()
    {
        if (!inputSignal) return;
        logicProcessor.isPowered = inputSignal.isPowered;
        
        if (inputSignal.hasPowerStateChanged)
        {
            if (logicProcessor.isPowered)
            {
                onPowered.Invoke();

                //animator.Play("Door_Open");
                //indicatorMesh.GetComponent<MeshRenderer>().material = poweredMaterial;
            }
            else
            {
                onUnpowered.Invoke();
                
                //animator.Play("Door_Close");
                //indicatorMesh.GetComponent<MeshRenderer>().material = unpoweredMaterial;
            }
            animator.SetBool("Powered", logicProcessor.isPowered);
        }
    }

    private void OnDrawGizmos()
    {
        if (inputSignal) Debug.DrawLine(gameObject.transform.position, inputSignal.transform.position, Color.blue);
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
