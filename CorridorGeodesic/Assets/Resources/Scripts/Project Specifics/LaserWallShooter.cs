//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWallShooter : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=

    [SerializeField] private GameObject laserWall;
    [SerializeField] private LayerMask layerMask;
    RaycastHit hit;
    private Vector3 extents = new Vector3 (0.05f, 1f, 0.05f);

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
    }

    private void Update()
    {
        Physics.Raycast (transform.position, transform.forward, out hit, 100f, layerMask);
        laserWall.transform.localScale = new Vector3 (1, .05f, hit.distance);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}