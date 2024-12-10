//===================== (Neverway 2024) Written by Connorses. =====================
//
// Purpose: Places the point that held objects will magnet to.
// Notes: Ignores rigidbodies and triggers for now.
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Neverway.Framework.PawnManagement;
using Neverway.Framework.LogicSystem;

public class Pawn_PlaceAttachmentPoint : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private float attachDistance;
    [SerializeField] private float maxYPos = 0.7f;
    [SerializeField] private float minYPos = -0.7f;
    [SerializeField] private Transform attachmentPointTransform;
    [SerializeField] private Pawn targetPawn;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    
    private void FixedUpdate()
    {
        if (Alt_Item_Geodesic_Utility_GeoGun.delayRiftCollapse == true)
        {
            return;
        }
        Vector3 forwardPos = transform.forward * attachDistance;
        Vector3 playerForward = transform.parent.forward * attachDistance;
        Vector2 horizontalPosition = new Vector2 (playerForward.x, playerForward.z);

        Vector3 resultPos;
        // Constrain the y-position of the attachment point
        if (forwardPos.y > 0)
        {
            resultPos = forwardPos;
        }
        else
        {
            if (forwardPos.y < minYPos) forwardPos.y = minYPos;
        
            horizontalPosition = horizontalPosition.normalized * attachDistance;
            resultPos = new Vector3 (horizontalPosition.x, forwardPos.y, horizontalPosition.y);
        }

        RaycastHit[] hits = Physics.SphereCastAll (transform.position, 0.2f, resultPos, resultPos.magnitude);
        float nearestHit = resultPos.magnitude;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.isTrigger)
                {
                    continue;
                }
                if (hit.collider.tag == "PhysProp")
                {
                    continue;
                }
                if (hit.collider.TryGetComponent<Rigidbody> (out var rb))
                {
                    continue;
                }
            }

            if (hit.distance < nearestHit)
            {
                nearestHit = hit.distance;
            }
        }

        if (nearestHit < resultPos.magnitude)
        {
            nearestHit = nearestHit - 0.25f;
            if (nearestHit < 0.35)
            {
                if (
                    targetPawn.physObjectAttachmentPoint.heldObject != null &&
                    Alt_Item_Geodesic_Utility_GeoGun.delayRiftCollapse == false &&
                    targetPawn.physObjectAttachmentPoint.heldObject.TryGetComponent<Object_Grabbable> (out var heldObject)
                    )
                {
                    heldObject.Drop ();
                }
            }
            resultPos = resultPos.normalized * (nearestHit);
        }

        attachmentPointTransform.position = transform.position + resultPos;
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}