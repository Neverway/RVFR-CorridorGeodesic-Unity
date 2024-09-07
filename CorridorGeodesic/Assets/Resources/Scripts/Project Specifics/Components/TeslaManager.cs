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
    public static List<TeslaReciever> recievers = new List<TeslaReciever> ();
    private const float MIN_DISTANCE = 5f;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start ()
    {
        senders.Clear ();
        recievers.Clear ();
        InvokeRepeating ("CheckSenders", 0f, 0.2f);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private void CheckSenders ()
    {
        foreach (TeslaReciever reciever in recievers)
        {
            reciever.processor.isPowered = false;
            if (!reciever.gameObject.activeInHierarchy)
            {
                continue;
            }
            foreach (TeslaSender sender in senders)
            {
                if (!sender.gameObject.activeInHierarchy)
                {
                    continue;
                }
                if (Vector3.Distance (sender.transform.position, reciever.transform.position) < MIN_DISTANCE)
                {
                    reciever.processor.isPowered = true;
                }
            }
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}