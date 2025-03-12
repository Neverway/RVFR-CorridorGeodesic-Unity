//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Component.Transforming;
using FishNet.Object;
using UnityEngine;

public class NetOwnerTriggerTransfer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public NetworkTransform netObject;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
    }

    private void Update()
    {
        print($"{netObject.GetComponent<NetworkTransform>().TakenOwnership} " + $"{netObject.GetComponent<NetworkTransform>().Owner} ");
    }

    private void OnTriggerEnter(Collider other)
    {
        var netTarget = other.GetComponent<NetworkObject>();
        if (netTarget && !netTarget.CompareTag("PhysProp"))
        {
            print(other.name);
            print(netObject.IsOwner);
            print(netObject.Owner);
            netObject.GetComponent<NetworkTransform>().RemoveOwnership();
            netObject.GetComponent<NetworkTransform>().GiveOwnership(netTarget.Owner);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
