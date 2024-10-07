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
    public static Vector3 RiftToWorld(Vector3 position)
    {
        return position;
    }
    public static Vector3 WorldToRift(Vector3 position)
    {
        return position;
    }
}
//public struct SpatialHash<T>
//{
//    public T hashedObject;
//    public Vector3 hash;
//    public SpatialHash(Vector3 input, float unitSize, T hashedObject)
//    {
//        hash = SX_UnityHelpers.CreateHash(input, unitSize);
//        this.hashedObject = hashedObject;
//    }
//    public SpatialHash(Vector3 hash, T hashedObject)
//    {
//        this.hash = hash;
//        this.hashedObject = hashedObject;
//    }
//    public void SetHashedObject(T hashedObject)
//    {
//        this.hashedObject = hashedObject;
//    }
//}
public struct SpatialHashMap<T>
{
    public int Count => HashMap.Count;

    //public List<SpatialHash<T>> HashMap;
    public Dictionary<Vector3, T> HashMap;
    public float UnitSize;

    public SpatialHashMap(float unitSize)
    {
        this.UnitSize = unitSize;
        //HashMap = new List<SpatialHash<T>>();
        HashMap = new Dictionary<Vector3, T>();
    }
    public void AddToSpatialHash(T hashedObject, Vector3 position, bool autoGenerateHash = true)
    {
        Vector3 hash = position;

        if (autoGenerateHash)
            hash = SX_UnityHelpers.CreateHash(position, UnitSize);

        if (!HashMap.ContainsKey(hash))
            HashMap.Add(hash, hashedObject);
        else
        {
            //SpatialHash<T> spatialHash = HashMap.Find(h => h.hash == hash);
            //spatialHash.SetHashedObject(hashedObject);
            HashMap[hash] = hashedObject;
        }
    }
    public bool TryGetFromSpatialHash(Vector3 position, out T hashedObject)
    {
        Vector3 hash = SX_UnityHelpers.CreateHash(position, UnitSize);

        if(!HashMap.ContainsKey(hash))
        {
            hashedObject = default;
            return false;
        }
        else
        {
            hashedObject = HashMap[hash];
            return true;
        }

        //SpatialHash<T> spatialHash = HashMap.Find(h => h.hash == hash);

        //if (spatialHash.Equals(null))
        //{
        //    hashedObject = default;
        //    return false;
        //}
        //else
        //{
        //    hashedObject = spatialHash.hashedObject;
        //    return true;
        //}
    }
    public bool GetHashFromHashedObject(T hashedObject, out Vector3 hash)
    {
        //return HashMap.First(h=>h.hashedObject.Equals(hashedObject)).hash;
        if (HashMap.ContainsValue(hashedObject))
        {
            hash = HashMap.First(k => k.Value.Equals(hashedObject)).Key;
            return true;
        }
        else
        {
            hash = default;
            return false;
        }
    }
    public void RemoveFromHashedObject(T hashedObject)
    {
        if (GetHashFromHashedObject(hashedObject, out Vector3 hash))
            HashMap.Remove(hash);

        //if(HashMap.Exists(h=>h.hashedObject.Equals(hashedObject)))
        //    HashMap.Remove(HashMap.First(h=>h.hashedObject.Equals(hashedObject)));
    }
}