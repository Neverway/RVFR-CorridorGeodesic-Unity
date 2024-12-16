using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_LightFlicker : MonoBehaviour
{
    public Light lightComponent;
    private float startingIntensity;
    private float timer;
    private bool turnedOnCycle;


    public void Awake()
    {
        startingIntensity = lightComponent.intensity;
    }

    public void Update()
    {
        lightComponent.intensity = turnedOnCycle ? startingIntensity : Mathf.Lerp(startingIntensity * 0.8f, startingIntensity * 0.6f, timer * 2f);
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            turnedOnCycle = !turnedOnCycle;
            timer = turnedOnCycle ? Random.Range(.1f, .5f) : Random.Range(.1f, .2f);
        }
    }
}
