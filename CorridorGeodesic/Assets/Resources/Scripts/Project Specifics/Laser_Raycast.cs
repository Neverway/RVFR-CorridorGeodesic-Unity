using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Raycast : MonoBehaviour
{

    //===================== (Neverway 2024) Written by Connorses =====================
    //
    // Purpose: Lines up a line renderer with a raycast aimed at the gameobject's forward vector, and places halo objects at both ends.
    // Notes:
    // Source:
    //
    //=============================================================================

    [SerializeField] private Transform halo;
    [SerializeField] private int maxReflectionCount = 5;
    [SerializeField] private float maxStepDistance = 200f;
    [SerializeField] private LineRenderer prefabLine;
    private LineRenderer[] lineRenderers;
   
    private void Start ()
    {
        lineRenderers = new LineRenderer[maxReflectionCount];
        for (int i = 0; i < maxReflectionCount; i++)
        {
            lineRenderers[i] = Instantiate (prefabLine);
            lineRenderers[i].gameObject.SetActive (false);
        }
    }

    private void Update ()
    {
        ShootLaser ();
        /*
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
        */
    }

    private void ShootLaser ()
    {
        foreach (var l in lineRenderers)
        {
            l.gameObject.SetActive (false);
        }
        DrawReflectionPattern (this.transform.position + this.transform.forward * 0.75f, this.transform.forward, maxReflectionCount);
    }

    private void DrawReflectionPattern (Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0)
        {
            return;
        }

        Vector3 startingPosition = position;

        Ray ray = new Ray (position, direction);
        RaycastHit hit;
        bool didHit = Physics.Raycast (ray, out hit, maxStepDistance);
        if (didHit)
        {
            direction = Vector3.Reflect (direction, hit.normal);
            position = hit.point;
        }
        else
        {
            position += direction * maxStepDistance;
        }

        Debug.DrawLine (startingPosition, position, Color.blue);

        LineRenderer line = lineRenderers[reflectionsRemaining - 1];
        line.gameObject.SetActive (true);
        line.transform.position = position;
        line.SetPosition (0, startingPosition);
        line.SetPosition (1, position);

        if (didHit)
        {
            DrawReflectionPattern (position, direction, reflectionsRemaining - 1);
        }
    }

}
