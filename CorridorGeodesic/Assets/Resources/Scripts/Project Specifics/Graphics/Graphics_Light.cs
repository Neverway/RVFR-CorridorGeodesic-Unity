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
    private float _power = 1;

    private float power
    {
        get { return _power; }
        set 
        {
            if (value == 0 && _power != 0)
                sparks.Play();

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
    [SerializeField] Graphics_ChangeMaterialProperty materialPropertyChange;
    [SerializeField] private ParticleSystem sparks;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        lightIntensity = _light.intensity;
    }
    private void Start()
    {
        emisColor = materialPropertyChange.material.GetColor("_EmissionColor");
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    void AdjustIntensity()
    {
        _light.intensity = lightIntensity * power;

        materialPropertyChange.ChangePropertyManual(emisColor * power);
    }
    IEnumerator SetLight(float value)
    {
        while (power != value)
        {
            power = Mathf.MoveTowards(power, value, Time.deltaTime * 0.5f);
            yield return null;
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void TurnOffLight()
    {
        StopAllCoroutines();
        StartCoroutine(SetLight(0));
    }
    public void TurnOnLight()
    {
        StopAllCoroutines();
        StartCoroutine(SetLight(1));
    }
}
