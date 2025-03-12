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

public class Func_JoinTeam : MonoBehaviour
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
        if (other.GetComponent<NetDummy>())
        {
            print("Joined");
            other.GetComponent<NetDummy>().SetTeam(team);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
