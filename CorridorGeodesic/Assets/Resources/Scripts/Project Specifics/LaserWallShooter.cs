//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose: Shoots a raycast and scales Laser Wall based on the distance of the raycast.
// Notes: LayerMask on the prefab is configured to hit certain layers, ignoring the player, projectiles and other laserwalls.
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWallShooter : MonoBehaviour
{

    //=-----------------=
    // Private Variables
    //=-----------------=

    [SerializeField] private GameObject laserWall;
    [SerializeField] private LayerMask layerMask;
    RaycastHit hit;
    private Vector3 extents = new Vector3 (0.05f, 1f, 0.05f);

    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Update()
    {
        Physics.Raycast (transform.position, transform.forward, out hit, 100f, layerMask);
        laserWall.transform.localScale = new Vector3 (1, .05f, hit.distance);
    }
}