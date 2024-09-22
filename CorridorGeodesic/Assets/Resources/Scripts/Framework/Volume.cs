//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("Depending on which team owns this trigger will change the functionality. For example, pain volumes normally don't affect their own team.")]
    public string owningTeam; // Which team owns the trigger
    [Tooltip("If enabled, the trigger will only affect targets that are on the same team")]
    public bool affectsOwnTeam; // If true, the trigger will only affect objects that are a part of the owning team
    [Tooltip("If enabled, physics props that are being held won't be effected by the volume (This is used for things like wind boxes)")]
    public bool ignoreHeldObjects = true;
    

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [HideInInspector] public List<Pawn> pawnsInTrigger = new List<Pawn>();
    [HideInInspector] public List<GameObject> propsInTrigger = new List<GameObject>();
    [HideInInspector] public Pawn targetEnt;
    [HideInInspector] public GameObject targetProp;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    protected void OnTriggerEnter2D(Collider2D _other)
    {
        // An Pawn has entered the trigger
        if (_other.CompareTag("Pawn"))
        {
            // Get a reference to the entity component
            targetEnt = _other.gameObject.transform.parent.GetComponent<Pawn>();
            // Exit if they are not on the effected team
            if (!IsOnAffectedTeam(targetEnt)) return;
            // Add the entity to the list if they are not already present
            if (!pawnsInTrigger.Contains(targetEnt))
            {
                pawnsInTrigger.Add(targetEnt);
            }
        }
        
        // A physics prop has entered the trigger
        if (_other.CompareTag("PhysProp"))
        {
            // Don't register held objects
            if (_other.gameObject.transform.parent.GetComponent<Object_Grabbable>()) { if (_other.gameObject.transform.parent.GetComponent<Object_Grabbable>().isHeld) { return; } }
            // Get a reference to the entity component
            targetProp = _other.gameObject.transform.parent.gameObject;
            // Add the entity to the list if they are not already present
            if (!propsInTrigger.Contains(targetProp))
            {
                propsInTrigger.Add(targetProp);
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D _other)
    {
        // An Pawn has exited the trigger
        if (_other.CompareTag("Pawn"))
        {
            // Get a reference to the entity component
            targetEnt = _other.gameObject.transform.parent.GetComponent<Pawn>();
            // Remove the entity to the list if they are not already absent
            if(pawnsInTrigger.Contains(targetEnt)) { pawnsInTrigger.Remove(targetEnt); }
        }
        
        // A physics prop has entered the trigger
        if (_other.CompareTag("PhysProp"))
        {
            // Get a reference to the entity component
            targetProp = _other.gameObject.transform.parent.gameObject;
            // Add the entity to the list if they are not already present
            if(propsInTrigger.Contains(targetProp)) { propsInTrigger.Remove(targetProp); }
        }
    }
    protected void OnTriggerEnter(Collider _other)
    {
        // An Pawn has entered the trigger
        if (_other.CompareTag("Pawn"))
        {
            // Get a reference to the entity component
            targetEnt = _other.gameObject.GetComponent<Pawn>();
            // Exit if they are not on the effected team
            if (!IsOnAffectedTeam(targetEnt)) return;
            // Add the entity to the list if they are not already present
            if (!pawnsInTrigger.Contains(targetEnt))
            {
                pawnsInTrigger.Add(targetEnt);
            }
        }
        
        // A physics prop has entered the trigger
        if (_other.CompareTag("PhysProp"))
        {
            // Don't register held objects
            if (_other.gameObject.GetComponent<Object_Grabbable>() && ignoreHeldObjects) { if (_other.gameObject.GetComponent<Object_Grabbable>().isHeld) { return; } }
            // Get a reference to the entity component
            targetProp = _other.gameObject;
            // Add the entity to the list if they are not already present
            if (!propsInTrigger.Contains(targetProp))
            {
                propsInTrigger.Add(targetProp);
            }
        }
    }

    protected void OnTriggerExit(Collider _other)
    {
        // An Pawn has exited the trigger
        if (_other.CompareTag("Pawn"))
        {
            // Get a reference to the entity component
            targetEnt = _other.gameObject.GetComponent<Pawn>();
            // Remove the entity to the list if they are not already absent
            if(pawnsInTrigger.Contains(targetEnt)) { pawnsInTrigger.Remove(targetEnt); }
        }
        
        // A physics prop has entered the trigger
        if (_other.CompareTag("PhysProp"))
        {
            // Get a reference to the entity component
            targetProp = _other.gameObject;
            // Add the entity to the list if they are not already present
            if(propsInTrigger.Contains(targetProp)) { propsInTrigger.Remove(targetProp); }
        }
    }

    public virtual void OnDisable ()
    {
        pawnsInTrigger.Clear();
        propsInTrigger.Clear();
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private bool IsOnAffectedTeam(Pawn _targetPawn)
    {
        // If an owning team is specified
        if (owningTeam != "")
        {
            // If targeting team
            if (affectsOwnTeam)
            {
                // Return if target is a part of team
                return _targetPawn.currentState.team == owningTeam;
            }
            // If targeting non-team
            // Return if target is not a part of team
            return _targetPawn.currentState.team != owningTeam;
        }
        
        // Owning team wasn't specified, so result is that all are affected
        return true;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    protected Pawn GetPlayerInTrigger()
    {
        foreach (var entity in pawnsInTrigger)
        {
            if (entity.isPossessed)
            {
                return entity;
            }
        }
        return null;
    }
}
