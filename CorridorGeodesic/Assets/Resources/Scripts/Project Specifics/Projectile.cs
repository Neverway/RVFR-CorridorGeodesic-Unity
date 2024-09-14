//===================== (Neverway 2024) Written by Andre Blunt ================
//
// Purpose:
// Notes:
//
//=============================================================================

using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] protected Opt_Lifetime lifetime;
    [SerializeField] protected float radius;
    /// <summary>
    /// Optional field that is used for a motion tween to fake the projectile being shot from the gun barrel.
    /// </summary>
    [SerializeField] private Transform projectileGraphics;

    private float moveSpeed;

    protected bool disabled;

    private Vector3 lastPosition;

    private LayerMask ignoreMask;

    private Vector3 moveVector; //used in update loop collision & move

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public virtual void Awake ()
    {
        ignoreMask = CorGeo_ReferenceManager.Instance.playerProjectileIgnoreMask;
        lastPosition = transform.position;
    }
    private void Update ()
    {
        if (disabled)
            return;

        moveVector = transform.forward * moveSpeed * Time.deltaTime;
        if (!CollisionLogic ())
            MoveLogic ();
    }
    private void OnEnable()
    {
        if(lifetime)
            lifetime.OnLifetimeOut += OnLifetimeOut;
    }
    private void OnDestroy()
    {
        if (lifetime)
            lifetime.OnLifetimeOut -= OnLifetimeOut;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public virtual void MoveLogic ()
    {
        transform.position += moveVector;
    }
    public virtual bool CollisionLogic ()
    {
        if (Physics.Raycast (transform.position, transform.forward * moveSpeed * Time.deltaTime, out RaycastHit hit, moveVector.magnitude + radius + 1f, ignoreMask))
        {
            OnCollision (hit);
            return true;
        }
        return false;
    }
    public virtual void OnCollision (RaycastHit hit)
    {
        if (projectileGraphics != null)
        {
            projectileGraphics.DOKill ();
            projectileGraphics.transform.localPosition = Vector3.zero;
        }
        lifetime.StopTimer();
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void InitializeProjectile (float moveSpeed, Vector3 position, Vector3 forward)
    {
        this.moveSpeed = moveSpeed;
        transform.position = position;
        transform.forward = forward;
    }
    public void InitializeProjectile (float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    /// <summary>
    /// Spawn projectile and tween it's graphics to toward it's origin,
    /// </summary>
    /// <param name="moveSpeed"></param>
    /// <param name="graphicsPosition"></param>
    /// <param name="distance"></param>
    public void InitializeProjectile (float moveSpeed, Vector3 graphicsPosition = default, float distance = 0)
    {
        this.moveSpeed = moveSpeed;
        if (projectileGraphics == null)
        {
            return;
        }
        float time = (distance * moveSpeed) - 0.1f;
        if (time < 0) time = 0;
        projectileGraphics.position = graphicsPosition;
        projectileGraphics.DOLocalMove (Vector3.zero, time);
    }
    public virtual void OnLifetimeOut()
    {

    }
}