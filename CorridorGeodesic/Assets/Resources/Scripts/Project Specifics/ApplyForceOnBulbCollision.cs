using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ApplyForceOnBulbCollision : MonoBehaviour, BulbCollisionBehaviour
{
    public float force;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public bool OnBulbCollision(Projectile_Vacumm bulb, RaycastHit hit)
    {
        rigidbody.AddForceAtPosition(bulb.moveVector * force, hit.point, ForceMode.Impulse);
        bulb.KillProjectile();
        return true;
    }
}
