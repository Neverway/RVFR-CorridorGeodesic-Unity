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
    public bool allowExpandingRift = false;

    //=-----------------=
    // Private Variables
    //=-----------------=
    public Vector3 previousPlanePosition;
    private AudioSource_PitchVarienceModulator audioSource;
    private RaycastHit viewPoint; //For aiming projectiles


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private AudioClip projectileTravel, projectileFire;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform centerViewTransform;
    [SerializeField] private GameObject debugObject;
    [FormerlySerializedAs ("vacuumProjectile")][SerializeField] private Projectile_Vacumm projectileVacumm;
    [SerializeField] private GameObject riftObject;
    [SerializeField] private GameObject cutPreviewPrefab;
    public GameObject[] cutPreviews;
    [SerializeField] private float projectileForce;
    [SerializeField] private CrushDetector crushDetector;

    public List<Projectile_Vacumm> deployedInfinityMarkers = new List<Projectile_Vacumm> ();
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
    private Vector3 planeBStartPos;
    private float riftTimer = 0f;
    private float maxRiftTimer;
    public static float lerpAmount;
    [SerializeField] private float riftSecondsPerUnit = 1.8f;
    public static float riftWidth;
    private float minRiftTimer;
    private float maxRiftWidth = 50f;

    private bool secondaryHeld = false;

    private bool isCutPreviewActive = false;
    private bool isCollapseStarted = false;
    public static bool delayRiftCollapse = false;
    private bool forceTweenRift = false;
    private bool delayRiftForCrushDamage = false;

    private float timeRiftHeld = 0f;
    private float maxRiftSpeedMod = 2.5f;
    private float secondsToMaxSpeedMod = 1.3f;
    private float timeMoveRiftButtonHeld = 0f;
    

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
        audioSource = GetComponent<AudioSource_PitchVarienceModulator> ();

        crushDetector.onCrushed.AddListener (()=>StartCoroutine(DelayRift (0.5f)));
    }

    private IEnumerator DelayRift (float delay)
    {
        delayRiftForCrushDamage = true;
        yield return new WaitForSeconds (delay);
        delayRiftForCrushDamage = false;
    }

    public override void UsePrimary ()
    {
        DeployInfinityMarker ();
    }

    public override void UseSecondary ()
    {
        if (!isCollapseStarted)
        {
            SetupForConvergingMarkers ();
        }
        secondaryHeld = true;
    }

    /// <summary>
    /// Pause actors to avoid them being bumped by innaccurate collision meshes
    /// </summary>
    private IEnumerator FreezeActors ()
    {
        delayRiftCollapse = true;
        foreach (CorGeo_ActorData actor in CorGeo_ActorDatas)
        {
            actor.Freeze ();
        }
        yield return null;
        foreach (CorGeo_ActorData actor in CorGeo_ActorDatas)
        {
            actor.UnFreeze ();
        }
        delayRiftCollapse = false;
    }

    public override void ReleaseSecondary ()
    {
        secondaryHeld = false;
    }

    public override void UseSpecial ()
    {
        StartRecallInfinityMarkers ();
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

        bool moveRiftBackwards = false;
        bool moveRift = false;
        if (secondaryHeld)
        {
            moveRiftBackwards = false;
            moveRift = true;
        }
        else if (Input.GetKey (KeyCode.LeftAlt) && allowExpandingRift)
        {
            if (!isCollapseStarted)
            {
                SetupForConvergingMarkers ();
            }
            moveRiftBackwards = true;
            moveRift = true;
        }

        if (delayRiftForCrushDamage)
        {
            moveRift = false;
        }

        if (!moveRift && !forceTweenRift)
        {
            timeMoveRiftButtonHeld = 0f;
        }

        if (forceTweenRift && riftTimer < 0)
        {
            MoveRift (false);
            return;
        }
        if (forceTweenRift && riftTimer > 0)
        {
            MoveRift (true);
        }

        if (isCollapseStarted && deployedRift && riftTimer <= maxRiftTimer)
        {
            if (moveRift && !delayRiftCollapse)
            {
                MoveRift (moveRiftBackwards);
            }
        }
    }

    private void MoveRift (bool moveRiftBackwards = false) //todo: moveRiftBackwards doesn't completely work, we need to re-activate things that were trapped inside of it.
    {
        float speedMod = 1f;

        float cutPreviewDistance = Vector3.Distance (cutPreviews[0].transform.position, cutPreviews[1].transform.position);

        //Calculate speed based on how long we've held the button down.
        if (timeMoveRiftButtonHeld < secondsToMaxSpeedMod)
        {
            //Scales speed up from 1 to maxRiftSpeedMod based on how long button was held
            timeMoveRiftButtonHeld += Time.fixedDeltaTime;
            if (timeMoveRiftButtonHeld > secondsToMaxSpeedMod)
            {
                timeMoveRiftButtonHeld = secondsToMaxSpeedMod;
            }
        }
        speedMod = (timeMoveRiftButtonHeld / secondsToMaxSpeedMod) * (maxRiftSpeedMod - 1) + 1;
        
        if (forceTweenRift) //if rift is being reset, increase the speed modifier.
        {
            speedMod *= 2.5f;
        }

        if (!forceTweenRift && !moveRiftBackwards && cutPreviewDistance < 1f)
        {
            speedMod = 0.8f;
        }
        //Debug.Log ("Sped" + speedMod);

        // If converging, increase the riftTimer and relocate actors and meshes
        if (moveRiftBackwards)
        {
            if (riftTimer >= maxRiftTimer)
            {
                //Un-collapse things in the rift.
                for (int i = 0; i < deployedRift.transform.childCount; i++)
                {
                    deployedRift.transform.GetChild (i).gameObject.SetActive (true);
                }
                foreach (var plane in cutPreviews)
                {
                    plane.SetActive (true);
                }
            }
            foreach (CorGeo_ActorData actor in CorGeo_ActorDatas)
            {
                if (actor.space == CorGeo_ActorData.Space.Null)
                {
                    actor.gameObject.SetActive (true);
                }
            }

            if (riftTimer <= minRiftTimer)
            {
                return;
            }


            riftTimer -= Time.fixedDeltaTime * speedMod;
            if (forceTweenRift && riftTimer < 0)
            {
                riftTimer = 0;
            }
            //Debug.Log (riftTimer);
        }
        else
        {
            //Move rift forwards
            if (riftTimer >= maxRiftTimer)
            {
                return;
            }
            riftTimer += Time.fixedDeltaTime * speedMod;
            if (forceTweenRift && riftTimer > 0)
            {
                riftTimer = 0;
            }

            //If rift is smaller than 0.4f then close it completely.
            if (cutPreviewDistance < 0.4f)
            {
                riftTimer = maxRiftTimer;
            }

            //Debug.Log (riftTimer);
        }

        riftTimer = Mathf.Clamp (riftTimer, minRiftTimer, maxRiftTimer);

        lerpAmount = riftTimer / maxRiftTimer;

        float prevRiftWidth = deployedRift.transform.localScale.z * riftWidth;

        // Squish null-space parent by scaling it
        deployedRift.transform.localScale = new Vector3 (1, 1, 1 - lerpAmount);

        float newRiftWidth = deployedRift.transform.localScale.z * riftWidth;

        // Move meshes relative to planeB/B-Space
        planeBMeshes.transform.position = planeBStartPos + (riftNormal * newRiftWidth - riftNormal * riftWidth);

        // Move actors relative to planeB/B-Space
        Vector3 moveInB = cutPreviews[1].transform.position - previousPlanePosition;
        foreach (CorGeo_ActorData obj in CorGeo_ActorDatas)
        {
            if (obj.space == CorGeo_ActorData.Space.B)
            {

                if (obj.TryGetComponent<Rigidbody> (out var objRigidBody))
                {
                    objRigidBody.MovePosition (obj.transform.position + moveInB);
                }
                else
                {
                    obj.transform.position += moveInB;
                }
            }

            if (obj.space == CorGeo_ActorData.Space.Null && obj.dynamic && !obj.crushInNullSpace)
            {
                //Get object's position relative to the rift, then move the object based on the new size of the rift.
                float percent = planeA.GetDistanceToPoint (obj.transform.position) / prevRiftWidth;
                float oldDistance = prevRiftWidth * percent;
                float newDistance = newRiftWidth * percent;

                Vector3 move = riftNormal * (newDistance - oldDistance);
                if (move.x != float.NaN)
                {
                    if (obj.TryGetComponent<Rigidbody> (out var objRigidBody))
                    {
                        objRigidBody.MovePosition (obj.transform.position + move);
                    }
                    else
                    {
                        obj.transform.position += move;
                    }
                }
            }
        }

        planeB = new Plane (-riftNormal, cutPreviews[1].transform.position);

        previousPlanePosition = cutPreviews[1].transform.position;


        // If we've converged the rift all the way, deactivate null-space actors and meshes
        if (riftTimer >= maxRiftTimer)
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

    private void DeployRiftAndPreview ()
    {

        Vector3 markerPos1 = deployedInfinityMarkers[0].transform.position;
        Vector3 markerPos2 = deployedInfinityMarkers[1].transform.position;
        float markerDistance = Vector3.Distance (markerPos1, markerPos2);

        if (Mathf.Approximately (markerDistance, 0f))
        {
            deployedInfinityMarkers[1].KillProjectile (true);
            return;
        }

        if (markerDistance < .51f)
        {
            return;
        }

        //riftNormal should point from marker 0 to marker 1

        deployedRift = Instantiate (new GameObject ());
        deployedRift.name = "Rift";
        deployedRift.transform.position = deployedInfinityMarkers[0].transform.position;
        deployedRift.transform.LookAt (deployedInfinityMarkers[1].transform);
        if (allowNoLinearSlicing) deployedRift.transform.rotation = new Quaternion (deployedRift.transform.rotation.x, deployedRift.transform.rotation.y, deployedRift.transform.rotation.z, deployedRift.transform.rotation.w);
        else deployedRift.transform.rotation = new Quaternion (0, deployedRift.transform.rotation.y, 0, deployedRift.transform.rotation.w);

        riftNormal = deployedRift.transform.forward;

        float planeOffset = .25f;

        if (markerDistance < planeOffset * 2)
        {
            planeOffset = markerDistance / 2;
        }

        Vector3 pos1 = deployedInfinityMarkers[0].transform.position + riftNormal * planeOffset;
        Vector3 pos2 = deployedInfinityMarkers[1].transform.position - riftNormal * planeOffset;

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

    public void StartRecallInfinityMarkers ()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine (RecallInfinityMarkers ());
        }
    }

    private IEnumerator RecallInfinityMarkers ()
    {
        if (currentAmmo >= 2) yield break;

        timeMoveRiftButtonHeld = 0f;

        while (riftTimer != 0)
        {
            forceTweenRift = true;
            yield return null;
        }
        forceTweenRift = false;

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
            yield break;
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

        Audio_FMODAudioManager.PlayOneShot (Audio_FMODEvents.Instance.nixieCrossShoot);

        var projectile = Instantiate (projectileVacumm, centerViewTransform.transform.position, centerViewTransform.rotation, null);
        projectile.InitializeProjectile (projectileForce, barrelTransform.position, viewPoint.distance);
        projectile.geoGun = this; // Get a reference to the gun that spawned the projectile, so we know who to give ammo to on a lifetime expiration
        deployedInfinityMarkers.Add (projectile);
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

    private void SetupForConvergingMarkers ()
    {
        if (AreMarkersPinned ())
        {
            if (deployedRift)
            {
                StartCoroutine (FreezeActors ());
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
                if (riftWidth > maxRiftWidth)
                {
                    minRiftTimer = 0;
                }
                else
                {
                    minRiftTimer = -(maxRiftTimer * ((maxRiftWidth - riftWidth) / riftWidth));
                }
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
            if (!actor.gameObject.activeInHierarchy)
            {
                continue;
            }
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
        viewPoint = new RaycastHit ();
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
