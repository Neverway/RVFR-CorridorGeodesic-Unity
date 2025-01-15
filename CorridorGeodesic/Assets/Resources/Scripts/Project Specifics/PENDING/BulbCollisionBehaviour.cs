using UnityEngine;

public interface BulbCollisionBehaviour
{
    /// <summary>
    /// What to do with the Bulb when it collides with this object
    /// </summary>
    /// <param name="bulb">The bulb that collided with this object</param>
    /// <param name="hit">The raycast hit info that led to this collision</param>
    /// <returns>True if bulb should stop its collision logic</returns>
    public abstract bool OnBulbCollision(Projectile_Vacumm bulb, RaycastHit hit);
}
