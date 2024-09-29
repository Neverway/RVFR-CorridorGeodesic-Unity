//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Prop_Explosive : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField][LogicComponentHandle] private LogicComponent inputSignal;
    [SerializeField] private float radius = 10;
    private bool exploded = false;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private GameObject explosionParticles;

    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=
    IEnumerator Explode()
    {
        explosionParticles.SetActive(true);

        Collider[] cols = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in cols)
        {
            if (col.TryGetComponent(out Pawn pawn))
            {
                float damageMultiplier = 1 - Vector3.Distance(transform.position, col.transform.position) / (radius * 0.75f);
                pawn.ModifyHealth(-200f * damageMultiplier);
            }
        }

        yield return new WaitForSeconds(10);

        Destroy(gameObject);
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        if (powered && !exploded)
        {
            exploded = true;
            StartCoroutine(Explode());
        }
    }
}
