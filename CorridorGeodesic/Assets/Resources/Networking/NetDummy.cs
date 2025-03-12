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
    public Vector3 offset;


    //=-----------------=
    // Private Variables
    //=-----------------=
    public List<GameObject> teamZeroObjects;
    public List<GameObject> teamOneObjects;
    public List<GameObject> teamTwoObjects;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            localControl = true;
            hiddenWhenLocallyControlled.SetActive(false);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void Update()
    {
        if (localControl && parentPawn || isHost && parentPawn)
        {
            gameObject.transform.position = parentPawn.transform.position + offset;
            gameObject.transform.rotation = parentPawn.transform.rotation;
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                offset = new Vector3(0, -0.5f, 0);
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                offset = new Vector3(0, -0.0f, 0);
            }
        }
        else if (localControl && !parentPawn || isHost && !parentPawn)
        {
            parentPawn = FindObjectOfType<GameInstance>().localPlayerCharacter.gameObject;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SetTeam(string _team)
    {
        switch (_team)
        {
            case "0":
                foreach (var _object in teamZeroObjects)
                {
                    _object.SetActive(true);
                }
                foreach (var _object in teamOneObjects)
                {
                    _object.SetActive(false);
                }
                foreach (var _object in teamTwoObjects)
                {
                    _object.SetActive(false);
                }
                break;
            case "1":
                foreach (var _object in teamZeroObjects)
                {
                    _object.SetActive(false);
                }
                foreach (var _object in teamOneObjects)
                {
                    _object.SetActive(true);
                }
                foreach (var _object in teamTwoObjects)
                {
                    _object.SetActive(false);
                }
                break;
            case "2":
                foreach (var _object in teamZeroObjects)
                {
                    _object.SetActive(false);
                }
                foreach (var _object in teamOneObjects)
                {
                    _object.SetActive(false);
                }
                foreach (var _object in teamTwoObjects)
                {
                    _object.SetActive(true);
                }
                break;
        }
    }
}
