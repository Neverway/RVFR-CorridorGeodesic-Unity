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
using UnityEngine.UI;

public class Object_Grabbable : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    public bool isHeld;
    private bool is2D;
    private bool wasGravityEnabled;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public Pawn targetPawn;
    public Vector3 lastFaceDirection;
    private Volume_TriggerInteractable interactableTrigger;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        if (transform.childCount == 0) return;
        interactableTrigger = transform.GetChild(0).GetComponent<Volume_TriggerInteractable>();
    }

    private void Update()
    {
        if (!isHeld || !targetPawn)
        {
            if (interactableTrigger) interactableTrigger.hideIndicator = false;
            return;
        }

        if (interactableTrigger) interactableTrigger.hideIndicator = true;
        
        if (is2D)
        {
            transform.parent.position = targetPawn.transform.position + Get2DTargetPawnOffset();
            //transform.parent.GetComponent<Object_DepthAssigner2D>().depthLayer = targetPawn.GetComponent<Object_DepthAssigner2D>().depthLayer;
        }
        else
        {
            //transform.position = targetPawn.physObjectAttachmentPoint.transform.position;
            //var targetRotation = targetPawn.physObjectAttachmentPoint.transform.localRotation;
            //transform.rotation = new Quaternion(targetRotation.x, targetRotation.y, targetRotation.z, targetRotation.w);
            targetPawn.physObjectAttachmentPoint.gameObject.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = new Vector3();
            // Drop the object if it's too far away
            if (Vector3.Distance(gameObject.transform.position,
                    targetPawn.physObjectAttachmentPoint.transform.position) > 2.5)
            {
                ToggleHeld();
            }
        }
    }

//=-----------------=
    // Internal Functions
    //=-----------------=
    private Vector3 Get2DTargetPawnOffset()
    {
        var facingDirection = Get2DFaceDirectionFromQuaternion(targetPawn.faceDirection, -90);
        switch (facingDirection.y)
        {
            case (1):
                lastFaceDirection = new Vector3(0, 0.25f, 0);
                return lastFaceDirection;
            case (-1):
                lastFaceDirection = new Vector3(0, -0.3f, 0);
                return lastFaceDirection;
        }
        
        switch (facingDirection.x)
        {
            case (1):
                lastFaceDirection = new Vector3(0.5f, 0, 0);
                return lastFaceDirection;
            case (-1):
                lastFaceDirection = new Vector3(-0.5f, 0, 0);
                return lastFaceDirection;
        }
        return lastFaceDirection;
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        var interaction = _other.GetComponent<Volume_TriggerInteraction>();
        if (interaction)
        {
            targetPawn = interaction.targetPawn;
            ToggleHeld();
            is2D = true;
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        var interaction = _other.GetComponent<Volume_TriggerInteraction>();
        if (interaction)
        {
            targetPawn = interaction.targetPawn;
            if (!isHeld)
            {
                // If this isn't being held, and the pawn is already holding something, exit
                if (targetPawn.physObjectAttachmentPoint.GetComponent<Pawn_AttachmentPoint>().heldObject!=null)
                {
                    return;
                }
                targetPawn.physObjectAttachmentPoint.GetComponent<Pawn_AttachmentPoint>().heldObject = gameObject;
                wasGravityEnabled=GetComponent<Rigidbody>().useGravity; // Store whether gravity was enabled before we get picked up
            }
            else
            {
                GetComponent<Rigidbody>().useGravity = wasGravityEnabled; // Restore gravity if it was enabled before pickup
                targetPawn.physObjectAttachmentPoint.gameObject.GetComponent<FixedJoint>().connectedBody = null;
                targetPawn.physObjectAttachmentPoint.GetComponent<Pawn_AttachmentPoint>().heldObject = null;
            }
            ToggleHeld();
            is2D = false;
        }
    }
    
    public Vector2 Get2DFaceDirectionFromQuaternion(Quaternion _rotation, int _zRotationOffset)
    {
        // Extract the z-axis rotation from the Quaternion
        float angle = _rotation.eulerAngles.z - _zRotationOffset;

        // Convert the angle back to radians since Unity's trig functions expect radians
        float angleInRadians = angle * Mathf.Deg2Rad;

        // Calculate the x and y components of the facing direction
        float x = Mathf.Cos(angleInRadians);
        float y = Mathf.Sin(angleInRadians);

        // Create and return the facing direction vector
        Vector2 facingDirection = new Vector2(x, y);

        return facingDirection;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void ToggleHeld()
    {
        isHeld = !isHeld;
        if (!isHeld)
        {
            GetComponent<Rigidbody>().useGravity = wasGravityEnabled; // Restore gravity if it was enabled before pickup
            targetPawn.physObjectAttachmentPoint.GetComponent<Pawn_AttachmentPoint>().heldObject = null;
            targetPawn.physObjectAttachmentPoint.gameObject.GetComponent<FixedJoint>().connectedBody = null;
        }
    }
}
