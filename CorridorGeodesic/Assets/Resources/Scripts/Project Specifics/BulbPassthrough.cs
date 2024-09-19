using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbPassthrough : MonoBehaviour, BulbCollisionBehaviour
{
    //Returns true to ignore rest of collision logic
    public bool OnBulbCollision(Projectile_Vacumm bulb, RaycastHit hit) => true;
}
