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
    private Rigidbody propRigidbody;
    [SerializeField] private LayerMask layerMask;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        if (transform.childCount == 0) return;
        interactableTrigger = transform.GetChild(0).GetComponent<Volume_TriggerInteractable>();
        propRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
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
            var direction = targetPawn.physObjectAttachmentPoint.transform.position - gameObject.transform.position;
            var distance = Vector3.Distance(targetPawn.physObjectAttachmentPoint.transform.position, gameObject.transform.position);
            Vector3 targetPosition;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, layerMask))
            {

                targetPosition = (hit.point - (direction.normalized * 0.5f));
            }
            else
            {
                targetPosition = (targetPawn.physObjectAttachmentPoint.transform.position);
            }
            propRigidbody.velocity = Vector3.zero;
            propRigidbody.MovePosition (targetPosition);
            
            var targetRotation = targetPawn.physObjectAttachmentPoint.transform.rotation;
            transform.rotation = new Quaternion(targetRotation.x, targetRotation.y, targetRotation.z, targetRotation.w);
            
            //propRigidbody.velocity = new Vector3();
            propRigidbody.useGravity = false;
            
            // Drop the object if it's too far away
            if (Vector3.Distance(gameObject.transform.position,
                    targetPawn.physObjectAttachmentPoint.transform.position) > 2)
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
        propRigidbody = GetComponent<Rigidbody>();
        var interaction = _other.GetComponent<Volume_TriggerInteraction>();
        if (interaction)
        {
            targetPawn = interaction.targetPawn;
            if (!isHeld)
            {
                // If this isn't being held, and the pawn is already holding something, exit
                if (targetPawn.physObjectAttachmentPoint.heldObject!=null)
                {
                    return;
                }
                targetPawn.physObjectAttachmentPoint.heldObject = gameObject;
                wasGravityEnabled = propRigidbody.useGravity; // Store whether gravity was enabled before we get picked up
                // Lerp to the position of the object to go to the position of the holding point
            }
            else
            {
                propRigidbody.useGravity = wasGravityEnabled; // Restore gravity if it was enabled before pickup
                targetPawn.physObjectAttachmentPoint.heldObject = null;
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
            propRigidbody.useGravity = wasGravityEnabled; // Restore gravity if it was enabled before pickup
            targetPawn.physObjectAttachmentPoint.GetComponent<Pawn_AttachmentPoint>().heldObject = null;
        }
    }

    public void Drop ()
    {
        if (isHeld)
        {
            ToggleHeld ();
        }
    }
}
