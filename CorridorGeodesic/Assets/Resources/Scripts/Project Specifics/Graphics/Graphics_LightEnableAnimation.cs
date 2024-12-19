using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_LightEnableAnimation : MonoBehaviour
{
    public Light lightComponent;
    private float startingIntensity;
    private float timer;


    public void Awake()
    {
        startingIntensity = lightComponent.intensity;
    }

    public void OnEnable()
    {
        timer = Random.Range(-1.8f, 0f);
    }

    public void OnValidate()
    {
        lightComponent = GetComponent<Light>();
    }
    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            lightComponent.intensity = 0f;
        }
        else if (timer <= 0.35f)
        {
            lightComponent.intensity = startingIntensity * 0.6f;
        }
        else if (timer <= 1f)
        {
            lightComponent.intensity = 0f;

        }
        else if (timer <= 1.5f)
        {
            lightComponent.intensity = Mathf.Lerp(startingIntensity, startingIntensity * 1.5f, (timer - 1f)*2f);
        }
        else if (timer <= 5f)
        {
            lightComponent.intensity = Mathf.Lerp(startingIntensity * 1.5f, startingIntensity, (timer - 1.5f)/3.5f);
        }

        timer += Time.deltaTime * 3f; //animation was too slow so I sped it up by multiplying by 3
    }
}
