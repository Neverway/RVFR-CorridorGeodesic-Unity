//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SX_UnityHelpers
{
    public static Vector3Int CreateHash(Vector3 input, int unitSize)
    {
        return new Vector3Int(Mathf.FloorToInt(input.x / unitSize), Mathf.FloorToInt(input.y / unitSize), Mathf.FloorToInt(input.z / unitSize));
    }
}
public struct SpatialHash<T> where T : class
{
    public T hashedObject;
    public Vector3Int hash;
    public SpatialHash(Vector3 input, int unitSize, T hashedObject)
    {
        hash = SX_UnityHelpers.CreateHash(input, unitSize);
        this.hashedObject = hashedObject;
    }
    public SpatialHash(Vector3Int hash, T hashedObject)
    {
        this.hash = hash;
        this.hashedObject = hashedObject;
    }
    public void SetHashedObject(T hashedObject)
    {
        this.hashedObject = hashedObject;
    }
}
public struct SpatialHashMap<T> where T : class
{
    public List<SpatialHash<T>> hashMap;
    public int unitSize;

    public SpatialHashMap(int unitSize)
    {
        this.unitSize = unitSize;
        hashMap = new List<SpatialHash<T>>();
    }
    public void AddToSpatialHash(T hashedObject, Vector3 position)
    {
        Vector3Int hash = SX_UnityHelpers.CreateHash(position, unitSize);

        if (!hashMap.Exists(h => h.hash == hash))
            hashMap.Add(new SpatialHash<T>(hash, hashedObject));
        else
        {
            SpatialHash<T> spatialHash = hashMap.Find(h => h.hash == hash);
            spatialHash.SetHashedObject(hashedObject);
        }
    }
    public T GetFromSpatialHash(Vector3 position)
    {
        Vector3Int hash = SX_UnityHelpers.CreateHash(position, unitSize);
        SpatialHash<T> spatialHash = hashMap.Find(h => h.hash == hash);

        if (spatialHash.Equals(null))
            return null;
        else
            return spatialHash.hashedObject;
    }
}