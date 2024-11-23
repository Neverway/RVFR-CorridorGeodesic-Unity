//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using Neverway.Framework.LogicSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume_NEW : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] protected bool resetsAutomatically;
    [SerializeField] protected VolumeActivationType volumeActivationType;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public virtual void OnTriggerEnter(Collider other)
    {
        if (volumeActivationType == VolumeActivationType.None)
            return;

        bool playerActivate = volumeActivationType == VolumeActivationType.Pawn;
        bool physActivate = volumeActivationType == VolumeActivationType.PhysProp;
        bool playerPhysActivate = volumeActivationType.Equals(VolumeActivationType.Pawn | VolumeActivationType.PhysProp);

        if ((playerActivate || playerPhysActivate) && other.CompareTag("Pawn"))
        {
            OnPawnEntered(other);
        }
        else if((physActivate || playerPhysActivate) && other.CompareTag("PhysProp"))
        {
            OnPhysEntered(other);
        }

        if(!resetsAutomatically)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public virtual void OnPawnEntered(Collider other)
    {

    }
    public virtual void OnPhysEntered(Collider other)
    {

    }

    //=-----------------=
    // External Functions
    //=-----------------=
}