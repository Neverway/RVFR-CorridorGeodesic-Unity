//===================== (Neverway 2024) Written by Andre Blunt ================
//
// Purpose: Attached to the vacuum tube projectile, detects when the projectile
//  has hit a valid surface, and attaches it
// Notes: This version fakes the projectile movement but prevents the "projectile"
//  from clipping out of bounds
//
//=============================================================================

using UnityEngine;

public class Projectile_VacummNew: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] protected float radius;
    private float moveSpeed;

    protected bool disabled;

    private Vector3 lastPosition;

    private LayerMask ignoreMask;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public virtual void Awake()
    {
        ignoreMask = CorGeo_ReferenceManager.Instance.playerProjectileIgnoreMask;
        lastPosition = transform.position;
    }
    private void Update()
    {
        if (disabled)
            return;

        MoveLogic();
        CollisionLogic();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public virtual void MoveLogic()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    public virtual void CollisionLogic()
    {
        if(Physics.Raycast(lastPosition, transform.forward, out RaycastHit hit, Vector3.Distance(lastPosition, transform.position) + radius, ignoreMask))
        {
            OnCollision(hit);
        }
        lastPosition = transform.position;
    }
    public virtual void OnCollision(RaycastHit hit)
    {

    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void InitializeProjectile(float moveSpeed, Vector3 position, Vector3 forward)
    {
        this.moveSpeed = moveSpeed;
        transform.position = position;
        transform.forward = forward;
    }
    public void InitializeProjectile(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
}
