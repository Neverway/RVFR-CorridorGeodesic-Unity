//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser_Detector : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=

    //=-----------------=
    // Private Variables
    //=-----------------=

    private float timeSinceHit = 0f;
    private float timeToCancelHit = 0.1f;
    [SerializeField] private GameObject laserLight;
    private bool isActive = false;

    //=-----------------=
    // Reference Variables
    //=-----------------=

    public UnityEvent OnPowered;
    public UnityEvent OnNotPowered;
    [SerializeField] private Material glassOff;
    [SerializeField] private Material glassOn;
    [SerializeField] private MeshRenderer lampRenderer;
    private List<Material> materials = new List<Material>();


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        lampRenderer.GetMaterials (materials);
        laserLight.SetActive(false);
        materials[2] = glassOff;
        lampRenderer.SetMaterials (materials);
    }

    private void Update ()
    {
        timeSinceHit += Time.deltaTime;
        if (timeSinceHit > timeToCancelHit)
        {
            if (isActive)
            {
                OnNotPowered.Invoke();
                isActive = false;
                isPowered = false;
                laserLight.SetActive (false);
                materials[2] = glassOff;
                lampRenderer.SetMaterials (materials);
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=

    public void OnHit ()
    {
        timeSinceHit = 0f;
        if (!isActive)
        {
            OnPowered.Invoke();
            isActive = true;
            isPowered = true;
            laserLight.SetActive(true);
            materials[2] = glassOn;
            lampRenderer.SetMaterials (materials);
        }
    }

}