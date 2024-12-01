//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.PawnManagement;

namespace Neverway.Framework.LogicSystem
{
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

            if (_other.TryGetComponent<CorGeo_ActorData>(out CorGeo_ActorData actor))
            {
                // Why was there a parameter to disable death in kill volumes? ~Liz
                if (actor.destroyedInKillTrigger)
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
}