using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerLight : MonoBehaviour
{
    public Color newColor;
    public float newIntensity;
    public float newRange;

    private new Light light;

    public void Update()
    {
        if (light == null)
        {
            Pawn pawn = FindAnyObjectByType<Pawn>();
            if (pawn == null) return;

            light = pawn.GetComponentInChildren<Light>();
            if (light == null) return;
        }
        else
        {
            light.range = newRange;
            light.intensity = newIntensity;
            light.color = newColor;
        }
    }
}
