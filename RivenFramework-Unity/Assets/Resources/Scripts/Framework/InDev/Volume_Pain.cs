//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume_Pain : Volume
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [Tooltip("Negative values will heal pawns")]
    [SerializeField] private float damageAmount;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        foreach (var entity in pawnsInTrigger)
        {
            // If no team specified, or self-infliction enabled, hurt everyone (Kinda metal huh)
            if (owningTeam == "" || affectsOwnTeam)
            {
                entity.ModifyHealth(-damageAmount);
            }
            else
            {
                // If teams match allow healing only
                if (entity.currentState.team == owningTeam && damageAmount < 0 && !affectsOwnTeam)
                {
                    entity.ModifyHealth(-damageAmount);
                }
                // If teams don't match allow pain only
                else if (entity.currentState.team != owningTeam && damageAmount > 0 && !affectsOwnTeam)
                {
                    entity.ModifyHealth(-damageAmount);
                }
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
