//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NEW_LogicProcessor))]
public class TeslaReciever : MonoBehaviour
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

    [HideInInspector] public NEW_LogicProcessor processor;
    [SerializeField] private GameObject lightObject;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private IEnumerator Start()
    {
        processor = GetComponent<NEW_LogicProcessor>();
        yield return null;
        TeslaManager.recievers.Add (this);
    }

    private void Update ()
    {
        lightObject.SetActive (processor.isPowered);
    }

    private void OnDestroy ()
    {
        TeslaManager.recievers.Remove (this);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}