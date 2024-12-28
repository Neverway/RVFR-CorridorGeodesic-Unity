//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Attach this to the foot "steppers" of an IK rig to allow the feet
//  to dynamically match the environment
// Notes: The foot targets themselves need a IKTargetMatcher with a reference
//  to this object
// References: https://www.youtube.com/watch?v=acMK93A-FSY
//              https://www.youtube.com/watch?v=Wx1s3CJ8NHw
//              https://youtu.be/FXhjhlNvvfw
//
//=============================================================================

using System.Reflection;
using UnityEngine;
using Neverway.Framework;
using Unity.Collections;
using UnityEditor;

namespace Neverway
{
public class IKFootSolver : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Header("Positioning")]
    [Tooltip("How far away from the 'body' to shift this target to the right (negative moves left)")]
    [SerializeField] private float footSpacing;
    [Tooltip("How far away from the 'body' to shift this target forward (negative moves back)")]
    [SerializeField] private float forwardSpacing;
    [Tooltip("How far up to shift the raycast from the 'body' origin")]
    [SerializeField] private float raycastOffset;
    [Tooltip("How far down to fire the raycast from the 'body' origin")]
    [SerializeField] private float raycastDistance;
    [Tooltip("What layers can trigger raycasts for the feet positions")]
    [SerializeField] private LayerMask layerMask;
    [Tooltip("Offset for the final position to place the targets")]
    [SerializeField] private Vector3 placementOffset;

    [Header("Movement")] 
    [Tooltip("")] 
    [SerializeField] private float movementThreshold;
    [Tooltip("How far the target can move before the leg updates it's position")]
    [SerializeField] private float stepDistance;
    [Tooltip("How high to bend the legs when taking a step")]
    [SerializeField] private float stepHeight;
    [Tooltip("How quickly to move the leg when taking a step")]
    [SerializeField] private float stepSpeed;
    [Tooltip("How much the current movement direction will affect the offset of the desired foot position")]
    [SerializeField] private float moveDirectionMultiplier=1;
    [Tooltip("If there is only one foot, or you want the legs to stay in sync, enable this")]
    [SerializeField] private bool disableWaitForFoot;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector3 oldPosition, currentPosition, newPosition;
    private Vector3 oldRotation, currentRotation, newRotation;
    private float stepTimeLerp;
    private Vector3 lastBodyPosition, currentBodyPosition;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [Header("References")]
    [SerializeField] private Transform body;
    [Tooltip("Used to make sure legs don't step at the same time")]
    [SerializeField] private IKFootSolver otherFoot;
    [ReadOnly][SerializeField] private Vector3 moveDirection;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        oldPosition = transform.position;
        currentPosition = transform.position;
        newPosition = transform.position;
        
        oldRotation = transform.up;
        currentRotation = transform.up;
        newRotation = transform.up;
    }

    private void Update()
    {
        // Update position and normal
        transform.position = currentPosition;
        transform.up = currentRotation;
        
        // Get the direction the entity is moving in, so we can use that value to place the feet ahead of where we are traveling
        CalculateMovementDirectionInfluence();
        
        // Update the new target position
        Ray ray = new Ray(body.position + (body.right * footSpacing) + (body.forward * forwardSpacing) + (body.up * raycastOffset), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, layerMask))
        {
            // If the current foot position is too far from our new target, move the foot
            if (Vector3.Distance(currentPosition, hit.point+moveDirection) >= stepDistance && stepTimeLerp >= 1)
            {
                if (!otherFoot.IsMoving() || disableWaitForFoot)
                {
                    stepTimeLerp = 0;
                    UpdateFootPosition(hit);
                }
            }
        
            // Swing the leg to the new point if it's moving
            if (stepTimeLerp < 1)
            {
                Vector3 swingingPosition = Vector3.Lerp(oldPosition, newPosition, stepTimeLerp);
                swingingPosition.y += Mathf.Sin(stepTimeLerp * Mathf.PI) * stepHeight;
                currentPosition = swingingPosition;
                currentRotation = Vector3.Lerp(oldRotation, newRotation, stepTimeLerp);
                stepTimeLerp += Time.deltaTime * stepSpeed;
            }
            else
            {
                oldPosition = newPosition;
                oldRotation = newRotation;
            }

            // Update the position the calculated current position
            transform.position = currentPosition;
            lastBodyPosition = body.position;
        }
        else
        {
            stepTimeLerp = 1;
            transform.position = body.position + (body.right * footSpacing) + (body.forward * forwardSpacing) + placementOffset;
            lastBodyPosition = body.position;
            oldPosition = transform.position;
            currentPosition = transform.position;
            newPosition = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.1f); // Target foot position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(body.position, 0.1f); // Body position
        Gizmos.color = Color.green;
        Gizmos.DrawLine(body.position, body.position + body.forward * forwardSpacing); // Forward direction
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void UpdateFootPosition(RaycastHit _hit)
    {
        newPosition = _hit.point+moveDirection;
        newRotation = _hit.normal;
    }

    private void CalculateMovementDirectionInfluence()
    {
        // Reset the moveDirection value
        moveDirection = Vector3.zero;
        
        Vector3 initialMovementDirection = (body.position - lastBodyPosition);
        
        // Use a threshold value to determine if the entity is actually moving somewhere, or just repositioning
        if (initialMovementDirection.x >= movementThreshold ||
            initialMovementDirection.z >= movementThreshold ||
            initialMovementDirection.x <= -movementThreshold ||
            initialMovementDirection.z <= -movementThreshold)
        {
            // We have moved a significant amount and should apply the movement offset
            moveDirection = initialMovementDirection * moveDirectionMultiplier + placementOffset;
        }
        else
        {
            moveDirection = placementOffset;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public bool IsMoving()
    {
        return stepTimeLerp < 1;
    }
}
}
