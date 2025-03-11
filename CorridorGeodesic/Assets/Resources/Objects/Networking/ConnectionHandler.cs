//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Right now this just makes sure the host gets a netdummy assigned
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using Neverway.Framework.PawnManagement;

public class ConnectionHandler : NetworkBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public GameObject playerPrefab;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void SpawnHostDummy()
    {
        var hostDummy = Instantiate(playerPrefab);
        hostDummy.GetComponent<NetDummy>().isHost = true;
        hostDummy.GetComponent<NetDummy>().parentPawn = FindObjectOfType<GameInstance>().localPlayerCharacter.gameObject;
        hostDummy.GetComponent<NetDummy>().initDummy();
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
