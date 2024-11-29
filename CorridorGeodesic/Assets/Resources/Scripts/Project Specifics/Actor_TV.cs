//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Neverway.Framework.LogicSystem;

public class Actor_TV : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [LogicComponentHandle] public LogicComponent input;
    public Material backlightUnpowered, backlightPowered, overlayUnpowered, overlayPowered;
    public UnityEvent OnPowered;
    public UnityEvent OnUnpowered;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private SkinnedMeshRenderer tvBacklight;
    [SerializeField] private SkinnedMeshRenderer tvOverlay;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        isPowered = false;
        
        List<Material> mats = new List<Material>();
        mats.AddRange(tvBacklight.sharedMaterials);
        mats[1] = backlightUnpowered;
        tvBacklight.sharedMaterials = mats.ToArray();
        
        tvOverlay.material = overlayUnpowered;
        OnUnpowered.Invoke();
    }

    private void Update()
    {
    }
    
    public override void SourcePowerStateChanged(bool powered)
    {
        if (input.isPowered)
        {
            isPowered = true;
            
            List<Material> mats = new List<Material>();
            mats.AddRange(tvBacklight.sharedMaterials);
            mats[1] = backlightPowered;
            tvBacklight.sharedMaterials = mats.ToArray();
            
            tvOverlay.material = overlayPowered;
            OnPowered.Invoke();
        }
        else
        {
            isPowered = false;
            
            List<Material> mats = new List<Material>();
            mats.AddRange(tvBacklight.sharedMaterials);
            mats[1] = backlightUnpowered;
            tvBacklight.sharedMaterials = mats.ToArray();
            
            tvOverlay.material = overlayUnpowered;
            OnUnpowered.Invoke();
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
