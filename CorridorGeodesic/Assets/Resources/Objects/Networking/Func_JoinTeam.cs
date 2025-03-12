//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class Func_JoinTeam : NetworkBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string team;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        print($"Detected {other.name}");
        if (other.GetComponent<NetDumSync>())
        {
            print("Joined");
            other.GetComponent<NetDumSync>().UpdateTeam(other.GetComponent<NetDumSync>(), team);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
