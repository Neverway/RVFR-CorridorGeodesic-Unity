using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DOMAIN_RELOAD_STATIC_FIELD_RESETS
{
    [RuntimeInitializeOnLoadMethod]
    public static void RESET_STATIC_FIELDS()
    {
        //todo: Make sure WorldLoader.OnWorldLoaded is working fine, not sure what references it
        //WorldLoader.OnWorldLoaded

        Alt_Item_Geodesic_Utility_GeoGun.deployedRift = null;
        Alt_Item_Geodesic_Utility_GeoGun.nullSlices = null;
        Alt_Item_Geodesic_Utility_GeoGun.planeBMeshes = null;
        Alt_Item_Geodesic_Utility_GeoGun.originalSliceableObjects = new List<Mesh_Slicable>();
        Alt_Item_Geodesic_Utility_GeoGun.slicedMeshes = new List<GameObject>();
        Alt_Item_Geodesic_Utility_GeoGun.CorGeo_ActorDatas = new List<CorGeo_ActorData>();
        Alt_Item_Geodesic_Utility_GeoGun.lerpAmount = 0;
        Alt_Item_Geodesic_Utility_GeoGun.riftWidth = 0;
        Alt_Item_Geodesic_Utility_GeoGun.delayRiftCollapse = false;
        Alt_Item_Geodesic_Utility_GeoGun.delayRiftCollapse = false;
        Alt_Item_Geodesic_Utility_GeoGun.previousState = RiftState.None;
        Alt_Item_Geodesic_Utility_GeoGun.currentState = RiftState.None;

        TeslaManager.lightningLinePrefab = null;
        TeslaManager.senders = new List<TeslaSender>();
        TeslaManager.conductors = new List<TeslaConductor>();

        GeolitePanel.count = 0; //todo: Make this private again to avoid tampering from other scripts

        Graphics_NixieBulbEffects.firstBulb = null;

        Checkpoint.lastCheckpointName = null;
        Checkpoint.lastCheckpointRank = -1;

        LemonBuddyDestroyedTracker.scenesBuddyWasDestroyedIn = new List<int>();

        EnableDisableSliceable.count = 0; //todo: Make this private again to avoid tampering from other scripts

        //GameInstance.Instance = GameObject.FindObjectOfType<GameInstance>();
        //Game_LevelStartIdentifier.Instance = null;
    }
}
