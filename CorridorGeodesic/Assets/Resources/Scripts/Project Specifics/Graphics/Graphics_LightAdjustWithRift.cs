using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(CorGeo_ActorData))]
public class Graphics_LightAdjustWithRift : MonoBehaviour
{
    private new Light light;
    private CorGeo_ActorData actorData;
    private float homeRange;
    private float homeIntensity;
    public float ActualLerpAmount =>
        (Alt_Item_Geodesic_Utility_GeoGun.deployedRift ? Alt_Item_Geodesic_Utility_GeoGun.lerpAmount : 0f);

    public float DistanceFactorToPlane
    {
        get
        {
            float distance = 0f;
            switch (actorData.space)
            {
                case CorGeo_ActorData.Space.None: return 1f;
                case CorGeo_ActorData.Space.Null: return 0f;
                case CorGeo_ActorData.Space.A: 
                    distance = Alt_Item_Geodesic_Utility_GeoGun.planeA.GetDistanceToPoint(transform.position); 
                    break;
                case CorGeo_ActorData.Space.B: 
                    distance = Alt_Item_Geodesic_Utility_GeoGun.planeB.GetDistanceToPoint(transform.position); 
                    break;
            }
            return 1f - Mathf.Clamp(distance / homeRange, 0f, 1f);
        }
    }

    void Awake()
    {
        light = GetComponent<Light>();
        actorData = GetComponent<CorGeo_ActorData>();
        homeRange = light.range;
        homeIntensity = light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        float distFactor = DistanceFactorToPlane;
        if (distFactor > 0f)
        {
            float lerpAmount = ActualLerpAmount * 0.3f * distFactor;
            light.range = Mathf.Lerp(homeRange, 0f, lerpAmount);
            light.intensity = Mathf.LerpUnclamped(homeIntensity, 0f, lerpAmount);
        }
        else
        {
            light.range = homeRange;
            light.intensity = homeIntensity;
        }
    }
}
