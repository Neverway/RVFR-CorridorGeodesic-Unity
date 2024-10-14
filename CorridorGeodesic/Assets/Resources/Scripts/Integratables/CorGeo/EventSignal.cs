//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;

public class EventSignal : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("If this is true, any outputs will be powered")]
    public bool isPowered;
    [Tooltip("These objects will receive power from this event")]
    public EventSignal[] outputs;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool previousIsPoweredState; // this is used to check to see if the powered state has changed


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        // Check if the powered state has changed
        if (isPowered != previousIsPoweredState)
        {
            foreach (var output in outputs)
            {
                output.isPowered = isPowered; // Pass the power signal along
            }

        }
        
        // Update the last power state
        previousIsPoweredState = isPowered;
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
