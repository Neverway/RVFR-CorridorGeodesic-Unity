//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbOutlet : MonoBehaviour, BulbCollisionBehaviour
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

    [field:SerializeField] public Transform attachPoint { get; private set; }

    //=-----------------=
    // Mono Functions
    //=-----------------=

    private IEnumerator Start ()
    {
        yield return null;
        attachPoint.SetParent (null);
        attachPoint.GetComponent<CorGeo_ActorData>().homeParent = null;
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public bool OnBulbCollision(Projectile_Vacumm bulb, RaycastHit hit)
    {
        bulb.Attach(attachPoint.position, attachPoint.forward);
        return true;
    }
}