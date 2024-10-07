using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Alt_Item_Geodesic_Utility_GeoGun;

public class Graphics_NixieBulbEffects : MonoBehaviour
{
    public static Graphics_NixieBulbEffects firstBulb;

    public Projectile_Vacumm self;

    public float sinWave1Speed = 1f;
    public float sinWave1Factor = 0.1f;
    public float sinWave2Speed = 0.1f;
    public float sinWave2Factor = 0.1f;

    [Space]
    public float noRiftFactor = 0.6f;
    public float startPreviewFactor = 5f;
    public float startPreviewTime = 1f;
    public float adjustRiftFactor = 2f;
    public float lerpSpeed = 10f;
    public Transform bulbGlowEffect;
    public Light bulbGlowLight;
    public GameObject destroyBulbEffect;
    public GameObject connectingLineEffect;

    [HideInInspector] public float glowFactor;
    [HideInInspector] public float actualFactor;
    [HideInInspector] public float previewBurstFactor;
    private float startLightIntensity;
    private bool hasDonePreviewBurst;
    private float previewBurstTimer;
    private TrailRenderer bulbTrailToDisable;
    private LineRenderer connectingLine;

    public void Awake()
    {
        startLightIntensity = bulbGlowLight.intensity;
        glowFactor = 0f;
        hasDonePreviewBurst = false;

        bulbGlowLight.intensity = 0f;
        bulbGlowEffect.localScale = Vector3.zero;

        bulbTrailToDisable = self.GetComponentInChildren<TrailRenderer>();

        if (firstBulb == null)
            firstBulb = this;
    }

    public void OnDestroy()
    {
        if (currentState != RiftState.None)
        {
            GameObject obj = Instantiate(destroyBulbEffect);
            obj.transform.position = bulbGlowEffect.position;
        }

        if (firstBulb == null)
            firstBulb = null;

        if (connectingLine != null)
            Destroy(connectingLine.gameObject);
    }

    public void Update()
    {
        if (connectingLine == null && firstBulb != null && this != firstBulb)
        {
            if (firstBulb.self.pinned && self.pinned)
            {
                connectingLine = Instantiate(connectingLineEffect).GetComponent<LineRenderer>();
            }
        }

        bulbTrailToDisable.enabled = !self.pinned;

        if (connectingLine != null && bulbGlowEffect && firstBulb && firstBulb.bulbGlowEffect)
        {
            connectingLine.SetPosition(0, bulbGlowEffect.position);
            connectingLine.SetPosition(1, firstBulb.bulbGlowEffect.position);

            connectingLine.widthMultiplier = actualFactor * actualFactor;
        }


        float targetFactor = 1f;

        if (!self.pinned)
        {
            targetFactor = 0f;
            glowFactor = 0f;
        }
        else if (currentState == RiftState.None)
        {
            hasDonePreviewBurst = false;
            previewBurstTimer = startPreviewTime;
            targetFactor = noRiftFactor;
        }
        else if (!hasDonePreviewBurst && currentState == RiftState.Preview)
        {
            hasDonePreviewBurst = true;
            glowFactor = startPreviewFactor;
        }
        else if (currentState == RiftState.Collapsing || currentState == RiftState.Expanding)
        {
            targetFactor = adjustRiftFactor;
        }

        glowFactor = Mathf.Lerp(glowFactor, targetFactor, Time.deltaTime * lerpSpeed);

        actualFactor = glowFactor;


        previewBurstFactor = Mathf.Lerp(actualFactor, startPreviewFactor, previewBurstTimer / startPreviewTime);
        previewBurstTimer -= Time.deltaTime;

        actualFactor *= previewBurstFactor;

        actualFactor *= 1 + (Mathf.Sin(Time.time * sinWave1Speed) * sinWave1Factor);
        actualFactor *= 1 + (Mathf.Sin(Time.time * sinWave2Speed) * sinWave1Factor);

        bulbGlowLight.intensity = startLightIntensity * actualFactor * actualFactor;
        bulbGlowEffect.localScale = Vector3.one * actualFactor;//* Mathf.Lerp(actualFactor, 1f, 0.5f);
    }
}
