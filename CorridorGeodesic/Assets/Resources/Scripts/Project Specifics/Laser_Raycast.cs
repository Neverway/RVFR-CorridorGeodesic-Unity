using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Raycast : MonoBehaviour
{

    //===================== (Neverway 2024) Written by Connorses =====================
    //
    // Purpose:
    // Notes:
    // Source:
    //
    //=============================================================================

    private LineRenderer lineRenderer;
    [SerializeField] private Transform halo;

    private void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, 100f))
        {
            Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.forward) * hit.distance, Color.red);
            lineRenderer.SetPosition (0, transform.position);
            lineRenderer.SetPosition (1, hit.point);
            halo.gameObject.SetActive (true);
            halo.transform.position = hit.point;
        }
        else
        {
            halo.gameObject.SetActive (false);
        }
    }

}
