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
                if (collision.relativeVelocity.y > pawn.currentState.fallDamageVelocity)
                {
                    Debug.Log ("!Fall Damage!");
                    pawn.ModifyHealth(-pawn.currentState.fallDamage);
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