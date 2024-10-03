//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SX_UnityHelpers
{
    public static Vector3 CreateHash(Vector3 input, float unitSize)
    {
        return new Vector3(Mathf.FloorToInt(input.x / unitSize), Mathf.FloorToInt(input.y / unitSize), Mathf.FloorToInt(input.z / unitSize));
    }
}
public struct SpatialHash<T>
{
    public T hashedObject;
    public Vector3 hash;
    public SpatialHash(Vector3 input, float unitSize, T hashedObject)
    {
        hash = SX_UnityHelpers.CreateHash(input, unitSize);
        this.hashedObject = hashedObject;
    }
    public SpatialHash(Vector3 hash, T hashedObject)
    {
        this.hash = hash;
        this.hashedObject = hashedObject;
    }
    public void SetHashedObject(T hashedObject)
    {
        this.hashedObject = hashedObject;
    }
}
public struct SpatialHashMap<T>
{
    public int Count => HashMap.Count;

    public List<SpatialHash<T>> HashMap;
    public float UnitSize;

    public SpatialHashMap(float unitSize)
    {
        this.UnitSize = unitSize;
        HashMap = new List<SpatialHash<T>>();
    }
    public void AddToSpatialHash(T hashedObject, Vector3 position, bool autoGenerateHash = true)
    {
        Vector3 hash = position;

        if (autoGenerateHash)
            hash = SX_UnityHelpers.CreateHash(position, UnitSize);

        if (!HashMap.Exists(h => h.hash == hash))
            HashMap.Add(new SpatialHash<T>(hash, hashedObject));
        else
        {
            SpatialHash<T> spatialHash = HashMap.Find(h => h.hash == hash);
            spatialHash.SetHashedObject(hashedObject);
        }
    }
    public bool TryGetFromSpatialHash(Vector3 position, out T hashedObject)
    {
        Vector3 hash = SX_UnityHelpers.CreateHash(position, UnitSize);

        if(!HashMap.Exists(h=>h.hash == hash))
        {
            hashedObject = default;
            return false;
        }

        SpatialHash<T> spatialHash = HashMap.Find(h => h.hash == hash);

        if (spatialHash.Equals(null))
        {
            hashedObject = default;
            return false;
        }
        else
        {
            hashedObject = spatialHash.hashedObject;
            return true;
        }
    }
    public Vector3 GetHashFromHashedObject(T hashedObject)
    {
        return HashMap.First(h=>h.hashedObject.Equals(hashedObject)).hash;
    }
    public void RemoveFromHashedObject(T hashedObject)
    {
        if(HashMap.Exists(h=>h.hashedObject.Equals(hashedObject)))
            HashMap.Remove(HashMap.First(h=>h.hashedObject.Equals(hashedObject)));
    }
}