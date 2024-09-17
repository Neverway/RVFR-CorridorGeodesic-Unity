//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

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


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        laserLight.SetActive(false);
    }

    private void Update ()
    {
        timeSinceHit += Time.deltaTime;
        if (timeSinceHit > timeToCancelHit)
        {
            if (isActive)
            {
                isActive = false;
                isPowered = false;
                OnNotPowered.Invoke();
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
        }
    }

}