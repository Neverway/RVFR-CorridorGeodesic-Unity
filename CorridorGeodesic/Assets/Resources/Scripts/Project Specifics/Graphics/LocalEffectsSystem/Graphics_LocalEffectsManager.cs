//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_LocalEffectsManager: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static Graphics_LocalEffectsManager Instance;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private ParticleSystem dustParticles;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private ParticleSystem rainParticles;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        Instance = this;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SetEffectSetting(LocalEffectSetting setting)
    {
        dustParticles.gameObject.SetActive(false);
        smokeParticles.gameObject.SetActive(false);
        rainParticles.gameObject.SetActive(false);
        switch (setting)
        {
            case LocalEffectSetting.Clean:
                break;
            case LocalEffectSetting.Rusty:
                dustParticles.gameObject.SetActive(true);
                smokeParticles.gameObject.SetActive(true);
                break;
            case LocalEffectSetting.Dusty:
                dustParticles.gameObject.SetActive(true);
                break;
            case LocalEffectSetting.Rainy:
                rainParticles.gameObject.SetActive(true);
                break;
            case LocalEffectSetting.Stormy:
                smokeParticles.gameObject.SetActive(true);
                rainParticles.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}