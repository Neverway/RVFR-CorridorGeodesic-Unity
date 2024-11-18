//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework
{
    public class Volume_Physics : Volume
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [SerializeField] private float actorForceIntensity;
        [SerializeField] private float propForceIntensity;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=
        [Tooltip("Uses the forward vector from this object to determine the direction to apply the force")]
        [SerializeField]
        private Transform forceDirection;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void FixedUpdate()
        {
            foreach (var entity in pawnsInTrigger)
            {
                if (entity == null)
                {
                    continue;
                }

                // If no team specified, push everyone
                if (owningTeam == "")
                {
                    if (entity.gameObject.GetComponent<Rigidbody>())
                    {
                        entity.gameObject.GetComponent<Rigidbody>()
                            .AddForce(forceDirection.transform.forward * actorForceIntensity, ForceMode.Force);
                    }
                    else if (entity.gameObject.GetComponent<Rigidbody2D>())
                    {
                        entity.gameObject.GetComponent<Rigidbody2D>()
                            .AddForce(forceDirection.transform.forward * actorForceIntensity, ForceMode2D.Force);
                    }
                }
                // If teams match and self-infliction enabled
                else if (owningTeam == entity.currentState.team && affectsOwnTeam)
                {
                    if (entity.gameObject.GetComponent<Rigidbody>())
                    {
                        entity.gameObject.GetComponent<Rigidbody>()
                            .AddForce(forceDirection.transform.forward * actorForceIntensity, ForceMode.Force);
                    }
                    else if (entity.gameObject.GetComponent<Rigidbody2D>())
                    {
                        entity.gameObject.GetComponent<Rigidbody2D>()
                            .AddForce(forceDirection.transform.forward * actorForceIntensity, ForceMode2D.Force);
                    }
                }
                // If teams don't match and self-infliction disabled
                else if (owningTeam != entity.currentState.team && !affectsOwnTeam)
                {
                    if (entity.gameObject.GetComponent<Rigidbody>())
                    {
                        entity.gameObject.GetComponent<Rigidbody>()
                            .AddForce(forceDirection.transform.forward * actorForceIntensity, ForceMode.Force);
                    }
                    else if (entity.gameObject.GetComponent<Rigidbody2D>())
                    {
                        entity.gameObject.GetComponent<Rigidbody2D>()
                            .AddForce(forceDirection.transform.forward * actorForceIntensity, ForceMode2D.Force);
                    }
                }
            }

            foreach (var entity in propsInTrigger)
            {
                if (entity == null)
                {
                    continue;
                }

                if (entity.GetComponent<Rigidbody>())
                {
                    entity.GetComponent<Rigidbody>().AddForce(forceDirection.transform.forward * propForceIntensity,
                        ForceMode.Force);
                }
                else if (entity.gameObject.GetComponent<Rigidbody2D>())
                {
                    entity.gameObject.GetComponent<Rigidbody2D>()
                        .AddForce(forceDirection.transform.forward * propForceIntensity, ForceMode2D.Force);
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