//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.LogicSystem;

public class CorGeo_FizzlerKiller : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public GameObject fizzleDust;
    public GameObject fizzleParty;


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private string buddyID;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    
    private new void OnTriggerEnter(Collider _other)
    {
        base.OnTriggerEnter(_other); // Call the base class method
        if (_other.TryGetComponent (out ActorData actor))
        {
            LemonBuddyDestroyedTracker buddy = actor.GetComponentInChildren<LemonBuddyDestroyedTracker>();
            if (buddy != null)
            {
                buddy.OnBuddyDestroyed();
                Instantiate(fizzleParty, actor.transform.position, actor.transform.rotation, null);
                Destroy(actor.gameObject);
            }
            else if (actor.actorId.Contains("Phys"))
            {
                Instantiate(fizzleDust, actor.transform.position, actor.transform.rotation, null);
                Destroy(actor.gameObject);
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
