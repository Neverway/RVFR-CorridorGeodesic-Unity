//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn_FallDamage : MonoBehaviour
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
    private Pawn pawn;

    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        pawn = GetComponent<Pawn>();
    }

    private void OnCollisionEnter (Collision collision)
    {
        foreach (var contact in collision.contacts)
        {
           if (Vector3.Angle(contact.normal, Vector3.up) < pawn.currentState.steepSlopeAngle)
            {
                if (collision.relativeVelocity.y > pawn.currentState.minFallDamageVelocity)
                {
                    float percent = (collision.relativeVelocity.y - pawn.currentState.minFallDamageVelocity) / (pawn.currentState.fallDamageVelocity - pawn.currentState.minFallDamageVelocity);
                    float totalDamage = pawn.currentState.minFallDamage + ((pawn.currentState.fallDamage - pawn.currentState.minFallDamage) * percent);
                    Debug.Log ("Fall Damage " + totalDamage + "(" + percent + "%)");
                    pawn.ModifyHealth(-totalDamage);
                }
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}