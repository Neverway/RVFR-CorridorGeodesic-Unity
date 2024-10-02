//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_ParticleDecalSplatter : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private float splatterScale;
    private bool useGradient;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private GameObject splatterDecal;

    private ParticleSystem particles;
    private Gradient particleGradient;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        if (particles == null)
            particles = GetComponent<ParticleSystem>();

        if (particles.main.startColor.mode == ParticleSystemGradientMode.TwoColors)
        {
            particleGradient = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0].alpha = 1;
            alphaKeys[1].alpha = 1;

            colorKeys[0].color = particles.main.startColor.colorMin;
            colorKeys[1].color = particles.main.startColor.colorMax;

            colorKeys[0].time = 0;
            colorKeys[1].time = 1;

            particleGradient.SetKeys(colorKeys, alphaKeys);
            useGradient = true;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (!splatterDecal)
            return;

        int collisionAmount = particles.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < collisionAmount; i++)
        {
            if (Physics.Raycast(collisionEvents[i].intersection + collisionEvents[i].normal * 0.5f, -collisionEvents[i].normal, 
                out RaycastHit hit, 2, particles.collision.collidesWith))
            {
                OnWorldCollision(hit);
            }
        }
    }
    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void OnWorldCollision(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out Multi_Oilable oilable))
            CollidedWithOilable(oilable);
        else
            CollidedWithSurface(hit);

        Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.liquidSplatter, hit.point);
    }
    void CollidedWithOilable(Multi_Oilable oilable)
    {
        oilable.AddOil(0.2f);
    }
    void CollidedWithSurface(RaycastHit hit)
    {
        GameObject newDecal = Tool_ObjectPool.Instance.AddToDecalPool(splatterDecal, hit.point + hit.normal * 0.01f, Quaternion.identity);

        newDecal.transform.up = hit.normal;
        newDecal.transform.Rotate(Vector3.up, Random.Range(-360, 360), Space.Self);

        newDecal.transform.localScale = Vector3.one * splatterScale;

        if (useGradient)
            newDecal.GetComponent<Graphics_DecalRandomizer>().InitializeDecal(particleGradient.Evaluate(Random.Range(0, 1f)));
        else
            newDecal.GetComponent<Graphics_DecalRandomizer>().InitializeDecal(particles.main.startColor.color);

        OilManager.Instance.AddOilSplatter(newDecal.transform);
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
