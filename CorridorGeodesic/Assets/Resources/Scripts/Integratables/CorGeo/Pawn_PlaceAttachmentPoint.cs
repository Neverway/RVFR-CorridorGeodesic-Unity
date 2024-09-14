//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

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

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        attachmentPointTransform.parent = null;
    }

    private void FixedUpdate()
    {
        Vector3 forwardPos = transform.forward * attachDistance;
        if (forwardPos.y > 0)
        {
            attachmentPointTransform.position = transform.position + forwardPos;
            attachmentPointTransform.rotation = transform.parent.rotation;
            return;
        }
        if (forwardPos.y < minYPos) forwardPos.y = minYPos;
        Vector3 playerForward = transform.parent.forward * attachDistance;
        Vector2 horizontalPosition = new Vector2 (playerForward.x, playerForward.z);
        horizontalPosition = horizontalPosition.normalized * attachDistance;
        Vector3 resultPos = new Vector3 (horizontalPosition.x, forwardPos.y, horizontalPosition.y);

        attachmentPointTransform.position = transform.position + resultPos;
        attachmentPointTransform.rotation = transform.parent.rotation;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}