//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop_LaserWall : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private LayerMask ignoreMask;
    [SerializeField] private int maxSteps = 32;

    private Ray ray;
    private RaycastHit hit;

    private float laserWidth;

    private Vector3 rayStepVector;

    [SerializeField] private List<Prop_LaserWallCollider> lasers = new List<Prop_LaserWallCollider>();

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent input;
    [SerializeField] private Transform checkTop, checkBottom;
    [SerializeField] private Prop_LaserWallCollider laserWallCollider;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    public override void Awake()
    {
        base.Awake();
        ray = new Ray(checkTop.position, transform.right);

        SetupColliders();
    }
    private void FixedUpdate()
    {
        if(isPowered)
            UpdateWall();
    }
    //private void OnEnable()
    //{
    //    if(input != null)
    //        input.OnPowerStateChanged += SourcePowerStateChanged;
    //}
    //private void OnDestroy()
    //{
    //    if (input != null)
    //        input.OnPowerStateChanged -= SourcePowerStateChanged;
    //}

    //=-----------------=
    // Internal Functions
    //=-----------------=
    void SetupColliders()
    {
        lasers.ForEach(l => Destroy(l));
        lasers.Clear();

        for (int i = 0; i < maxSteps; i++)
        {
            lasers.Add(Instantiate(laserWallCollider, transform));
        }

        rayStepVector = (checkTop.position - checkBottom.position) / maxSteps;
        laserWidth = rayStepVector.magnitude;
    }
    void UpdateWall()
    {
        rayStepVector = (checkTop.position - checkBottom.position) / maxSteps;

        ray.direction = transform.right;

        float hitDistance;
        for (int i = 0; i < maxSteps; i++)
        {
            ray.origin = rayStepVector * i + rayStepVector * 0.5f + transform.position;

            if (Physics.Raycast(ray, out hit, 999, ~ignoreMask))
                hitDistance = hit.distance;
            else
                hitDistance = 999;

            float laserHeight = laserWidth * i;

            lasers[i].ChangeValues(
                new Vector3(hitDistance, laserWidth, 0.1f),
                new Vector3(hitDistance * 0.5f, laserHeight + laserWidth * 0.5f, 0),
                ray.origin, hitDistance == 999 ? ray.GetPoint(999) : hit.point, laserWidth);
        }
    }
    //=-----------------=
    // External Functions
    //=-----------------=
    public override void SourcePowerStateChanged(bool powered)
    {
        isPowered = powered;

        if (!isPowered)
            lasers.ForEach(l => l.ResetValues());
    }
}
