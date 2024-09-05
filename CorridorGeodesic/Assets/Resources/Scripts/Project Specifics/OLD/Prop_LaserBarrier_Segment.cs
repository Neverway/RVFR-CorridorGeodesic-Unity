//===================== (Neverway 2024) Written by Connorses =====================
//
// Purpose: Shoots a raycast and scales Laser Wall based on the distance of the raycast.
// Notes: LayerMask on the prefab is configured to hit certain layers, ignoring the player, projectiles and other laserwalls.
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop_LaserBarrier_Segment : MonoBehaviour
{

    //=-----------------=
    // Private Variables
    //=-----------------=

    [SerializeField] private GameObject laserWall;
    [SerializeField] private LayerMask layerMask;
    RaycastHit hit;
    private Vector3 extents = new Vector3 (0.05f, 1f, 0.05f);
    private bool active = true;

    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Update()
    {
        if (!active)
        {
            return;
        }
        Physics.Raycast (transform.position, transform.forward, out hit, 100f, layerMask);
        laserWall.transform.localScale = new Vector3 (1, .05f, hit.distance);
    }

    public void SetActive (bool _active)
    {
        active = _active;
        laserWall.SetActive (active);
    }

}