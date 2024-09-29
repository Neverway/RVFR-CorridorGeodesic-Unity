using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_RiftPreviewEffects : MonoBehaviour
{
    [SerializeField] private Alt_Item_Geodesic_Utility_GeoGun geoGun;

    [SerializeField] private Material riftPreview;
    [SerializeField] private float defaultEmisStrength = 6;
    [SerializeField] private float collapseExpandEmisStrength = 9;

    //public float edgeStrength = 1.3f;
    //public float opacity = 0.05f;
    //public float edgeStrengthFactorPower = 1f;
    //public float opacityFactorPower = 2f;
    //public float burstFactorPower = 0.7f;

    private void OnEnable()
    {
        Alt_Item_Geodesic_Utility_GeoGun.OnStateChanged += OnStateChanged;
    }
    private void OnDestroy()
    {
        Alt_Item_Geodesic_Utility_GeoGun.OnStateChanged -= OnStateChanged;
        riftPreview.SetFloat("_EffectTime", 0);
        riftPreview.SetFloat("_SphereSize", 0);
        riftPreview.SetFloat("_EmissionStrength", defaultEmisStrength);
    }
    public void Update()
    {
        //Graphics_NixieBulbEffects bulb = Graphics_NixieBulbEffects.firstBulb;
        //if (bulb == null)
        //    return;

        //float lerpFactor = bulb.glowFactor * Mathf.Pow(bulb.previewBurstFactor, burstFactorPower);

        //GameObject obj = geoGun.cutPreviews[0];
        //Material mat = obj.GetComponentInChildren<Renderer>().sharedMaterial;

        //mat.SetFloat("_edgeStrength", edgeStrength * Mathf.Pow(lerpFactor, edgeStrengthFactorPower));
        //mat.SetFloat("_opacity", opacity * Mathf.Pow(lerpFactor, opacityFactorPower));

        //float newEdgeStrength = edgeStrength * Mathf.Pow(lerpFactor, edgeStrengthFactorPower);
        //float newOpacity = opacity * Mathf.Pow(lerpFactor, opacityFactorPower);

        //if (Alt_Item_Geodesic_Utility_GeoGun.currentState == RiftState.Closed)
        //{
        //    newEdgeStrength *= 1.5f;
        //    newOpacity *= 1.5f;
        //}

        //SetPreview(geoGun.cutPreviews[0], newEdgeStrength, newOpacity);
        //SetPreview(geoGun.cutPreviews[1], newEdgeStrength, newOpacity);

        //geoGun.cutPreviews[0].SetActive(Alt_Item_Geodesic_Utility_GeoGun.currentState != RiftState.Closed);
    }

    void OnStateChanged()
    {
        if (Alt_Item_Geodesic_Utility_GeoGun.currentState != RiftState.None && Alt_Item_Geodesic_Utility_GeoGun.previousState == RiftState.None)
        {
            StopAllCoroutines();
            StartCoroutine(OnRiftCreated());
        }

        switch (Alt_Item_Geodesic_Utility_GeoGun.currentState)
        {
            case RiftState.None:
                StopAllCoroutines();
                riftPreview.SetFloat("_EffectTime", 0);
                riftPreview.SetFloat("_SphereSize", 0);
                riftPreview.SetFloat("_EmissionStrength", defaultEmisStrength);
                break;
            case RiftState.Preview:
                break;
            case RiftState.Collapsing:
                riftPreview.SetFloat("_EmissionStrength", collapseExpandEmisStrength);
                break;
            case RiftState.Closed:
                riftPreview.SetFloat("_EmissionStrength", defaultEmisStrength);
                break;
            case RiftState.Expanding:
                riftPreview.SetFloat("_EmissionStrength", collapseExpandEmisStrength);
                break;
            case RiftState.Idle:
                riftPreview.SetFloat("_EmissionStrength", defaultEmisStrength);
                break;
            default:
                break;
        }
    }
    IEnumerator OnRiftCreated()
    {
        riftPreview.SetFloat("_EffectTime", 0);
        riftPreview.SetFloat("_SphereSize", 0);
        riftPreview.SetFloat("_EmissionStrength", defaultEmisStrength);

        Vector3 bulbA = geoGun.cutPreviews[0].transform.position;
        Vector3 bulbB = geoGun.cutPreviews[1].transform.position;

        riftPreview.SetVector("_BulbsCenter", (bulbA + bulbB) * 0.5f);
        riftPreview.SetFloat("_SphereSize", Vector3.Distance(bulbA, bulbB) * 0.5f);

        float effectTimer = 0;

        while (effectTimer < 1)
        {
            riftPreview.SetFloat("_EffectTime", effectTimer);
            effectTimer += Time.deltaTime;
            yield return null;
        }

        riftPreview.SetFloat("_EffectTime", 1);
    }
    public void SetPreview(GameObject preview, float edgeStrength, float opacity)
    {
        Material mat = preview.GetComponentInChildren<Renderer>().sharedMaterial;

        mat.SetFloat("_edgeStrength", edgeStrength);
        mat.SetFloat("_opacity", opacity);
    }
}
