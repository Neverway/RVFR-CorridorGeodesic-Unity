using System;
using System.Net.Http.Headers;
using UnityEngine;

public class Laser_Raycast : MonoBehaviour
{

    //===================== (Neverway 2024) Written by Connorses =====================
    //
    // Purpose: Recursively bounces a raycast off reflective surfaces. Also places laser prefabs to render each line/halo effect.
    // Notes: Checks for the IsReflective component when deciding if it can bounce.
    // Source: https://stackoverflow.com/questions/51931455/unity3d-bouncing-reflecting-raycast
    //
    //=============================================================================

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Transform halo;
    [SerializeField] private int maxReflectionCount = 5;
    [SerializeField] private float maxStepDistance = 200f;
    [SerializeField] private LineRenderer prefabLine;
    public bool isPowered;
    public Transform laserFireTransform;
    
    //=-----------------=
    // Private Variables
    //=-----------------=
    private LineRenderer[] lineRenderers;

    private void Start ()
    {
        lineRenderers = new LineRenderer[maxReflectionCount];
        for (int i = 0; i < maxReflectionCount; i++)
        {
            lineRenderers[i] = Instantiate (prefabLine);
            //lineRenderers[i].gameObject.SetActive (false);
        }
    }

    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Update ()
    {
        ShootLaser ();
    }

    private void OnDisable ()
    {
        foreach (var line in lineRenderers)
        {
            line.gameObject.SetActive (false);
        }
    }

    private void OnDestroy ()
    {
        foreach (var line in lineRenderers)
        {
            try
            {
                Destroy(line.gameObject);
            }
            catch (Exception)
            {            
                Debug.LogWarning("A Known bug has occured. Blame Liz.");
                //throw;
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private void ShootLaser ()
    {
        foreach (var l in lineRenderers)
        {
            l.gameObject.SetActive (false);
        }
        if (!isPowered)
        {
            return;
        }
        DrawReflectionPattern (laserFireTransform.position + laserFireTransform.forward * 0.75f, laserFireTransform.forward, maxReflectionCount);
    }

    private void DrawReflectionPattern (Vector3 position, Vector3 direction, int reflectionsRemaining, bool bounce = true)
    {
        if (reflectionsRemaining == 0)
        {
            return;
        }
        //if one of the hits didn't bounce then the rest of the recursions just set the linerenderers inactive.
        if (!bounce)
        {
            lineRenderers[reflectionsRemaining - 1].gameObject.SetActive (false);
            DrawReflectionPattern (position, direction, reflectionsRemaining - 1, bounce);
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

            if (hit.collider.gameObject.TryGetComponent<ALTMeshSlicer> (out var slicer))
            {
                if (!slicer.isReflective)
                {
                    bounce = false;
                }
                OnHit(hit);
            }
            else if (hit.collider.gameObject.TryGetComponent<Laser_Detector> (out var detector))
            {
                bounce = false;
                OnHit(hit);
            }
            else
            {
                bounce = false;
            }
        }
        else
        {
            bounce = false;
            position += direction * maxStepDistance;
        }

        Debug.DrawLine (startingPosition, position, Color.blue);

        LineRenderer line = lineRenderers[reflectionsRemaining - 1];
        if (!line.gameObject.activeSelf)
            line.gameObject.SetActive (true);
        line.transform.position = position;
        line.SetPosition (0, startingPosition);
        line.SetPosition (1, position);

        DrawReflectionPattern (position, direction, reflectionsRemaining - 1, bounce);
    }

    private void OnHit (RaycastHit hit)
    {
        if (hit.collider.gameObject.TryGetComponent<Laser_Detector> (out var detector))
        {
            detector.OnHit ();
        }
    }

    public void SetIsPowered(bool _isPowered)
    {
        isPowered = _isPowered;
    }

}
