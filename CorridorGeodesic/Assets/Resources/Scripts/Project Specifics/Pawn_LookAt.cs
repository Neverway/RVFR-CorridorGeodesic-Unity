//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Attached to a bone on a rig to make a pawn look towards a target point
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pawn_LookAt : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("The distance from ourselves to check for targets")]
    [SerializeField] private Vector2 targetRange;
    
    [SerializeField] private bool xRotation, yRotation, zRotation;
    //[SerializeField] private Vector2 xRotationRange, yRotationRange, zRotationRange;
    [SerializeField] private Transform rotatingTarget;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private Vector3 rotationOffset;
    //=-----------------=
    // Private Variables
    //=-----------------=
    /*
    [Tooltip("NearestActor includes phys props and pawns")]
    private enum TargetType
    {
        PlayerPawn, 
        NearestPawn, 
        NearestActor, 
        None
    }
    [SerializeField] private TargetType targetType;
    [Tooltip("If the tracked object has stopped moving, how long until we loose interest and stop looking at it (set to 0 to disable)")]
    [SerializeField] private float attentionSpan = 0;
    [Tooltip("How quickly we look at our target")]
    [SerializeField] private float rotationSpeed = 1;*/

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        UpdateTargets();
        UpdateRotations();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void UpdateTargets()
    {
        float closestDistance = -1;
        foreach (var movableObject in FindObjectsOfType<Pawn>())
        {
            var distanceToPawn = Vector3.Distance(transform.position, movableObject.transform.position);
            if (distanceToPawn >= targetRange.x && distanceToPawn <= targetRange.y)
            {
                if (distanceToPawn < closestDistance || closestDistance == -1)
                {
                    closestDistance = distanceToPawn;
                    currentTarget = movableObject.transform;
                }
            }
        }
    }

    private void UpdateRotations()
    {
        if (currentTarget == null) return;
        
        // Store the current rotation
        Vector3 storedRotation = rotatingTarget.rotation.eulerAngles;
        
        // Manual LookAt function
        Vector3 lookPos = currentTarget.position - rotatingTarget.position;
        Quaternion lookRot = Quaternion.LookRotation(lookPos, Vector3.up);
        
        
        rotatingTarget.rotation = lookRot;
        
        // Add the offset
        rotatingTarget.rotation *= Quaternion.Euler(rotationOffset);
        
        // Restore locked axis so they aren't modified
        Vector3 currentRotation = rotatingTarget.rotation.eulerAngles;
        if (!xRotation) rotatingTarget.rotation = Quaternion.Euler(storedRotation.x, currentRotation.y, currentRotation.z);
        if (!yRotation) rotatingTarget.rotation = Quaternion.Euler(currentRotation.x, storedRotation.y, currentRotation.z);
        if (!zRotation) rotatingTarget.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, storedRotation.z);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
