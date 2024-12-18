//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Attach this to the foot targets of an IK rig to allow the feet
//  to dynamically match the environment
// Notes:
// References: https://www.youtube.com/watch?v=acMK93A-FSY
//              https://www.youtube.com/watch?v=Wx1s3CJ8NHw
//              https://youtu.be/FXhjhlNvvfw
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Neverway
{
public class IKFootSolver : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private float footSpacing;
    [SerializeField] private float raycastOffset;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float stepDistance;
    [SerializeField] private float stepFarRange;
    [SerializeField] private float stepLength;
    [SerializeField] private float stepHeight;
    [SerializeField] private float stepSpeed;
    [SerializeField] private Vector3 footOffset;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector3 oldPosition, currentPosition, newPosition;
    private Vector3 oldRotation, currentRotation, newRotation;
    private float lerp;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Transform body;
    [SerializeField] private Transform target;
    [SerializeField] private IKFootSolver otherFoot;


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
        
        // Update the new target position
        Ray ray = new Ray(body.position + (body.right * footSpacing) + (body.up * raycastOffset), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, layerMask))
        {
            // If the current foot position is too far from our new target, move the foot
            if (Vector3.Distance(newPosition, hit.point) >= stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                lerp = 0;
                UpdateFootPosition(hit);
            }
            // If the distance is too far, force the position to update
            /*if (Vector3.Distance(newPosition, hit.point) >= stepFarRange)
            {
                lerp = 1;
                UpdateFootPosition(hit);
                currentPosition = newPosition;
            }*/
        }
        // Swing the leg to the new point if it's moving
        if (lerp < 1)
        {
            Vector3 swingingPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            swingingPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            currentPosition = swingingPosition;
            currentRotation = Vector3.Lerp(oldRotation, newRotation, lerp);
            lerp += Time.deltaTime * stepSpeed;
        }
        else
        {
            oldPosition = newPosition;
            oldRotation = newRotation;
        }

        // Update the position the calculated current position
        transform.position = currentPosition;
    }

    private void OnDrawGizmos()
    {
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void UpdateFootPosition(RaycastHit _hit)
    {
        //fullSwingDistance = Vector3.Distance(newPosition, _hit.point);
        // Get the direction to swing the leg
        int direction;
        if (body.InverseTransformPoint(_hit.point).z > body.InverseTransformPoint(newPosition).z)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        Debug.DrawLine(body.position + (body.right * footSpacing) + (body.up * raycastOffset), _hit.point + (body.forward * stepLength * direction) + footOffset, Color.green, 0.25f);
        newPosition = _hit.point + (body.forward * stepLength * direction) + footOffset;
        newRotation = _hit.normal;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public bool IsMoving()
    {
        return lerp < 1;
    }
}
}
