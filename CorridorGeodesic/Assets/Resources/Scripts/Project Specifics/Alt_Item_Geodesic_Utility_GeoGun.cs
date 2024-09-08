//===================== (Neverway 2024) Written by Liz M. & Connorses =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Alt_Item_Geodesic_Utility_GeoGun : Item_Geodesic_Utility
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public int maxAmmo = 2;
    public int currentAmmo = 2;
    public bool allowNoLinearSlicing;
    public LayerMask viewCastMask;


    //=-----------------=
    // Private Variables
    //=-----------------=
    public Vector3 previousPlanePosition;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform centerViewTransform;
    [SerializeField] private GameObject debugObject;
    [FormerlySerializedAs("vacuumProjectile")] [SerializeField] private Projectile_Vacumm projectileVacumm;
    [SerializeField] private GameObject riftObject;
    [SerializeField] private GameObject cutPreviewPrefab;
    public GameObject[] cutPreviews;
    [SerializeField] private float projectileForce;
    public List<GameObject> deployedInfinityMarkers = new List<GameObject> ();
    public static GameObject deployedRift;
    private Mesh_Slicable[] meshSlicers;

    //Statics for ALTMeshSlicer to use
    public static Plane planeA;
    public static Plane planeB;
    public static List<GameObject> nullSlices;
    public static GameObject planeBMeshes;
    public static List<Mesh_Slicable> originalSliceableObjects = new List<Mesh_Slicable> ();
    public static List<GameObject> slicedMeshes = new List<GameObject> ();
    public static List<CorGeo_ActorData> CorGeo_ActorDatas = new List<CorGeo_ActorData> ();

    //Object groups for resetting

    //Rift collapse lerp 
    private Vector3 riftNormal;
    private float riftTimer = 0f;
    private float maxRiftTimer = 2f;
    public static float lerpAmount;
    [SerializeField] private float riftSecondsPerUnit = 1f;
    public static float riftWidth;
    private Vector3 planeBStartPos;
    private bool secondaryHeld = false;

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
        meshSlicers = FindObjectsByType<Mesh_Slicable> (FindObjectsSortMode.None);

        CreateCutPreviews ();
    }

    public override void UsePrimary ()
    {
        DeployInfinityMarker ();
    }

    public override void UseSecondary ()
    {
        ConvergeInfinityMarkers ();
        secondaryHeld = true;
    }

    public override void ReleaseSecondary ()
    {
        secondaryHeld = false;
    }

    public override void UseSpecial ()
    {
        RecallInfinityMarkers ();
    }

    public void FixedUpdate ()
    {
        AimTowardsCenterOfView ();

        if (!isCutPreviewActive)
        {
            if (AreMarkersPinned ())
            {
                DeployRiftAndPreview ();
            }
        }

        if (deployedRift)
        {
            CheckForActorSpaceChanges ();
        }

        if (isCollapseStarted && deployedRift && riftTimer <= maxRiftTimer)
        {
            // If converging, increase the riftTimer and relocate actors and meshes
            if (secondaryHeld)
            {
                riftTimer += Time.fixedDeltaTime;

                // Calculate offset
                Vector3 targetOffset = -(deployedRift.transform.forward * riftWidth);
                lerpAmount = Mathf.Clamp ((riftTimer / maxRiftTimer), 0, 1);

                float prevRiftWidth = deployedRift.transform.localScale.z * riftWidth;

                // Squish null-space parent
                deployedRift.transform.localScale = new Vector3 (1, 1, 1 - lerpAmount);

                float newRiftWidth = deployedRift.transform.localScale.z * riftWidth;

                // Collapse meshes in planeB/B-Space
                planeBMeshes.transform.position = Vector3.Lerp (planeBStartPos, planeBStartPos + targetOffset, lerpAmount);

                // Collapse actors in planeB/B-Space
                foreach (CorGeo_ActorData obj in CorGeo_ActorDatas)
                {
                    if (obj.space == CorGeo_ActorData.Space.B)
                    {
                        obj.transform.position += cutPreviews[1].transform.position - previousPlanePosition;
                    }

                    if (obj.space == CorGeo_ActorData.Space.Null && obj.dynamic && !obj.crushInNullSpace)
                    {
                        //Get object's position relative to the rift, then move the object based on the new size of the rift.
                        float percent = planeA.GetDistanceToPoint (obj.transform.position) / prevRiftWidth;
                        float oldDistance = prevRiftWidth * percent;
                        float newDistance = newRiftWidth * percent;
                        obj.transform.position += riftNormal * (newDistance - oldDistance);
                    }
                }

                planeB = new Plane (-riftNormal, cutPreviews[1].transform.position);

                previousPlanePosition = cutPreviews[1].transform.position;
            }

            // If we've converged the rift all the way, deactivate null-space actors and meshes
            if (riftTimer > maxRiftTimer)
            {
                for (int i = 0; i < deployedRift.transform.childCount; i++)
                {
                    if (deployedRift.transform.GetChild (i).TryGetComponent<CorGeo_ActorData> (out var actor))
                    {
                        if (actor.activeInNullSpace)
                        {
                            continue;
                        }
                    }
                    deployedRift.transform.GetChild (i).gameObject.SetActive (false);
                }
                foreach (var plane in cutPreviews)
                {
                    plane.SetActive (false);
                }
            }
            else
            {
                for (int i = 0; i < deployedRift.transform.childCount; i++)
                {
                    if (deployedRift.transform.GetChild (i).TryGetComponent<CorGeo_ActorData> (out var actor))
                    {
                        if (actor.activeInNullSpace)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (deployedRift.transform.GetChild (i).GetComponents<MeshCollider> ().Length > 1)
                        {
                            Destroy (deployedRift.transform.GetChild (i).GetComponents<MeshCollider> ()[0]);
                        }
                    }
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

        riftNormal = deployedRift.transform.forward;

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

        planeA = new Plane (riftNormal, pos1);
        planeB = new Plane (-riftNormal, pos2);


        // TODO make this neater tomorrow
        if (!cutPreviews[0])
        {
            CreateCutPreviews ();
        }

        cutPreviews[0].SetActive (true);
        cutPreviews[1].SetActive (true);
        cutPreviews[0].transform.position = pos1;
        cutPreviews[1].transform.position = pos2;
        cutPreviews[0].transform.rotation = deployedRift.transform.rotation;
        cutPreviews[1].transform.rotation = deployedRift.transform.rotation;
        isCutPreviewActive = true;

        lerpAmount = 0f;
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void CreateCutPreviews ()
    {
        cutPreviews = new GameObject[2];
        for (int i = 0; i < cutPreviews.Length; i++)
        {
            cutPreviews[i] = Instantiate (cutPreviewPrefab);
            cutPreviews[i].GetComponent<CutPreviewTracker> ().cutPreviewID = i; // Label them so we know whether they are a or b space's side preview
            cutPreviews[i].SetActive (false);
        }
    }

    public void RecallInfinityMarkers ()
    {
        if (currentAmmo >= 2) return;
        foreach (var projectile in deployedInfinityMarkers)
        {
            projectile.GetComponent<Projectile_Vacumm> ().KillProjectile (false);
        }
        deployedInfinityMarkers.Clear ();
        currentAmmo = 2;

        if (cutPreviews[0])
        {
            cutPreviews[0].transform.SetParent (null);
            cutPreviews[0].SetActive (false);
        }
        if (cutPreviews[1])
        {
            cutPreviews[1].transform.SetParent (null);
            cutPreviews[1].SetActive (false);
        }
        isCutPreviewActive = false;

        isCutPreviewActive = false;

        isCollapseStarted = false;

        foreach (CorGeo_ActorData _actor in CorGeo_ActorDatas)
        {
            if (_actor)
            {
                if (riftTimer < maxRiftTimer && _actor.dynamic)
                {
                    RecallDynamicActor (_actor);
                }
                else
                {
                    _actor.GoHome ();
                }
            }
        }

        currentAmmo = maxAmmo;

        foreach (var _gameObject in slicedMeshes)
        {
            if (_gameObject) Destroy (_gameObject);
        }
        slicedMeshes.Clear ();
        foreach (Mesh_Slicable _gameObject in originalSliceableObjects)
        {
            if (_gameObject) _gameObject.GoHome ();
        }

        Destroy (planeBMeshes);
        if (deployedRift)
        {
            if (nullSlices != null)
            {
                foreach (var _gameObject in nullSlices)
                {
                    if (_gameObject)
                    {
                        _gameObject.GetComponent<Mesh_Slicable> ().GoHome ();
                    }
                }
            }
            StartCoroutine (DestroyWorker (deployedRift));
        }
        // Reset the value used to track if an actor is in null space
        foreach (CorGeo_ActorData actor in CorGeo_ActorDatas)
        {
            actor.space = CorGeo_ActorData.Space.None;
        }

        if (isCollapseStarted == false && deployedRift)
        {
            Destroy (deployedRift.gameObject);
            return;
        }

        StartCoroutine (WitchHunt ());
        lerpAmount = 0f;
    }

    private void RecallDynamicActor (CorGeo_ActorData _actor)
    {
        if (_actor.space == CorGeo_ActorData.Space.Null)
        {
            _actor.transform.SetParent (_actor.homeParent, true);
            _actor.transform.localScale = _actor.homeScale;
            //Moves an actor to keep them in the same relative position to the map when the rift is recalled.
            float scaledRiftWidth = deployedRift.transform.localScale.z * riftWidth;
            float percent = planeA.GetDistanceToPoint (_actor.transform.position) / scaledRiftWidth;
            float oldDistance = scaledRiftWidth * percent;
            float newDistance = riftWidth * percent;
            _actor.transform.position += riftNormal * (newDistance - oldDistance);
            return;
        }

        if (_actor.space != CorGeo_ActorData.Space.Null && Alt_Item_Geodesic_Utility_GeoGun.planeA.GetDistanceToPoint (_actor.transform.position) > 0)
        {
            if (!Alt_Item_Geodesic_Utility_GeoGun.deployedRift) return;
            //move actor away from collapse direction scaled by the rift timer's progress
            _actor.transform.position += Alt_Item_Geodesic_Utility_GeoGun.deployedRift.transform.forward *
                                    Alt_Item_Geodesic_Utility_GeoGun.riftWidth *
                                    (Alt_Item_Geodesic_Utility_GeoGun.lerpAmount);
        }
    }

    private IEnumerator DestroyWorker (GameObject _gameObject)
    {
        yield return new WaitForEndOfFrame ();
        Destroy (_gameObject);
    }

    private void DeployInfinityMarker ()
    {
        if (currentAmmo <= 0) return;
        currentAmmo--;
        var projectile = Instantiate (projectileVacumm, barrelTransform.transform.position, barrelTransform.rotation, null);
        projectile.InitializeProjectile (projectileForce);
        projectile.geoGun = this; // Get a reference to the gun that spawned the projectile, so we know who to give ammo to on a lifetime expiration
        deployedInfinityMarkers.Add (projectile.gameObject);
    }

    private bool AreMarkersPinned ()
    {
        //Return true if 2 vacuum tubes are deployed and pinned.
        if (deployedInfinityMarkers.Count >= 2)
        {
            foreach (var marker in deployedInfinityMarkers)
            {
                if (!marker.GetComponent<Projectile_Vacumm> ().pinned)
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
                meshSlicers = FindObjectsOfType<Mesh_Slicable> ();
                nullSlices = new List<GameObject> ();
                planeBMeshes = Instantiate (new GameObject ());
                planeBMeshes.name = "planeBMeshes";
                planeBStartPos = planeBMeshes.transform.position;
                // Find all slice-able meshes
                foreach (var sliceableMesh in meshSlicers)
                {
                    sliceableMesh.ApplyCuts ();
                }
                foreach (GameObject obj in nullSlices)
                {
                    obj.transform.SetParent (deployedRift.transform, true);
                    //remove extra meshes (there were un-sliced meshes sticking around)
                    var meshColliders = GetComponents<MeshCollider> ();
                    if (meshColliders.Length > 1)
                    {
                        Destroy (meshColliders[0]);
                    }
                }
                AssignInitialActorRiftSpace ();
                isCollapseStarted = true;
                riftTimer = 0f;
                maxRiftTimer = riftWidth * riftSecondsPerUnit;
                previousPlanePosition = cutPreviews[1].transform.position;
            }
        }
    }

    private void AssignInitialActorRiftSpace ()
    {
        if (!deployedRift) return;
        // Reposition objects in A & B space to match the change in distance between the markers

        foreach (var actor in CorGeo_ActorDatas)
        {
            float distance1 = planeA.GetDistanceToPoint (actor.transform.position);

            if (distance1 < 0)
            {
                actor.space = CorGeo_ActorData.Space.A;
                continue;
            }

            float distance2 = planeB.GetDistanceToPoint (actor.transform.position);

            if (distance2 < 0)
            {
                actor.space = CorGeo_ActorData.Space.B;
                actor.homePosition = actor.transform.position;
                continue;
            }

            //if both distances are >= 0, we are in null space.
            actor.space = CorGeo_ActorData.Space.Null;
        }

        foreach (CorGeo_ActorData actor in CorGeo_ActorDatas)
        {
            if (actor.space == CorGeo_ActorData.Space.Null)
            {
                if (!actor.crushInNullSpace) continue;
                actor.homePosition = actor.transform.position;
                actor.transform.SetParent (deployedRift.transform);
            }
        }
        cutPreviews[1].transform.SetParent (planeBMeshes.transform);
    }

    private void CheckForActorSpaceChanges ()
    {
        // Check A-Space entities to see if they have exited A-Space
        // Check B-Space entities to see if they have exited B-Space
        // Check Null-Space entities to see if they have exited Null-Space (This one sucks!)
        if (!deployedRift) return;
        foreach (var actor in CorGeo_ActorDatas)
        {
            if (!actor.dynamic)
            {
                continue;
            }
            switch (GetActorRiftSpace (actor))
            {
                case CorGeo_ActorData.Space.A:
                    if (actor.space == CorGeo_ActorData.Space.Null)
                    {
                        Debug.Log ($"{actor.gameObject.name} moved [Null] -> [A]");
                        if (actor.crushInNullSpace)
                        {
                            actor.transform.SetParent (actor.homeParent);
                            actor.transform.localScale = actor.homeScale;
                        }
                        actor.space = CorGeo_ActorData.Space.A;
                        continue;
                    }
                    if (actor.space == CorGeo_ActorData.Space.B)
                    {
                        Debug.Log ($"{actor.gameObject.name} moved [B] -> [A]");
                        actor.space = CorGeo_ActorData.Space.A;
                        continue;
                    }
                    actor.space = CorGeo_ActorData.Space.A;
                    continue;
                case CorGeo_ActorData.Space.B:
                    if (actor.space == CorGeo_ActorData.Space.Null)
                    {
                        if (actor.crushInNullSpace)
                        {
                            actor.transform.SetParent (actor.homeParent);
                            actor.transform.localScale = actor.homeScale;
                        }
                        actor.space = CorGeo_ActorData.Space.B;
                        Debug.Log ($"{actor.gameObject.name} moved [Null] -> [B]");
                        continue;
                    }
                    if (actor.space == CorGeo_ActorData.Space.A)
                    {
                        actor.space = CorGeo_ActorData.Space.B;
                        Debug.Log ($"{actor.gameObject.name} moved [A] -> [B]");
                        continue;
                    }
                    actor.space = CorGeo_ActorData.Space.B;
                    continue;
                case CorGeo_ActorData.Space.Null:
                    if (actor.space == CorGeo_ActorData.Space.A)
                    {
                        actor.homePosition = actor.transform.position;
                        if (actor.crushInNullSpace)
                        {
                            actor.transform.SetParent (deployedRift.transform);
                        }
                        actor.space = CorGeo_ActorData.Space.Null;
                        Debug.Log ($"{actor.gameObject.name} moved [A] -> [Null]");
                        continue;
                    }
                    if (actor.space == CorGeo_ActorData.Space.B)
                    {
                        actor.homePosition = actor.transform.position;
                        if (actor.crushInNullSpace)
                        {
                            actor.transform.SetParent (deployedRift.transform);
                        }
                        actor.space = CorGeo_ActorData.Space.Null;
                        Debug.Log ($"{actor.gameObject.name} moved [B] -> [Null]");
                        continue;
                    }
                    actor.space = CorGeo_ActorData.Space.Null;
                    continue;
            }
        }
    }

    private void AimTowardsCenterOfView ()
    {
        RaycastHit viewPoint = new RaycastHit ();
        // Perform the raycast, ignoring the trigger layer
        if (Physics.Raycast (centerViewTransform.position, centerViewTransform.forward, out viewPoint, Mathf.Infinity, viewCastMask))
        {
            // If the raycast hits something, aim the barrel towards the hit point
            barrelTransform.LookAt (viewPoint.point);
        }
    }

    private IEnumerator WitchHunt ()
    {
        var everything = FindObjectsOfType<GameObject> ();
        foreach (GameObject obj in everything)
        {
            yield return new WaitForEndOfFrame ();
            if (obj && obj.name == "New Game Object")
            {
                Destroy (obj);
            }
        }
    }

    /// <summary>
    /// Returns weather an actor is in A-Space, B-Space, or Null-Space
    /// </summary>
    /// <returns></returns>
    private CorGeo_ActorData.Space GetActorRiftSpace (CorGeo_ActorData _actor)
    {
        float distance1 = planeA.GetDistanceToPoint (_actor.transform.position);
        float distance2 = planeB.GetDistanceToPoint (_actor.transform.position);

        if (_actor.debugLogData) print ($"{_actor.gameObject.name}: A{distance1} | B{distance2}");

        if (distance1 < 0)
        {
            return CorGeo_ActorData.Space.A;
        }

        if (distance2 < 0)
        {
            return CorGeo_ActorData.Space.B;
        }

        return CorGeo_ActorData.Space.Null;
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
