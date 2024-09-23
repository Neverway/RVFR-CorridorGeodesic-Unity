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


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=

    public static List<TeslaSender> senders = new List<TeslaSender> ();
    public static List<TeslaConductor> conductors = new List<TeslaConductor>();
    private const float MIN_DISTANCE = 5f;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        senders.Clear ();
        conductors.Clear ();
        InvokeRepeating ("CheckSenders", 0f, 0.2f);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private void CheckSenders ()
    {
        foreach (TeslaConductor conductor in conductors)
        {
            conductor.isRecievingPower = false;
            if (!conductor.gameObject.activeInHierarchy)
            {
                continue;
            }
            foreach (TeslaSender sender in senders)
            {
                if (!sender.gameObject.activeInHierarchy)
                {
                    continue;
                }
                if (Vector3.Distance (sender.transform.position, conductor.transform.position) < MIN_DISTANCE)
                {
                    conductor.isRecievingPower = true;
                }
            }
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}