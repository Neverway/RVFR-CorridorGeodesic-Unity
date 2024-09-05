using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager Instance;

    [Header("Materials")]
    public List<Material> conductiveMats = new List<Material>();
    public Material nullSpace;

    [HideInInspector] public LayerMask playerIgnoreMask;
    [HideInInspector] public LayerMask playerProjectileIgnoreMask;

    private void Awake()
    {
        Instance = this;

        playerProjectileIgnoreMask = ~LayerMask.GetMask("Ignore Raycast", "Trigger", "Ignore Player", "Ignore Projectile");
    }
}