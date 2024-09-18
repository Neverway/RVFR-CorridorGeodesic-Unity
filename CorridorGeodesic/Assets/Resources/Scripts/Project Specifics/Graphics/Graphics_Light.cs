//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_Light: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField][Range(0, 1)] private float __power = 1;
    private float _power = 1;

    private float power
    {
        get { return _power; }
        set 
        {
            if (value == 0 && _power != 0)
                StartSparks();
            if (value > 0 && _power == 0)
                StopSparks();

            _power = value;
            AdjustIntensity();
        }
    }
    private float lightIntensity;
    private Color emisColor;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Light _light;
    [SerializeField] private Renderer rend;
    [SerializeField] private Material lightMat;
    [SerializeField] private ParticleSystem sparks;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        List<Material> mats = new List<Material>();

        mats.AddRange(rend.sharedMaterials);

        mats[mats.IndexOf(lightMat)] = new Material(lightMat);

        rend.sharedMaterials = mats.ToArray();

        lightIntensity = _light.intensity;
        emisColor = lightMat.GetColor("_EmissionColor");
    }
    private void OnValidate()
    {
        power = __power;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    void AdjustIntensity()
    {
        _light.intensity = lightIntensity * power;

        lightMat.SetColor("_EmissionColor", emisColor * power);
    }
    void StartSparks()
    {
        sparks.Play();
    }
    void StopSparks()
    {
        sparks.Stop();
    }
	
    //=-----------------=
    // External Functions
    //=-----------------=
}
