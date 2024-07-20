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
    [SerializeField] private GameObject cutPreviewPrefab;
    private GameObject[] cutPreviews;
    [SerializeField] private float projectileForce;
    [SerializeField] private AnimationCurve riftAnimationCurve;
    public List<GameObject> deployedInfinityMarkers = new List<GameObject> ();
    public static GameObject deployedRift;
    private MeshSlicer[] meshSlicers;

    //Statics for MeshSlicer to use
    public static Plane plane1;
    public static Plane plane2;
    public static List<GameObject> nullSlices;
    public static GameObject plane2Meshes;
    public static List<MeshSlicer> backupMeshes = new List<MeshSlicer> ();
    public static List<GameObject> slicedMeshes = new List<GameObject> ();
    public static List<ActorData> actorDatas = new List<ActorData> ();


    //Object groups for moving slice
    private List<ActorData> plane1Objects;
    private List<ActorData> plane2Objects;
    private List<ActorData> nullSpaceObjects;

    //Object groups for resetting

    //Rift collapse lerp 
    private float riftTimer = 0f;
    private float maxRiftTimer = 2f;
    public static float riftWidth;
    private Vector3 plane2StartPos;

    private bool isCutPreviewActive = false;
    private bool isCollapseStarted = false;

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
        cutPreviews = new GameObject[2];
        for (int i = 0; i < cutPreviews.Length; i++)
        {
            cutPreviews[i] = Instantiate (cutPreviewPrefab);
            cutPreviews[i].SetActive (false);
        }
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
        collapsedDistance = 0;
        collapsedDirection = new Vector3 ();
    }

    public void Update ()
    {
        AimTowardsCenterOfView ();

        if (!isCutPreviewActive)
        {
            if (AreMarkersPinned ())
            {
                DeployRiftAndPreview ();
            }
        }

        if (isCollapseStarted && deployedRift && riftTimer <= maxRiftTimer)
        {
            riftTimer += Time.deltaTime;
            float p = Mathf.Clamp ((riftTimer / maxRiftTimer), 0, 1);
            float lerpAmount = riftAnimationCurve.Evaluate (p);

            deployedRift.transform.localScale = new Vector3 (
                1,
                1,
                1 - lerpAmount
                );

            Vector3 targetOffset = -(deployedRift.transform.forward * riftWidth);

            plane2Meshes.transform.position = Vector3.Lerp (plane2StartPos, plane2StartPos + targetOffset, lerpAmount);

            foreach (ActorData obj in plane2Objects)
            {
                obj.transform.position = Vector3.Lerp (obj.homePosition, obj.homePosition + targetOffset, lerpAmount);
            }

            if (riftTimer > maxRiftTimer)
            {
                deployedRift.SetActive (false);
                foreach (var plane in cutPreviews)
                {
                    plane.SetActive (false);
                }
            }
        }
    }

    private void DeployRiftAndPreview ()
    {

        //riftNormal should point from marker 0 to marker 1

        deployedRift = Instantiate (new GameObject ());
        deployedRift.name = "Rift";
        deployedRift.transform.position = deployedInfinityMarkers[0].transform.position;
        deployedRift.transform.LookAt (deployedInfinityMarkers[1].transform);
        if (allowNoLinearSlicing) deployedRift.transform.rotation = new Quaternion (deployedRift.transform.rotation.x, deployedRift.transform.rotation.y, deployedRift.transform.rotation.z, deployedRift.transform.rotation.w);
        else deployedRift.transform.rotation = new Quaternion (0, deployedRift.transform.rotation.y, 0, deployedRift.transform.rotation.w);

        Vector3 riftNormal = deployedRift.transform.forward;

        Vector3 pos1 = deployedInfinityMarkers[0].transform.position + riftNormal * .25f;
        Vector3 pos2 = deployedInfinityMarkers[1].transform.position - riftNormal * .25f;

        if (allowNoLinearSlicing)
        {
            riftWidth = Vector3.Distance (pos1, pos2);
        }
        else
        {
            riftWidth = Vector3.Distance (new Vector3 (pos1.x, 0, pos1.z), new Vector3 (pos2.x, 0, pos2.z));
        }

        deployedRift.transform.position = pos1;

        plane1 = new Plane (riftNormal, pos1);
        plane2 = new Plane (-riftNormal, pos2);

        cutPreviews[0].SetActive (true);
        cutPreviews[1].SetActive (true);
        cutPreviews[0].transform.position = pos1;
        cutPreviews[1].transform.position = pos2;
        cutPreviews[0].transform.rotation = deployedRift.transform.rotation;
        cutPreviews[1].transform.rotation = deployedRift.transform.rotation;
        isCutPreviewActive = true;
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
        deployedInfinityMarkers.Clear ();
        currentAmmo = 2;        

        cutPreviews[0].transform.SetParent (null);
        cutPreviews[1].transform.SetParent (null);
        cutPreviews[0].SetActive (false);
        cutPreviews[1].SetActive (false);

        if (isCollapseStarted == false)
        {
            return;
        }

        isCutPreviewActive = false;
        isCollapseStarted = false;

        foreach (ActorData a in actorDatas)
        {
            if (a)
            {
                a.GoHome ();
            }
        }

        currentAmmo = maxAmmo;
        deployedInfinityMarkers.Clear ();

        foreach (var g in slicedMeshes)
        {
            if (g) Destroy (g);
        }
        slicedMeshes.Clear ();
        foreach (var g in backupMeshes)
        {
            g.gameObject.SetActive (true);
        }
        backupMeshes.Clear ();

        Destroy (plane2Meshes);
        if (deployedRift)
        {
            StartCoroutine (DestroyWorker (deployedRift));
        }
    }

    private IEnumerator DestroyWorker (GameObject go)
    {
        yield return new WaitForEndOfFrame ();
        Destroy (go);
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

    private bool AreMarkersPinned ()
    {
        //Return true if 2 vacuum tubes are deployed and pinned.
        if (deployedInfinityMarkers.Count >= 2)
        {
            foreach (var marker in deployedInfinityMarkers)
            {
                if (marker.GetComponent<VacuumProjectile> ().pinned == false)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    private void ConvergeInfinityMarkers ()
    {
        if (isCollapseStarted)
        {
            return;
        }
        if (AreMarkersPinned ())
        {
            if (deployedRift)
            {
                Debug.Log ("CONVERGING");
                meshSlicers = FindObjectsOfType<MeshSlicer> ();
                nullSlices = new List<GameObject> ();
                plane2Meshes = Instantiate (new GameObject ());
                plane2Meshes.name = "plane2Meshes";
                plane2StartPos = plane2Meshes.transform.position;
                // Find all slice-able meshes
                foreach (var sliceableMesh in meshSlicers)
                {
                    sliceableMesh.ApplyCuts ();
                }
                foreach (GameObject obj in nullSlices)
                {
                    obj.transform.SetParent (deployedRift.transform, true);
                }
                ParentCollapseObjects ();
                isCollapseStarted = true;
                riftTimer = 0f;
            }
        }
    }

    private void ParentCollapseObjects ()
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

        foreach (var actor in nullSpaceObjects)
        {
            actor.homePosition = actor.transform.position;
            actor.transform.SetParent (deployedRift.transform);
            actor.nullSpace = true;
        }
        cutPreviews[1].transform.SetParent (plane2Meshes.transform);
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
