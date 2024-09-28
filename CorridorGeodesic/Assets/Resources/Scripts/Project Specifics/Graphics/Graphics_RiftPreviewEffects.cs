using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_RiftPreviewEffects : MonoBehaviour
{
    public Alt_Item_Geodesic_Utility_GeoGun geoGun;

    public float edgeStrength = 1.3f;
    public float opacity = 0.05f;
    public float edgeStrengthFactorPower = 1f;
    public float opacityFactorPower = 2f;
    

    public void Update()
    {
        Graphics_NixieBulbEffects bulb = Graphics_NixieBulbEffects.firstBulb;
        if (bulb == null)
            return;

        float lerpFactor = bulb.glowFactor * bulb.previewBurstFactor;

        foreach(GameObject obj in geoGun.cutPreviews)
        {
            Material mat = obj.GetComponentInChildren<Renderer>().sharedMaterial;

            mat.SetFloat("_edgeStrength", edgeStrength * Mathf.Pow(lerpFactor, edgeStrengthFactorPower));
            mat.SetFloat("_opacity", opacity * Mathf.Pow(lerpFactor, opacityFactorPower));
        }

        //geoGun.cutPreviews[0].SetActive(Alt_Item_Geodesic_Utility_GeoGun.currentState != RiftState.Closed);
    }
}
