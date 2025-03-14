//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Optimizes real-time lights
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework.PawnManagement
{
    [RequireComponent(typeof(Light))]
public class Light_DistanceCulling : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private bool cullWhenOutOfRange;
    [SerializeField] private float rangeMultiplier = 1;
    [SerializeField] private bool fadeLightWhenCulled;
    [SerializeField] private float fadeSpeed = 0.2f;
    [SerializeField] private bool debugDrawCullRange;


    //=-----------------=
    // Private Variables
    //=-----------------=
    // The original intensity of the light before we began fading it out
    private float storedLightIntensity;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Light light;
    private Transform localPlayer;
    private GameInstance gameInstance;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        light = GetComponent<Light>();
        gameInstance = FindObjectOfType<GameInstance>();
        storedLightIntensity = light.intensity;
    }

    private void OnDrawGizmosSelected()
    {
        if (!light)
        {
            light = GetComponent<Light>();
            return;
        }
        Gizmos.color = new Color(0.9f,0.5f,0.0f,0.25f);
        Gizmos.DrawSphere(transform.position, light.range * rangeMultiplier);
        Gizmos.color = new Color(0.9f,0.5f,0.0f,0.4f);
        Gizmos.DrawWireSphere(transform.position, light.range * rangeMultiplier);
    }

    private void Update()
    {
        if (!GetLocalPlayer()) return;
        
        if (!cullWhenOutOfRange) return;
        if (fadeLightWhenCulled)
        {
            // Light is out of range & intensity is full
            if (!LightIsInActiveRange() && light.intensity >= storedLightIntensity)
            {
                // Fadeout
                StartCoroutine(FadeLight("out"));
            }
            // Light is in range & intensity is zero
            if (LightIsInActiveRange() && light.intensity <= 0f)
            {
                // Fadein
                StartCoroutine(FadeLight("in"));
            }
        }
        else
        {
            light.enabled = LightIsInActiveRange();
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private bool GetLocalPlayer()
    {
        if (localPlayer) return true;
        
        if (!gameInstance)
        {
            gameInstance = FindObjectOfType<GameInstance>();
            return false;
        }

        if (gameInstance.localPlayerCharacter)
        {
            localPlayer = gameInstance.localPlayerCharacter.transform;
            return false;
        }
        return false;
    }
    
    private bool LightIsInActiveRange()
    {
        if (Vector3.Distance(transform.position, localPlayer.position) <= light.range * rangeMultiplier)
        {
            return true;
        }
        return false;
    }
    
    private void SetFadedLightState()
    {
        if (light.intensity <= 0)
        {
            light.enabled = false;
        }
        else if (light.intensity > 0)
        {
            light.enabled = true;
        }
    }

    private IEnumerator FadeLight(string _mode)
    {
        float timeElapsed = 0;
        if (_mode == "in")
        {
            light.enabled = true;
            while (timeElapsed < fadeSpeed)
            {
                light.intensity = Mathf.Lerp(0, storedLightIntensity, timeElapsed / fadeSpeed);
                timeElapsed += Time.deltaTime;

                yield return null;
            }
            
            light.intensity = storedLightIntensity;
        }
        else
        {
            while (timeElapsed < fadeSpeed)
            {
                light.intensity = Mathf.Lerp(storedLightIntensity, 0, timeElapsed / fadeSpeed);
                timeElapsed += Time.deltaTime;

                yield return null;
            }
            light.enabled = false;
            light.intensity = 0;
        }
    }




    //=-----------------=
    // External Functions
    //=-----------------=
}
}
