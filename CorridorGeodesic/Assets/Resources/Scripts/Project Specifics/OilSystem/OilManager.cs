//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static OilManager Instance;
    public float oilLifetime = 5;

    //=-----------------=
    // Private Variables
    //=-----------------=
    private SpatialHashMap<OilVoxel> oilHashMap;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        Instance = this;
        oilHashMap = new SpatialHashMap<OilVoxel>(1);
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < oilHashMap.hashMap.Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(oilHashMap.hashMap[i].hash, Vector3.one);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void AddOilSplatter(Transform splatter)
    {
        OilVoxel oilVoxel = oilHashMap.GetFromSpatialHash(splatter.position);
        if (oilVoxel.Equals(default))
        {
            OilVoxel voxel = new OilVoxel();
            voxel.associatedSplatters.Add(splatter);

            oilHashMap.AddToSpatialHash(voxel, splatter.position);
        }
        else
            oilVoxel.associatedSplatters.Add(splatter);
    }
}

public class OilVoxel
{
    public List<Transform> associatedSplatters = new List<Transform>();
    private float lifetime = 1;
    public void BurnTick()
    {
        lifetime -= Time.deltaTime * (1 / OilManager.Instance.oilLifetime);
    }
    public void ResetLifetime()
    {
        lifetime = 1;
    }
}