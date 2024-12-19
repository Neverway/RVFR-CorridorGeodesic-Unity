//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_UnityExtents
{
    public static Vector3 ClampVector3(Vector3 value, Vector3 min, Vector3 max)
    {
        Vector3 result = new Vector3(
            Mathf.Clamp(value.x, min.x, max.x),
            Mathf.Clamp(value.y, min.y, max.y),
            Mathf.Clamp(value.z, min.z, max.z));

        return result;
    }
}