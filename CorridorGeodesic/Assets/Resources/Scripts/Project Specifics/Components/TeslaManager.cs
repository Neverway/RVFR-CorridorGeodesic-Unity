//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;

public class TeslaManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public LightningLine _lightningLinePrefab;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [IsDomainReloaded] public static LightningLine lightningLinePrefab;
    [IsDomainReloaded] public static List<TeslaSender> senders = new List<TeslaSender> ();
    [IsDomainReloaded] public static List<TeslaConductor> conductors = new List<TeslaConductor>();
    public const float MIN_DISTANCE = 5f;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        lightningLinePrefab = _lightningLinePrefab; //todo: Maybe use an instance setup instead? -use the Reference Manager for this

        senders.Clear ();
        conductors.Clear ();
        InvokeRepeating ("CheckSenders", 0f, 0.2f);
    }
    //=-----------------=
    // Internal Functions
    //=-----------------=

    private void CheckSenders ()
    {
        //foreach (TeslaConductor conductor in conductors)
        //{
        //    conductor.IsTeslaPowered() = false;
        //    if (!conductor.gameObject.activeInHierarchy)
        //    {
        //        continue;
        //    }
        //    foreach (TeslaSender sender in senders)
        //    {
        //        if (!sender.gameObject.activeInHierarchy)
        //        {
        //            continue;
        //        }
        //        if (Vector3.Distance (sender.transform.position, conductor.transform.position) < MIN_DISTANCE)
        //        {
        //            conductor.IsTeslaPowered() = true;
        //        }
        //    }
        //}
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}

public interface TeslaPowerSource
{
    public bool IsTeslaPowered();
    public Transform GetZapTargetTransform();
}