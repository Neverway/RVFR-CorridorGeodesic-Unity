using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbPowerSocket : LogicComponent, BulbCollisionBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=

    //=-----------------=
    // Private Variables
    //=-----------------=
    private Projectile_Vacumm fittedBulb;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [field: SerializeField] public Transform attachPoint { get; private set; }

    //=-----------------=
    // Mono Functions
    //=-----------------=

    private IEnumerator Start()
    {
        yield return null;
        attachPoint.SetParent(null);
        attachPoint.GetComponent<CorGeo_ActorData>().homeParent = null;
    }
    private void Update()
    {
        isPowered = fittedBulb != null;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public bool OnBulbCollision(Projectile_Vacumm bulb, RaycastHit hit)
    {
        if (fittedBulb != null)
        {
            bulb.KillProjectile();
            return true;
        }

        fittedBulb = bulb;
        bulb.Attach(attachPoint.position, attachPoint.forward);
        return true;
    }
}
