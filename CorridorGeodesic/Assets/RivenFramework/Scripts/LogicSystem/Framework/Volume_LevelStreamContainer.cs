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
using UnityEngine.SceneManagement;

namespace Neverway.Framework.LogicSystem
{
public class Volume_LevelStreamContainer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Vector3 exitOffset;
    public bool initializedExitZone;
    public GameObject parentStreamVolume;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool hasActivated;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if (!initializedExitZone) return;
        if (!parentStreamVolume && !hasActivated)
        {
            hasActivated = true;
            print($"link to parent has been lost, scene must have changed! Ejecting...");
            StartCoroutine(EjectStreamedActors());
        }
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public IEnumerator EjectStreamedActors()
    {
        transform.position += exitOffset;
        while (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject actor = transform.GetChild(i).gameObject;
                actor.transform.SetParent(null);
            }
        }
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
}
