using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager Instance;

    public List<Material> conductiveMats = new List<Material>();

    private void Awake()
    {
        Instance = this;
    }
}