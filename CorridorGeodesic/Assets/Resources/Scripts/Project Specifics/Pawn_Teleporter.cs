//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn_Teleporter : Volume
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

    [SerializeField] private Pawn_Teleporter linkedTeleporter;
    [SerializeField] public Transform pawnTeleportLocation;
    [SerializeField] private float teleportDelay = 0f;
    private const float DELAY_DURATION = 1f;
    private bool canTeleport;

    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Update ()
    {
        teleportDelay -= Time.deltaTime;
        canTeleport = false;
        if (teleportDelay <= 0f)
        {
            teleportDelay = 0f;
            if (Vector3.Angle (transform.up, Vector3.up) < 10)
            {
                canTeleport = true;
            }
        }
    }

    new private void OnTriggerEnter (Collider other)
    {
        base.OnTriggerEnter (other);
        if (!linkedTeleporter.gameObject.activeInHierarchy)
        {
            return;
        }
        if (canTeleport && linkedTeleporter.canTeleport && other.TryGetComponent<Pawn> (out var pawn))
        {
            pawn.GetComponent<Rigidbody>().position = linkedTeleporter.pawnTeleportLocation.position;
            teleportDelay = DELAY_DURATION;
            canTeleport = false;
            linkedTeleporter.teleportDelay = DELAY_DURATION;
            linkedTeleporter.canTeleport = false;
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}