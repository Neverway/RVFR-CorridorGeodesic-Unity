//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using static SX_UnityHelpers;

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
    public SpatialHashMap<bool> decalHashMap;

    private List<Vector3> flameStartPositions = new List<Vector3>();
    [SerializeField] private List<OilVoxel> burningVoxels = new List<OilVoxel>();

    //private bool oilHashUpdated;

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
        decalHashMap = new SpatialHashMap<bool>(0.5f);

        StartCoroutine(BurnTick());
    }
    private void OnDrawGizmos()
    {
        if (oilHashMap.Equals(default(SpatialHashMap<OilVoxel>)))
            return;

        Gizmos.color = Color.green;
        for (int i = 0; i < oilHashMap.Count; i++)
        {
            Vector3 position = (oilHashMap.HashMap.ElementAt(i).Key + Vector3.one * 0.5f) * oilHashMap.UnitSize;
            if (oilHashMap.HashMap.ElementAt(i).Value.burning)
                Gizmos.DrawCube(position, Vector3.one * oilHashMap.UnitSize);
            else
                Gizmos.DrawWireCube(position, Vector3.one * oilHashMap.UnitSize);
        }

        if (decalHashMap.Equals(default(SpatialHashMap<bool>)))
            return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < decalHashMap.Count; i++)
        {
            Gizmos.DrawWireCube((decalHashMap.HashMap.ElementAt(i).Key + Vector3.one * 0.5f) * decalHashMap.UnitSize, 
                Vector3.one * decalHashMap.UnitSize);
        }

        if (flameStartPositions == null || flameStartPositions.Count <= 0)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i < flameStartPositions.Count; i++)
        {
            Gizmos.DrawWireCube((flameStartPositions[i] + Vector3.one * 0.5f) * oilHashMap.UnitSize, Vector3.one * oilHashMap.UnitSize);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    IEnumerator BurnTick()
    {
        List<OilVoxel> voxelsToRemove = new List<OilVoxel>();
        while (true)
        {
            if (oilHashMap.Count <= 0 || burningVoxels.Count <= 0)
                yield return null;

            oilHashMap.HashMap.Values.ToList().ForEach(v =>
            {
                if (v.lifetime <= 0)
                {
                    voxelsToRemove.Add(v);
                    v.DeInitialize();
                }
                else if(v.burning)
                {
                    v.Burn();
                }
            });

            for (int i = 0; i < voxelsToRemove.Count; i++)
            {
                oilHashMap.RemoveFromHashedObject(voxelsToRemove[i]);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void StartFlame(Vector3 position)
    {
        Vector3 hashCenter = CreateHash(position, oilHashMap.UnitSize);
        Vector3 offset = Vector3.zero;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    offset.x = x;
                    offset.y = y;
                    offset.z = z;

                    if(oilHashMap.TryGetFromSpatialHash(hashCenter + offset, out OilVoxel voxel) && !burningVoxels.Contains(voxel))
                    {
                        StartCoroutine(BurnDelay(voxel));
                    }
                }
            }
        }
    }
    public void StartFlameSingle(Vector3 position)
    {
        if (oilHashMap.TryGetFromSpatialHash(position, out OilVoxel voxel) && !burningVoxels.Contains(voxel))
            StartCoroutine(BurnDelay(voxel));
    }
    public bool BurnAtPosition(Vector3 position)
    {
        Vector3 hashCenter = CreateHash(position, oilHashMap.UnitSize);
        Vector3 offset = Vector3.zero;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    offset.x = x;
                    offset.y = y;
                    offset.z = z;

                    Vector3 checkPosition = hashCenter + offset;

                    if (oilHashMap.TryGetFromSpatialHash(checkPosition, out OilVoxel voxel) && voxel.burning)
                        return true;
                }
            }
        }

        return false;
    }
    public bool BurnAtPositionSingle(Vector3 position)
    {
        Vector3 hashCenter = CreateHash(position, oilHashMap.UnitSize);
        return oilHashMap.TryGetFromSpatialHash(hashCenter, out OilVoxel voxel) && voxel.burning;
    }
    public void AddToBurningVoxels(OilVoxel voxel)
    {
        burningVoxels.Add(voxel);
    }
    IEnumerator BurnDelay(OilVoxel voxel)
    {
        yield return new WaitForSeconds(0.05f);
        voxel.burning = true;
        burningVoxels.Add(voxel);
    }
    public void AddFlame(OilVoxel voxel)
    {
        Vector3 position = (voxel.hash + new Vector3(1, 0, 1) * 0.5f) * oilHashMap.UnitSize;

        voxel.flame = Tool_ObjectPool.Instance.GetPooledObject(CorGeo_ReferenceManager.Instance.flameParticles, position, Quaternion.identity).transform;
    }
    public bool DecalValid(Vector3 position)
    {
        decalHashMap.TryGetFromSpatialHash(position, out bool containsDecal);

        if (!containsDecal)
            decalHashMap.AddToSpatialHash(true, position);

        return !containsDecal;
    }
    public void AddOilSplatter(Transform splatter)
    {
        oilHashMap.TryGetFromSpatialHash(splatter.position, out OilVoxel oilVoxel);

        if (oilVoxel == null)
        {
            OilVoxel voxel = new OilVoxel(CreateHash(splatter.position, oilHashMap.UnitSize));

            voxel.associatedSplatters.Add(splatter);

            oilHashMap.AddToSpatialHash(voxel, voxel.hash, false);

            voxel.Update();
        }
        else
        {
            oilVoxel.AddSplatter(splatter);
            oilVoxel.ResetLifetime();
            oilVoxel.Update();
        }
    }
    public void ResetLifeTimeOfOilVoxel(Vector3 position)
    {
        if(oilHashMap.TryGetFromSpatialHash(position, out OilVoxel oilVoxel))
        {
            oilVoxel.ResetLifetime();
        }
    }
}

[System.Serializable]
public class OilVoxel
{
    public Vector3 hash;
    public List<Vector3> DecalHashes = new List<Vector3>();

    public List<Transform> associatedSplatters = new List<Transform>();
    public Transform flame;

    public float lifetime = 1;
    private float startScale = 2;

    public bool _burning = false;
    public OilVoxel(Vector3 hash)
    {
        this.hash = hash;
        Update();
    }
    public bool burning
    {
        get {  return _burning; }
        set
        {
            if(_burning == false && value)
            {
                OilManager.Instance.AddFlame(this);
                OilManager.Instance.AddToBurningVoxels(this);
            }
            _burning = value;

            Update();
        }
    }
    public void Burn()
    {
        lifetime -= Time.deltaTime * (1 / OilManager.Instance.oilLifetime);

        Vector3 scale = Vector3.Lerp(Vector3.one, Vector3.zero, Mathf.Clamp01(1 - lifetime - 0.5f) * 2);

        flame.localScale = scale;

        associatedSplatters.ForEach(s =>
        {
            s.localScale = scale * startScale;
        });
    }
    public void Update()
    {
        if (burning)
        {
            OilManager.Instance.StartFlame(hash);
        }
        else if (OilManager.Instance.BurnAtPosition(hash))
        {
            burning = true;
            OilManager.Instance.AddToBurningVoxels(this);
        }
    }
    public void AddSplatter(Transform splatter)
    {
        associatedSplatters.Add(splatter);
    }
    public void DeInitialize()
    {
        associatedSplatters.ForEach(s=> 
        {
            OilManager.Instance.decalHashMap.HashMap.Remove(CreateHash(s.position, OilManager.Instance.decalHashMap.UnitSize));
            s.gameObject.SetActive(false);
        });
        associatedSplatters.Clear();
    }
    public void ResetLifetime()
    {
        lifetime = 1;
    }
}