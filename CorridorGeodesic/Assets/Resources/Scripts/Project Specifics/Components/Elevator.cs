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

public class Elevator : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private LogicComponent elevatorSignal, liftSignal;
    [SerializeField] private float descendSpeed = 3;
    private bool elevatorActivated;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Transform elevatorTransform;
    [SerializeField] private Animator animator;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void OnDrawGizmos()
    {
        if (elevatorSignal) Debug.DrawLine(transform.position, elevatorSignal.transform.position, Color.blue);
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    IEnumerator DescendElevator()
    {
        float timer = 10;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            elevatorTransform.position -= Vector3.up * Time.deltaTime * descendSpeed;
            yield return null;
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        if (!elevatorActivated)
        {
            if (elevatorSignal)
            {
                isPowered = elevatorSignal.isPowered;
                animator.SetBool("Powered", isPowered);
            }

            if (liftSignal && liftSignal.isPowered)
            {
                animator.SetBool("Powered", false);
                StartCoroutine(DescendElevator());
                elevatorActivated = true;
            }
        }
    }
}
