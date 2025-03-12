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

public class NetDumSync : NetworkBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public readonly SyncVar<string> team;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            GetComponent<NetDumSync>().enabled = false;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpdateTeam(this,("0"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpdateTeam(this,("1"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UpdateTeam(this, ("2"));
        }
    }

    [ServerRpc]
    public void UpdateTeam(NetDumSync _script, string _team)
    {
        _script.team.Value = new SyncVar<string>(_team).Value;
        _script.gameObject.GetComponent<NetDummy>().SetTeam(_team);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
