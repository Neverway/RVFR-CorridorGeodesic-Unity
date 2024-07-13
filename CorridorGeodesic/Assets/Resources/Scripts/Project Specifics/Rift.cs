//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rift : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public List<GameObject> objectsInASpace;
    public List<GameObject> objectsInBSpace;
    public List<GameObject> objectsInNullSpace;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public GameObject CenterPlane;
    public GameObject portalA, portalB;
    public GameObject visualPlaneA, visualPlaneB;
    public float distanceToMarker;
    

    //=-----------------=
    // Mono Functions
    //=-----------------=
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void DivergePortals(float speed)
    {
        portalA.transform.localPosition = Vector3.Lerp(portalA.transform.localPosition, -Vector3.forward*distanceToMarker, speed*Time.deltaTime);
        portalB.transform.localPosition = Vector3.Lerp(portalB.transform.localPosition, Vector3.forward*distanceToMarker, speed*Time.deltaTime);
    }

    public void GetObjectSpacialLists()
    {
        objectsInASpace.Clear();
        objectsInBSpace.Clear();
        objectsInNullSpace.Clear();
        
        // Get the plane's normal and its point in space
        Plane plane = new Plane(CenterPlane.transform.up, CenterPlane.transform.position);
        
        foreach (var actor in FindObjectsOfType<ActorData>())
        {
            Vector3 actorPosition = actor.transform.position;

            // Determine the side of the plane
            float distance = plane.GetDistanceToPoint(actorPosition);

            if (distance > 0)
            {
                objectsInASpace.Add(actor.gameObject); // In front of the plane
            }
            else if (distance < 0)
            {
                objectsInBSpace.Add(actor.gameObject); // Behind the plane
            }
            else
            {
                objectsInNullSpace.Add(actor.gameObject); // On the plane
            }
        }
    }
}
