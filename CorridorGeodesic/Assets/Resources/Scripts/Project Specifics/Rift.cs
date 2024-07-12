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
    public void DivergePortals(float distance, float speed)
    {
        portalA.transform.localPosition = Vector3.Lerp(portalA.transform.localPosition, -Vector3.forward*distanceToMarker, Time.deltaTime);
        portalB.transform.localPosition = Vector3.Lerp(portalB.transform.localPosition, Vector3.forward*distanceToMarker, Time.deltaTime);
    }

    public void GetObjectSpacialLists()
    {
        objectsInASpace.Clear();
        objectsInBSpace.Clear();
        objectsInNullSpace.Clear();
        
        foreach (var actor in FindObjectsOfType<ActorData>())
        {
            var distanceVector = actor.transform.position.z -
            if (CenterPlane.transform.localPosition.z>)
            {
                
            }
        }
    }
}
