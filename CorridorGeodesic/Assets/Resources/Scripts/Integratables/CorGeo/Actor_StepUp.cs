//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Actor_StepUp : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("The maximum a player can set upwards in units when they hit a wall that's potentially a step")]
    public float maxStepHeight = 0.9f;

    [Tooltip(
        "How much to overshoot into the direction a potential step in units when testing. High values prevent player from walking up tiny steps but may cause problems.")]
    public float stepSearchOvershoot = 0.005f;

    public float additionalStepUpOffset=1;
    public float stephieghtvar1 = 0.00001f;
    public float steptestvar = 0.00000001f;
    public float anglecheck = 0.0001f;


    
    //=-----------------=
    // Private Variables
    //=-----------------=
    public List<ContactPoint> contactPoints = new List<ContactPoint>();
    public Vector3 lastVelocity;

    
    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void FixedUpdate()
    {
        Vector3 velocity = this.GetComponent<Rigidbody>().velocity;
        
        //Filter through the ContactPoints to see if we're grounded and to see if we can step up
        ContactPoint groundCP = default(ContactPoint);
        bool grounded = FindGround(out groundCP, contactPoints);
        
        Vector3 stepUpOffset = default(Vector3);
        bool stepUp = false;
        if (grounded)
        {
            stepUp = FindStep(out stepUpOffset, contactPoints, groundCP, velocity);
        }
        
        //Steps
        if(stepUp)
        {
            //print("Attempting stepup");
            this.GetComponent<Rigidbody>().position += stepUpOffset*additionalStepUpOffset;
            this.GetComponent<Rigidbody>().velocity = lastVelocity;
        }

        foreach (var contact in contactPoints)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.cyan, 1);
        }
        
        contactPoints.Clear();
        lastVelocity = velocity;
    }
    
    private void OnCollisionEnter(Collision col)
    {
        contactPoints.AddRange(col.contacts);
    }
 
    private void OnCollisionStay(Collision col)
    {
        contactPoints.AddRange(col.contacts);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    /// Finds the MOST grounded (flattest y component) ContactPoint
    /// \param contactPoints List to search
    /// \param groundCP The contact point with the ground
    /// \return If grounded
    private bool FindGround(out ContactPoint groundCP, List<ContactPoint> contactPoints)
    {
        groundCP = default(ContactPoint);
        bool found = false;
        foreach(ContactPoint cp in contactPoints)
        {   
            //Pointing with some up direction
            if(cp.normal.y >= anglecheck && (found == false || cp.normal.y > groundCP.normal.y))
            {
                groundCP = cp;
                found = true;
            }
        }
        
        return found;
    }
    /// Find the first step up point if we hit a step
    /// \param contactPoints List to search
    /// \param stepUpOffset A Vector3 of the offset of the player to step up the step
    /// \return If we found a step
    private bool FindStep(out Vector3 stepUpOffset, List<ContactPoint> contactPoints, ContactPoint groundCP, Vector3 currVelocity)
    {
        stepUpOffset = default(Vector3);
        //print("FindStep 0");
        //No chance to step if the player is not moving
        Vector2 velocityXZ = new Vector2(currVelocity.x, currVelocity.z);
        if (velocityXZ.sqrMagnitude < 0.0001f)
        {
            //print("FindStep BRK1");
            //return false;
        }
        
        foreach(ContactPoint cp in contactPoints)
        {
            bool test = ResolveStepUp(out stepUpOffset, cp, groundCP);
            if (test)
            {
                //print("FindStep 1");
                return test;
            }
        }
        //print("FindStep BRK2");
        return false;
    }
    
    /// Takes a contact point that looks as though it's the side face of a step and sees if we can climb it
    /// \param stepTestCP ContactPoint to check.
    /// \param groundCP ContactPoint on the ground.
    /// \param stepUpOffset The offset from the stepTestCP.point to the stepUpPoint (to add to the player's position so they're now on the step)
    /// \return If the passed ContactPoint was a step
    private bool ResolveStepUp(out Vector3 stepUpOffset, ContactPoint stepTestCP, ContactPoint groundCP)
    {
        stepUpOffset = default(Vector3);
        Collider stepCol = stepTestCP.otherCollider;
        //print("ResolveStepUp 0");
        
        //( 1 ) Check if the contact point normal matches that of a step (y close to 0)
        if(Mathf.Abs(stepTestCP.normal.y) >= steptestvar)
        {
            //print("Contact point was not flat enough");
            return false;
        }
        //print("Found a contact point that could be a step");
        
        //( 2 ) Make sure the contact point is low enough to be a step
        if (stepTestCP.point.y - groundCP.point.y > maxStepHeight)
        {
            //print("Step distance was too high to step up");
            return false;
        }
        
        //( 3 ) Check to see if there's actually a place to step in front of us
        //Fires one Raycast
        RaycastHit hitInfo;
        float stepHeight = groundCP.point.y + maxStepHeight + stephieghtvar1;
        Vector3 stepTestInvDir = new Vector3(-stepTestCP.normal.x, 0, -stepTestCP.normal.z).normalized;
        Vector3 origin = new Vector3(stepTestCP.point.x, stepHeight, stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
        Vector3 direction = Vector3.down;
        Debug.DrawRay(origin, direction, Color.red);/*
        Debug.Log("Step Test Contact Point: " + stepTestCP.point);
        Debug.Log("Ground Contact Point: " + groundCP.point);
        Debug.Log("Calculated Step Height: " + stepHeight);
        Debug.Log("Ray Origin: " + origin);
        Debug.Log("Ray Direction: " + direction);
        Debug.Log("Max Step Height: " + maxStepHeight);*/

        Ray ray = new Ray(origin, direction);
        

        if (!stepCol) return false;
        if (!stepCol.Raycast(ray, out hitInfo, maxStepHeight))
        {
            return false;
        }
        else
        {
            //Debug.Log("Raycast Succeeded - Hit Point: " + hitInfo.point);
        }
        
        //We have enough info to calculate the points
        Vector3 stepUpPoint = new Vector3(stepTestCP.point.x, hitInfo.point.y+0.1f, stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
        Vector3 stepUpPointOffset = stepUpPoint - new Vector3(stepTestCP.point.x, groundCP.point.y, stepTestCP.point.z);
        
        //We passed all the checks! Calculate and return the point!
        stepUpOffset = stepUpPointOffset;
        //print("ResolveStepUp 1");
        return true;
    }



    //=-----------------=
    // External Functions
    //=-----------------=
}
