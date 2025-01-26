//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipView : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Transform portal1;
    public Transform portal2;
    public Material portalMaterial;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    void Update()
    {
        SetClippingPlane("_ClipPlane1", portal1);
        SetClippingPlane("_ClipPlane2", portal2);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    void SetClippingPlane(string planeName, Transform portal)
    {
        Vector3 planeNormal = portal.forward;
        Vector3 planePosition = portal.position;
        float planeDistance = -Vector3.Dot(planeNormal, planePosition);
        Vector4 planeEquation = new Vector4(planeNormal.x, planeNormal.y, planeNormal.z, planeDistance);

        portalMaterial.SetVector(planeName, planeEquation);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
