//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Geodesic_Utility_GeoFolder : Item_Geodesic_Utility
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public int maxAmmo = 2;
    public int currentAmmo = 2;
    public bool allowNoLinearSlicing;
    public float convergenceSpeed = 0.25f;


    //=-----------------=
    // Private Variables
    //=-----------------=
    public float collapsedDistance;
    public Vector3 collapsedDirection;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform centerViewTransform;
    [SerializeField] private GameObject debugObject;
    [SerializeField] private GameObject vacuumProjectile;
    [SerializeField] private GameObject riftObject;
    [SerializeField] private float projectileForce;
    [SerializeField] private AnimationCurve riftAnimationCurve;
    public List<GameObject> deployedInfinityMarkers = new List<GameObject> ();
    public GameObject deployedRift;
    private MeshSlicer[] meshSlicers;

    //Statics for MeshSlicer to use
    public static Plane plane1;
    public static Plane plane2;
    public static List<GameObject> nullSlices;
    public static GameObject plane2Meshes;
    private Vector3 plane2StartPos;

    //Object groups for moving slice
    private List<ActorData> plane1Objects;
    private List<ActorData> plane2Objects;
    private List<ActorData> nullSpaceObjects;

    //Rift collapse lerp 
    private float riftTimer = 0f;
    private float maxRiftTimer = 2f;

    enum SliceSpace
    {
        Plane1,
        Plane2,
        Null
    }

    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        meshSlicers = FindObjectsByType<MeshSlicer> (FindObjectsSortMode.None);
    }

    public override void UsePrimary ()
    {
        DeployInfinityMarker ();
    }

    public override void UseSecondary ()
    {
        ConvergeInfinityMarkers ();
    }

    public override void ReleaseSecondary ()
    {
        //StopCoroutine(PeriodiclyFoldGeometry());
        //UndoGeometricCuts();
        //ApplyGeometricCuts();
    }

    public override void UseSpecial ()
    {
        RecallInfinityMarkers ();
        UndoGeometricCuts ();
        collapsedDistance = 0;
        collapsedDirection = new Vector3 ();
    }

    public void Update ()
    {
        AimTowardsCenterOfView ();

        if (deployedRift && riftTimer <= maxRiftTimer)
        {
            riftTimer += Time.deltaTime;
            float p = Mathf.Clamp ((riftTimer / maxRiftTimer), 0, 1);
            float lerpAmount = riftAnimationCurve.Evaluate (p);

            deployedRift.transform.localScale = new Vector3 (
                1,
                1,
                1 - lerpAmount
                );

            Vector3 targetOffset = (deployedRift.transform.forward * deployedRift.GetComponent<Rift> ().riftWidth);

            plane2Meshes.transform.position = Vector3.Lerp (plane2StartPos, plane2StartPos + targetOffset, lerpAmount);

            foreach (ActorData obj in plane2Objects)
            {
                obj.transform.position = Vector3.Lerp (obj.homePosition, obj.homePosition + targetOffset, lerpAmount);
            }

            if (riftTimer > maxRiftTimer)
            {
                deployedRift.SetActive (false);
            }
        }
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void RecallInfinityMarkers ()
    {
        var projectiles = FindObjectsOfType<VacuumProjectile> ();
        foreach (var projectile in projectiles)
        {
            Destroy (projectile.gameObject);
        }

        if (deployedRift)
        {
            Destroy (deployedRift);
        }
        currentAmmo = maxAmmo;
        deployedInfinityMarkers.Clear ();
    }
    private void DeployInfinityMarker ()
    {
        if (currentAmmo <= 0) return;
        currentAmmo--;
        var projectile = Instantiate (vacuumProjectile, barrelTransform.transform.position, barrelTransform.rotation, null);
        projectile.GetComponent<Rigidbody> ().AddForce (projectile.transform.forward * projectileForce, ForceMode.Impulse);
        projectile.GetComponent<VacuumProjectile> ().geoFolder = this; // Get a reference to the gun that spawned the projectile, so we know who to give ammo to on a lifetime expiration
        deployedInfinityMarkers.Add (projectile);
    }

    private void ConvergeInfinityMarkers ()
    {
        if (deployedInfinityMarkers.Count >= 2)
        {
            if (!deployedRift)
            {
                //riftNormal should point from marker 0 to marker 1
                Vector3 riftNormal = (deployedInfinityMarkers[1].transform.position - deployedInfinityMarkers[0].transform.position).normalized;

                Vector3 pos1 = deployedInfinityMarkers[0].transform.position + riftNormal * .25f;
                Vector3 pos2 = deployedInfinityMarkers[1].transform.position - riftNormal * .25f;

                deployedRift = Instantiate (riftObject);
                deployedRift.transform.position = pos1;
                deployedRift.transform.LookAt (deployedInfinityMarkers[0].transform);
                if (allowNoLinearSlicing) deployedRift.transform.rotation = new Quaternion (deployedRift.transform.rotation.x, deployedRift.transform.rotation.y, deployedRift.transform.rotation.z, deployedRift.transform.rotation.w);
                else deployedRift.transform.rotation = new Quaternion (0, deployedRift.transform.rotation.y, 0, deployedRift.transform.rotation.w);

                deployedRift.GetComponent<Rift> ().riftWidth = Vector3.Distance (pos1, pos2);

                plane1 = new Plane (riftNormal, pos1);
                plane2 = new Plane (-riftNormal, pos2);

                nullSlices = new List<GameObject> ();
                plane2Meshes = Instantiate (new GameObject ());
                plane2StartPos = plane2Meshes.transform.position;
                // Find all slice-able meshes
                foreach (var sliceableMesh in meshSlicers)
                {
                    sliceableMesh.ApplyCuts ();
                }
                foreach (GameObject obj in nullSlices)
                {
                    if (obj.TryGetComponent<MeshFilter> (out var meshFilter))
                    {
                        // Recalculate the mesh render bounds
                        // (This tells the game how big, and where, the object is. If it's small enough or out of view the game doesn't render it)
                        meshFilter.mesh.RecalculateBounds ();
                    }
                    obj.transform.SetParent (deployedRift.transform, true);
                }
                OffsetActorsAccordingToConvergenceDistance (true);
            }
        }
    }

    private IEnumerator PeriodiclyFoldGeometry ()
    {
        yield return new WaitForSeconds (0f);
        UndoGeometricCuts ();
        ApplyGeometricCuts ();
    }

    private void ApplyGeometricCuts ()
    {
        if (!deployedRift) return;
        collapsedDistance = 0;
        collapsedDirection = new Vector3 ();

        // Delay the movement until after the cut has been fully created
        foreach (var sliceableMesh in FindObjectsByType<MeshSlicer> (FindObjectsSortMode.None))
        {
            if (sliceableMesh.segmentId == 2)
            {
                sliceableMesh.gameObject.transform.position += (collapsedDirection * collapsedDistance) / 2;// We divide by two, so we can get half of the distance
            }
            else if (sliceableMesh.segmentId == 0)
            {
                sliceableMesh.gameObject.transform.position -= (collapsedDirection * collapsedDistance) / 2;// We divide by two, so we can get half of the distance
            }
            // Any segment with the id of 1 is the result of some wierd artifacting, so we BURN IT IN A FIRELY BLAZE OF GLORY
            else if (sliceableMesh.segmentId == 1)
            {
                Destroy (sliceableMesh.gameObject);
            }
        }

        OffsetActorsAccordingToConvergenceDistance (_collapsing: true);
    }

    private void UndoGeometricCuts ()
    {
        // Find all slice-able meshes
        foreach (var sliceableMesh in FindObjectsByType<MeshSlicer> (FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (sliceableMesh.isCascadeSegment)
            {
                Destroy (sliceableMesh.gameObject);
            }
            else
            {
                sliceableMesh.gameObject.SetActive (true);
            }
        }

        OffsetActorsAccordingToConvergenceDistance (false);
    }

    private void OffsetActorsAccordingToConvergenceDistance (bool _collapsing)
    {
        if (!deployedRift) return;
        // Reposition objects in A & B space to match the change in distance between the markers

        plane1Objects = new List<ActorData> ();
        plane2Objects = new List<ActorData> ();
        nullSpaceObjects = new List<ActorData> ();

        foreach (var actor in FindObjectsOfType<ActorData> ())
        {
            float distance1 = plane1.GetDistanceToPoint (actor.transform.position);

            if (distance1 < 0)
            {
                plane1Objects.Add (actor);
                continue;
            }

            float distance2 = plane2.GetDistanceToPoint (actor.transform.position);

            if (distance2 < 0)
            {
                plane2Objects.Add (actor);
                actor.homePosition = actor.transform.position;
                continue;
            }

            //if both distances are >= 0, we are in null space.
            nullSpaceObjects.Add (actor);
        }

        GameObject p1 = Instantiate (new GameObject ());
        p1.name = "plane1Objects";
        GameObject p2 = Instantiate (new GameObject ());
        p2.name = "plane2Objects";

        foreach (var actor in plane1Objects)
        {
            actor.transform.SetParent (p1.transform);
        }
        foreach (var actor in plane2Objects)
        {
            actor.transform.SetParent (p2.transform);
        }
        foreach (var actor in nullSpaceObjects)
        {
            actor.transform.SetParent (deployedRift.transform);
        }

    }

    private void AimTowardsCenterOfView ()
    {
        RaycastHit viewPoint = new RaycastHit ();
        Physics.Raycast (centerViewTransform.position, centerViewTransform.forward, out viewPoint);
        Debug.DrawRay (centerViewTransform.position, centerViewTransform.forward, Color.cyan);
        barrelTransform.LookAt (viewPoint.point);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
