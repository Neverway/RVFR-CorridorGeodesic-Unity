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

public class Tool_ObjectPool: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static Tool_ObjectPool Instance;

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private int maxDecalAmount = 1000;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    private List<GameObject> pooledObjects = new List<GameObject>();
    private List<GameObject> decalObjects = new List<GameObject>();

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        Instance = this;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void ResetPool()
    {
        pooledObjects.Clear();
        decalObjects.Clear();
    }
    public GameObject GetPooledObject(GameObject obj, Vector3 pos, Quaternion rot)
    {
        string objName = obj.name + "(Clone)";
        if (!AllObjectsInUse(objName, pooledObjects))
        {
            GameObject returnObj = pooledObjects.First(g => !g.activeSelf && g.name == objName);

            returnObj.transform.SetPositionAndRotation(pos, rot);

            returnObj.SetActive(true);
            return returnObj;
        }
        else
        {
            GameObject newObj = Instantiate(obj, pos, rot);

            if (newObj.transform.position != pos || newObj.transform.rotation != rot)
                newObj.transform.SetPositionAndRotation(pos, rot);

            pooledObjects.Add(newObj);
            return newObj;
        }
    }
    public GameObject AddToDecalPool(GameObject obj, Vector3 pos, Quaternion rot)
    {
        string objName = obj.name + "(Clone)";
        GameObject returnObj;
        if (decalObjects.Count >= maxDecalAmount)
        {
            returnObj = decalObjects.Last(d => d.name == objName);

            returnObj.transform.localScale = Vector3.one;
            returnObj.transform.SetPositionAndRotation(pos, rot);

            returnObj.SetActive(true);
        }
        else if (!AllObjectsInUse(objName, decalObjects))
        {
            returnObj = decalObjects.First(d => !d.activeSelf && d.name == objName);

            returnObj.transform.localScale = Vector3.one;
            returnObj.transform.SetPositionAndRotation(pos, rot);

            returnObj.SetActive(true);
        }
        else
        {
            returnObj = Instantiate(obj, pos, rot);
            decalObjects.Add(returnObj);
        }
        return returnObj;
    }
    bool AllObjectsInUse(string name, List<GameObject> list)
    {
        list.RemoveAll(g => g == null);
        if (list.Count > 0)
            return list.FindAll(g => g.name == name).All(g => g.activeSelf);
        return true;
    }
}