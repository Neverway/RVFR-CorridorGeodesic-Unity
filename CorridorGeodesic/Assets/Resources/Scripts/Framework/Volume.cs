//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [Tooltip("If enabled, will remove objects from the list of objects in the volume when that object becomes disabled")]
    public bool disabledObjectsExitVolume = true;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [DebugReadOnly] public List<Pawn> pawnsInTrigger = new List<Pawn>();
    [DebugReadOnly] public List<GameObject> propsInTrigger = new List<GameObject>();
    [HideInInspector] public Pawn targetEntity;
    [HideInInspector] public GameObject targetProp;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    protected void Update()
    {
        //Validate the pawns that are inside the trigger (if there are any)
        if (pawnsInTrigger.Count > 0)
        {
            List<Pawn> toRemove = new List<Pawn>();
            for (int i = pawnsInTrigger.Count - 1; i >= 0; i--)
            {
                //If object is null, or, if object is disabled and disabledObjectsExitVolume is checked...
                if (pawnsInTrigger[i] == null || (disabledObjectsExitVolume && !pawnsInTrigger[i].gameObject.activeInHierarchy))
                    //... Add this object to the list of objects to remove (may be null)
                    toRemove.Add(pawnsInTrigger[i]);
            }
            //Remove the pawns in toRemove from the volume (may contain some null values)
            foreach (Pawn pawn in toRemove)
                RemovePawnFromVolume(pawn);
        }

        //Validate the props that are inside the trigger (if there are any)
        if (propsInTrigger.Count > 0)
        {
            List<GameObject> toRemove = new List<GameObject>();
            for (int i = propsInTrigger.Count - 1; i >= 0; i--)
            {
                //If object is null, or, if object is disabled and disabledObjectsExitVolume is checked...
                if (propsInTrigger[i] == null || (disabledObjectsExitVolume && !propsInTrigger[i].activeInHierarchy))
                    //... Add this object to the list of objects to remove (may be null)
                    toRemove.Add(propsInTrigger[i]);
            }
            //Remove the props in toRemove from the volume (may contain some null values)
            foreach (GameObject prop in toRemove)
                RemovePropFromVolume(prop);
        }
    }
    protected void OnTriggerEnter2D(Collider2D _other)
    {
        // An Pawn has entered the trigger
        if (_other.CompareTag("Pawn"))
        {
            // Get a reference to the entity component
            targetEntity = _other.gameObject.transform.parent.GetComponent<Pawn>();
            // Exit if they are not on the effected team
            if (!IsOnAffectedTeam(targetEntity)) return;
            // Add the entity to the list if they are not already present
            AddPawnToVolume(targetEntity);
        }
        
        // A physics prop has entered the trigger
        if (_other.CompareTag("PhysProp"))
        {
            // Don't register held objects
            if (_other.gameObject.transform.parent.GetComponent<Object_Grabbable>()) { if (_other.gameObject.transform.parent.GetComponent<Object_Grabbable>().isHeld) { return; } }
            // Get a reference to the entity component
            targetProp = _other.gameObject.transform.parent.gameObject;
            // Add the entity to the list if they are not already present
            AddPropToVolume(targetProp);
        }
    }

    protected void OnTriggerExit2D(Collider2D _other)
    {
        // An Pawn has exited the trigger
        if (_other.CompareTag("Pawn"))
        {
            // Get a reference to the entity component
            targetEntity = _other.gameObject.transform.parent.GetComponent<Pawn>();
            // Remove the entity to the list if they are not already absent
            RemovePawnFromVolume(targetEntity);
        }
        
        // A physics prop has entered the trigger
        if (_other.CompareTag("PhysProp"))
        {
            // Get a reference to the entity component
            targetProp = _other.gameObject.transform.parent.gameObject;
            // Add the entity to the list if they are not already present
            RemovePropFromVolume(targetProp);
        }
    }
    protected void OnTriggerEnter(Collider _other)
    {
        // An Pawn has entered the trigger
        if (_other.CompareTag("Pawn"))
        {
            // Get a reference to the entity component
            targetEntity = _other.gameObject.GetComponent<Pawn>();
            // Exit if they are not on the effected team
            if (!IsOnAffectedTeam(targetEntity)) return;
            // Add the entity to the list if they are not already present
            AddPawnToVolume(targetEntity);
        }
        
        // A physics prop has entered the trigger
        if (_other.CompareTag("PhysProp"))
        {
            // Don't register held objects
            if (_other.gameObject.GetComponent<Object_Grabbable>() && ignoreHeldObjects) { if (_other.gameObject.GetComponent<Object_Grabbable>().isHeld) { return; } }
            // Get a reference to the entity component
            targetProp = _other.gameObject;
            // Add the entity to the list if they are not already present
            AddPropToVolume(targetProp);
        }
    }

    protected void OnTriggerExit(Collider _other)
    {
        // An Pawn has exited the trigger
        if (_other.CompareTag("Pawn"))
        {
            // Get a reference to the entity component
            targetEntity = _other.gameObject.GetComponent<Pawn>();
            // Remove the entity to the list if they are not already absent
            RemovePawnFromVolume(targetEntity);
        }
        
        // A physics prop has entered the trigger
        if (_other.CompareTag("PhysProp"))
        {
            // Get a reference to the entity component
            targetProp = _other.gameObject;
            // Add the entity to the list if they are not already present
            RemovePropFromVolume(targetProp);
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

    /// <summary>
    /// Call this to add a Pawn to the volume. Override it if you want to extend the logic for adding Pawns to volumes
    /// </summary>
    /// <param name="pawn"> Pawn to add to the volume</param>
    /// <returns> Whether or not the Pawn was successfully added </returns>
    protected virtual bool AddPawnToVolume(Pawn pawn) 
    {
        //Cannot add null pawns
        if (pawn == null)
            return false;

        //Add pawn to pawnsInTrigger if it isn't already in the list
        if (!pawnsInTrigger.Contains(pawn))
        {
            pawnsInTrigger.Add(pawn);
            OnObjectsInVolumeUpdated(VolumeUpdateType.PawnAdded);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Call this to remove a Pawn from the volume. Override it if you want to extend the logic for removing Pawns from volumes
    /// </summary>
    /// <param name="pawn"> Pawn to remove from the volume</param>
    /// <returns> Whether or not the Pawn was successfully removed </returns>
    protected virtual bool RemovePawnFromVolume(Pawn pawn) 
    {
        //Cannot remove null pawns
        if (pawn == null)
            return false;

        //Remove pawn from pawnsInTrigger if its in the list
        if (pawnsInTrigger.Contains(pawn))
        {
            pawnsInTrigger.Remove(pawn);
            OnObjectsInVolumeUpdated(VolumeUpdateType.PawnRemoved);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Call this to add a prop to the volume. Override it if you want to extend the logic for adding props to volumes
    /// </summary>
    /// <param name="prop"> Prop to add to the volume</param>
    /// <returns> Whether or not the prop was successfully added </returns>
    protected virtual bool AddPropToVolume(GameObject prop) 
    {
        //Cannot add null props
        if (prop == null)
            return false;

        //Add prop to propsInTrigger if it isn't already in the list
        if (!propsInTrigger.Contains(prop))
        {
            propsInTrigger.Add(prop);
            OnObjectsInVolumeUpdated(VolumeUpdateType.PropAdded);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Call this to remove a prop from the volume. Override it if you want to extend the logic for removing props from volumes
    /// </summary>
    /// <param name="prop"> Prop to remove from the volume</param>
    /// <returns> Whether or not the prop was successfully removed </returns>
    protected virtual bool RemovePropFromVolume(GameObject prop) 
    {
        //Cannot remove null props
        if (prop == null)
            return false;

        //Remove prop from propsInTrigger if its in the list
        if (propsInTrigger.Contains(prop))
        {
            propsInTrigger.Remove(prop);
            OnObjectsInVolumeUpdated(VolumeUpdateType.PropRemoved);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Override this method in extended classes to react when any change to the volume contents occurs
    /// </summary>
    /// <param name="updateType">flags for what kind of update the volume was</param>
    // Example: To check if Props were updated:
    //      (updateType & VolumeUpdateType.PropAdded) != 0
    protected virtual void OnObjectsInVolumeUpdated(VolumeUpdateType updateType) { }

    /// <summary>
    /// Used to filter different types of updates from OnObjectsInVolumeUpdated callback
    /// </summary>
    [System.Flags]
    public enum VolumeUpdateType
    { 
        NoUpdate      = 0,
        PawnAdded     = 1 << 0,
        PawnRemoved   = 1 << 1,
        PropAdded     = 1 << 2,
        PropRemoved   = 1 << 3,

        AnythingAdded = PawnAdded | PropAdded,
        AnythingRemoved = PawnRemoved | PropRemoved,
        
        PawnUpdated = PawnAdded | PawnRemoved,
        PropUpdated = PropAdded | PropRemoved,

        AnyUpdate = PawnUpdated | PropUpdated
    }

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

    //=-----------------=
    // External Functions
    //=-----------------=
}
