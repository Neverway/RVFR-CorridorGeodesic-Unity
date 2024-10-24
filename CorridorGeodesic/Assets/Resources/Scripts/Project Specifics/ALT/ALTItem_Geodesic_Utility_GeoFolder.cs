//===================== (Neverway 2024) Written by Liz M. & Connorses =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALTItem_Geodesic_Utility_GeoFolder : Item_Geodesic_Utility
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
    [SerializeField] private GameObject vacuumProjectile;
    [SerializeField] private GameObject riftObject;
    [SerializeField] private GameObject cutPreviewPrefab;
    private GameObject[] cutPreviews;
    [SerializeField] private float projectileForce;
    public List<GameObject> deployedInfinityMarkers = new List<GameObject> ();
    public static GameObject deployedRift;
    private ALTMeshSlicer[] meshSlicers;

    //Statics for ALTMeshSlicer to use
    public static Plane plane1;
    public static Plane plane2;
    public static List<GameObject> nullSlices;
    public static GameObject plane2Meshes;
    public static List<ALTMeshSlicer> originalSliceableObjects = new List<ALTMeshSlicer> ();
    public static List<GameObject> slicedMeshes = new List<GameObject> ();
    public static List<CorGeo_ActorData> CorGeo_ActorDatas = new List<CorGeo_ActorData> ();


    //Object groups for moving slice
    private List<CorGeo_ActorData> plane1Objects;
    private List<CorGeo_ActorData> plane2Objects;
    private List<CorGeo_ActorData> nullSpaceObjects;

    //Object groups for resetting

    //Rift collapse lerp 
    private float riftTimer = 0f;
    private float maxRiftTimer = 2f;
    public static float lerpAmount;
    [SerializeField] private float riftSecondsPerUnit = 1f;
    public static float riftWidth;
    private Vector3 plane2StartPos;
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
        meshSlicers = FindObjectsByType<ALTMeshSlicer> (FindObjectsSortMode.None);

        CreateCutPreviews();
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
        // TODO Fix this dummy (This function is currently resetting null-space actors homeworld position which majorly breaks things)
        CheckForActorSpaceChanges();

        if (isCollapseStarted && deployedRift && riftTimer <= maxRiftTimer)
        {
            // If converging, increase the riftTimer and relocate actors and meshes
            if (secondaryHeld)
            {
                riftTimer += Time.deltaTime;
            
                // Calculate offset
                Vector3 targetOffset = -(deployedRift.transform.forward * riftWidth);
                lerpAmount = Mathf.Clamp ((riftTimer / maxRiftTimer), 0, 1);

                // Squish null-space parent
                deployedRift.transform.localScale = new Vector3 (1, 1, 1 - lerpAmount);
            
                // Collapse meshes in plane2/B-Space
                plane2Meshes.transform.position = Vector3.Lerp (plane2StartPos, plane2StartPos + targetOffset, lerpAmount);
                
                // Collapse actors in plane2/B-Space
                foreach (CorGeo_ActorData obj in plane2Objects)
                {
                    //obj.transform.position = Vector3.Lerp(obj.homePosition, obj.homePosition + targetOffset, lerpAmount);
                    
                    obj.transform.position += cutPreviews[1].transform.position - previousPlanePosition;
                }

                previousPlanePosition = cutPreviews[1].transform.position;
            }

            // If we've converged the rift all the way, deactivate null-space actors and meshes
            if (riftTimer > maxRiftTimer)
            {
                for (int i = 0; i < deployedRift.transform.childCount; i++)
                {
                    if (deployedRift.transform.GetChild(i).TryGetComponent<CorGeo_ActorData>(out var actor))
                    {
                        if (actor.activeInNullSpace)
                        {
                            continue;
                        }
                    }
                    deployedRift.transform.GetChild(i).gameObject.SetActive(false);
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
                    if (deployedRift.transform.GetChild(i).TryGetComponent<CorGeo_ActorData>(out var actor))
                    {
                        if (actor.activeInNullSpace)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (deployedRift.transform.GetChild(i).GetComponents<MeshCollider>().Length > 1)
                        {
                            print("THIS OBJECT IS HORDING MESH COLLIDERS! GET 'EM BOYS!");
                            Destroy(deployedRift.transform.GetChild(i).GetComponents<MeshCollider>()[0]);
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

        
        // TODO make this neater tomorrow
        if (!cutPreviews[0])
        {
            CreateCutPreviews();
        }
        
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
    private void CreateCutPreviews()
    {
        cutPreviews = new GameObject[2];
        for (int i = 0; i < cutPreviews.Length; i++)
        {
            cutPreviews[i] = Instantiate (cutPreviewPrefab);
            cutPreviews[i].GetComponent<CutPreviewTracker>().cutPreviewID = i; // Label them so we know whether they are a or b space's side preview
            cutPreviews[i].SetActive (false);
        }
    }
    
    private void RecallInfinityMarkers ()
    {
        if (currentAmmo >= 2) return;
        foreach (var projectile in deployedInfinityMarkers)
        {
            projectile.GetComponent<VacuumProjectile>().KillProjectile(false);
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

        if (isCollapseStarted == false && deployedRift)
        {
            Destroy(deployedRift.gameObject);
            return;
        }
        isCollapseStarted = false;

        foreach (CorGeo_ActorData _actor in CorGeo_ActorDatas)
        {
            if (_actor)
            {
                _actor.GoHome();
            }
        }

        currentAmmo = maxAmmo;

        foreach (var _gameObject in slicedMeshes)
        {
            if (_gameObject) Destroy (_gameObject);
        }
        slicedMeshes.Clear ();
        foreach (ALTMeshSlicer _gameObject in originalSliceableObjects)
        {
            if (_gameObject) _gameObject.GoHome ();
        }

        Destroy (plane2Meshes);
        if (deployedRift)
        {
            foreach (var _gameObject in nullSlices)
            {
                if (_gameObject)
                {
                    _gameObject.GetComponent<ALTMeshSlicer>().GoHome ();
                }
            }
            StartCoroutine (DestroyWorker (deployedRift));
        }
        // Reset the value used to track if an actor is in null space
        foreach (var actor in FindObjectsOfType<CorGeo_ActorData> ())
        {
            actor.nullSpace = false;
        }
        
        StartCoroutine(WitchHunt ());
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
        var projectile = Instantiate (vacuumProjectile, barrelTransform.transform.position, barrelTransform.rotation, null);
        projectile.GetComponent<Rigidbody>().AddForce (projectile.transform.forward * projectileForce, ForceMode.Impulse);
        projectile.GetComponent<VacuumProjectile> ().geoFolder = gameObject; // Get a reference to the gun that spawned the projectile, so we know who to give ammo to on a lifetime expiration
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
                meshSlicers = FindObjectsOfType<ALTMeshSlicer> ();
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
                    //remove extra meshes (there were un-sliced meshes sticking around)
                    var meshColliders = GetComponents<MeshCollider> ();
                    if (meshColliders.Length > 1)
                    {
                        Destroy (meshColliders[0]);
                    }
                }
                AssignActorsToRelativeSpace ();
                isCollapseStarted = true;
                riftTimer = 0f;
                maxRiftTimer = riftWidth * riftSecondsPerUnit;
                previousPlanePosition = cutPreviews[1].transform.position;
            }
        }
    }

    private void AssignActorsToRelativeSpace ()
    {
        if (!deployedRift) return;
        // Reposition objects in A & B space to match the change in distance between the markers

        plane1Objects = new List<CorGeo_ActorData> ();
        plane2Objects = new List<CorGeo_ActorData> ();
        nullSpaceObjects = new List<CorGeo_ActorData> ();

        foreach (var actor in FindObjectsOfType<CorGeo_ActorData> ())
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
            
            // Check to make sure the object we want to add is not the player (This is because the player should not be squished in null space)
            //if (FindObjectOfType<GameInstance>().localPlayerCharacter == actor.GetComponent<Pawn>()) continue;
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

    private void CheckForActorSpaceChanges()
    {
        // Check A-Space entities to see if they have exited A-Space
        // Check B-Space entities to see if they have exited B-Space
        // Check Null-Space entities to see if they have exited Null-Space (This one sucks!)
    }

    private void AimTowardsCenterOfView ()
    {
        RaycastHit viewPoint = new RaycastHit ();
        // Perform the raycast, ignoring the trigger layer
        if (Physics.Raycast(centerViewTransform.position, centerViewTransform.forward, out viewPoint, Mathf.Infinity, viewCastMask))
        {
            // If the raycast hits something, aim the barrel towards the hit point
            barrelTransform.LookAt(viewPoint.point);
        }
    }

    private IEnumerator WitchHunt ()
    {
        var everything = FindObjectsOfType<GameObject> ();
        foreach (GameObject obj in everything) {
            yield return new WaitForEndOfFrame ();
            if (obj && obj.name == "New Game Object")
            {
                    Destroy (obj);
            }
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
