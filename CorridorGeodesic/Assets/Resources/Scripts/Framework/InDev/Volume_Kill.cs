//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume_Kill : Volume
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


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private new void OnTriggerEnter2D(Collider2D _other)
    {
        base.OnTriggerEnter2D(_other); // Call the base class method
        if (_other.CompareTag("Pawn"))
        {
            _other.GetComponent<Pawn>().Kill();
        }
    }
    
    private new void OnTriggerEnter(Collider _other)
    {
        base.OnTriggerEnter(_other); // Call the base class method
        print("Connors stole something.");
        if (_other.CompareTag("Pawn"))
        {
            print("Connors Has a hat!");
            _other.GetComponent<Pawn>().Kill();
        }
        if (_other.TryGetComponent<CorGeo_ActorData> (out CorGeo_ActorData actor))
        {
            if (actor.diesInKillTrigger)
            {
                Destroy(_other.gameObject);
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
