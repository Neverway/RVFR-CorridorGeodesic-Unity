using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargerLights : MonoBehaviour
{
    public float factor;

    public void Awake()
    {
        Light light = GetComponent<Light>();
        if (light == null)
            return;

        light.range *= factor;
        light.intensity *= factor;
    }
}
