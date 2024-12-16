//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework.PawnManagement;

public class Rift_Audio : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Transform playerTransform;
    private CorGeo_ActorData playerActorData;
    private Vector3 camPos => Camera.main.transform.position;

    private EventInstance riftIdleInstance;
    private EventInstance riftCollapseInstance;
    private EventInstance riftExpandInstance;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        playerTransform = FindAnyObjectByType<Player>().transform;
        playerActorData = playerTransform.gameObject.GetComponent<CorGeo_ActorData>();

        riftIdleInstance = Audio_FMODAudioManager.CreateInstance(Audio_FMODEvents.Instance.riftIdle);
        riftCollapseInstance = Audio_FMODAudioManager.CreateInstance(Audio_FMODEvents.Instance.riftCollapsing);
        riftExpandInstance = Audio_FMODAudioManager.CreateInstance(Audio_FMODEvents.Instance.riftExpanding);
    }
    private void OnEnable()
    {
        Alt_Item_Geodesic_Utility_GeoGun.OnStateChanged += OnStateChanged;
    }
    private void OnDisable()
    {
        Alt_Item_Geodesic_Utility_GeoGun.OnStateChanged -= OnStateChanged;
    }
    private void OnDestroy()
    {
        riftIdleInstance.release();
        riftCollapseInstance.release();
        riftExpandInstance.release();
    }

    private void Update()
    {
        Update3DAttributes();

        if (Alt_Item_Geodesic_Utility_GeoGun.currentState == RiftState.None)
        {
            transform.position = camPos;
            return;
        }

        transform.position = GetAudioClosestPosition();
    }
    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void OnStateChanged ()
    {
        if (Alt_Item_Geodesic_Utility_GeoGun.currentState != RiftState.None && Alt_Item_Geodesic_Utility_GeoGun.previousState == RiftState.None)
        {
            OnRiftCreated();
        }

        bool collapseStart = false;
        bool expandStart = false;

        switch (Alt_Item_Geodesic_Utility_GeoGun.currentState)
        {
            case RiftState.None:
                OnRiftRemoved();
                break;
            case RiftState.Preview:
                break;
            case RiftState.Collapsing:
                collapseStart = true;
                break;
            case RiftState.Closed:
                break;
            case RiftState.Expanding:
                expandStart = true;
                break;
            case RiftState.Idle:
                break;
            default:
                break;
        }

        if (collapseStart)
            riftCollapseInstance.start();
        else
            riftCollapseInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        if (expandStart)
            riftExpandInstance.start();
        else
            riftExpandInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    private Vector3 GetAudioClosestPosition()
    {
        Vector3 closestPoint = camPos;

        if(playerActorData.space == CorGeo_ActorData.Space.Null)
            return closestPoint;

        Vector3 planeAAlignment = Alt_Item_Geodesic_Utility_GeoGun.planeA.ClosestPointOnPlane(camPos);
        Vector3 planeBAlignment = Alt_Item_Geodesic_Utility_GeoGun.planeB.ClosestPointOnPlane(camPos);

        if ((planeAAlignment - camPos).sqrMagnitude < (planeBAlignment - camPos).sqrMagnitude)
            closestPoint = planeAAlignment;
        else
            closestPoint = planeBAlignment;

        return closestPoint;
    }
    private void Update3DAttributes()
    {
        FMOD.ATTRIBUTES_3D attributes = FMODUnity.RuntimeUtils.To3DAttributes(transform.position);

        riftIdleInstance.set3DAttributes(attributes);
        riftCollapseInstance.set3DAttributes(attributes);
        riftExpandInstance.set3DAttributes(attributes);
    }
    private void OnRiftCreated()
    {
        //Put code here for when rift first starts moving
        Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.riftSpawned);

        riftIdleInstance.start();
    }
    private void OnRiftRemoved()
    {
        //Put code here for when rift is cleared.
        Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.riftKilled);

        riftIdleInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    //=-----------------=
    // External Functions
    //=-----------------=
}