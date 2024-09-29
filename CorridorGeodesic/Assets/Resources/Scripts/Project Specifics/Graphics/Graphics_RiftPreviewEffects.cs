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
    public float burstFactorPower = 0.7f;

    public void Update()
    {
        Graphics_NixieBulbEffects bulb = Graphics_NixieBulbEffects.firstBulb;
        if (bulb == null)
            return;

        float lerpFactor = bulb.glowFactor * Mathf.Pow(bulb.previewBurstFactor, burstFactorPower);

        GameObject obj = geoGun.cutPreviews[0];
        Material mat = obj.GetComponentInChildren<Renderer>().sharedMaterial;

        mat.SetFloat("_edgeStrength", edgeStrength * Mathf.Pow(lerpFactor, edgeStrengthFactorPower));
        mat.SetFloat("_opacity", opacity * Mathf.Pow(lerpFactor, opacityFactorPower));

        float newEdgeStrength = edgeStrength * Mathf.Pow(lerpFactor, edgeStrengthFactorPower);
        float newOpacity = opacity * Mathf.Pow(lerpFactor, opacityFactorPower);

        if (Alt_Item_Geodesic_Utility_GeoGun.currentState == RiftState.Closed)
        {
            newEdgeStrength *= 1.5f;
            newOpacity *= 1.5f;
        }

        SetPreview(geoGun.cutPreviews[0], newEdgeStrength, newOpacity);
        SetPreview(geoGun.cutPreviews[1], newEdgeStrength, newOpacity);

        //geoGun.cutPreviews[0].SetActive(Alt_Item_Geodesic_Utility_GeoGun.currentState != RiftState.Closed);
    }

    public void SetPreview(GameObject preview, float edgeStrength, float opacity)
    {
        Material mat = preview.GetComponentInChildren<Renderer>().sharedMaterial;

        mat.SetFloat("_edgeStrength", edgeStrength);
        mat.SetFloat("_opacity", opacity);
    }
}
