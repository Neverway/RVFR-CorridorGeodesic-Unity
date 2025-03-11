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
using FishNet.Connection;
using FishNet.Object;
using Neverway.Framework.PawnManagement;

public class NetDummy : NetworkBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public bool isHost;
    public GameObject parentPawn;
    public bool localControl;
    public GameObject hiddenWhenLocallyControlled;


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
        if (isHost) return;
        base.OnStartClient();
        if (base.IsOwner)
        {
            initDummy();
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void Update()
    {
        if (localControl && parentPawn || isHost && parentPawn)
        {
            gameObject.transform.position = parentPawn.transform.position;
            gameObject.transform.rotation = parentPawn.transform.rotation;
        }
        else if (localControl && !parentPawn || isHost && !parentPawn)
        {
            initDummy();
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void initDummy()
    {
        localControl = true;
        hiddenWhenLocallyControlled.SetActive(false);
        parentPawn = FindObjectOfType<GameInstance>().localPlayerCharacter.gameObject;
    }
}
